using Godot;
using System;

namespace MartianDelivery
{
	public class Player : KinematicBody
	{
		// Camera varibles
		[Export] private float mouseSensitivity = 0.006F;
		private float cameraAngle = 0;
		private InputEventMouseMotion mouseMotion;

		// Fly kinematic variables
		[Export] private int flySpeed = 5;
		[Export] private float flyAccerlation = 1.5f;

		// Walk variables
		[Export] private float gravity = -(float)ProjectSettings.GetSetting("physics/3d/default_gravity");
		[Export] private int maxSpeed = 4;
		[Export] private float sprintMultiplier = 1.5f;
		[Export] private int acceleration = 4;
		[Export] private int deacceleration = 6;
		[Export] private int jumpSpeed = 5;
		
		public float PlaneRotation
		{
			get
			{
				if (driving)
				{
					return vehicle.GlobalTransform.basis.GetEuler().y;
				}
				else
				{
					return GlobalTransform.basis.GetEuler().y;
				}				
			}
		}

		private Vector3 velocity;
		private Vector3 rotation;
		public bool driving = false;
		public PlayerShip vehicle;

		// Child Nodes
		public CollisionShape CollisionShape { get { return (CollisionShape)GetNode("CollisionShape"); } }
		public Spatial Neck { get { return (Spatial)GetNode("Neck"); } }
		public Spatial Head { get { return (Spatial)Neck.GetNode("Head"); } }
		public Camera Camera { get { return (Camera)Head.GetNode("Camera"); } }
		public RayCast LineOfSight { get { return (RayCast)Head.GetNode("LineOfSight"); } }
		public UserInterface UserInterface { get { return (UserInterface)GetNode("UserInterface"); } }

		private Inventory inventory;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			Input.SetMouseMode(Input.MouseMode.Captured);
			mouseMotion = new InputEventMouseMotion();
			inventory = new Inventory();
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _PhysicsProcess(float delta)
		{
			if (!driving)
			{
				Walk(delta);
				MouseMotion(mouseMotion);
			}
			else
			{
				GlobalTransform = new Transform(GlobalTransform.basis, vehicle.GlobalTransform.origin);

				if (Input.IsActionJustPressed("abandon_vehicle"))
				{
					AbandonVehicle();
				}
			}

			CheckSelectables();
		}

		protected void CheckSelectables()
		{
			if (LineOfSight.IsColliding() && Visible)
			{
				Node collider = (Node)LineOfSight.GetCollider();
				if (collider.GetParent() is ISelectable selectable && collider.Name == "SelectionArea")
				{
					UserInterface.Tooltip.Show(selectable.TooltipDescription);

					if (Input.IsActionJustPressed("interact"))
					{
						if (!driving && collider.GetParent() is PlayerShip vehicle)
						{
							EnterVehicle(vehicle);
						}
					}
				}
				else
				{
					UserInterface.Tooltip.Fade();
				}
			}
			else
			{
				UserInterface.Tooltip.Fade();
			}
		}

		public override void _Input(InputEvent e)
		{
			if (e is InputEventMouseMotion motion && Input.GetMouseMode() == Input.MouseMode.Captured)
			{
				mouseMotion = motion;
			}
		}

		private void MouseMotion(InputEventMouseMotion motion)
		{
			if (motion.Relative.Length() > 0)
			{
				float angleChange = -motion.Relative.y * mouseSensitivity;
				float newAngle = angleChange + cameraAngle;
				if ((newAngle) < Mathf.Pi / 2 && (newAngle) > -Mathf.Pi / 2)
				{
					Neck.RotateX(angleChange);
					cameraAngle += angleChange;
				}

				RotateY(-motion.Relative.x * mouseSensitivity);

				mouseMotion.Relative = new Vector2();
			}          
		}

		private void Walk(float delta)
		{
			// Reset rotation of the player input
			rotation = new Vector3();

			// Get rotation of camera
			Basis facing = Transform.basis;

			// Get input
			if (Input.IsActionPressed("move_forward"))
			{
				rotation -= facing.z;
			}
			if (Input.IsActionPressed("move_backward"))
			{
				rotation += facing.z;
			}
			if (Input.IsActionPressed("strafe_left"))
			{
				rotation -= facing.x;
			}
			if (Input.IsActionPressed("strafe_right"))
			{
				rotation += facing.x;
			}

			rotation = rotation.Normalized();

			velocity.y += gravity * delta;
			Vector3 planeVelocity = new Vector3(velocity.x, 0, velocity.z);

			float speed;
			if (Input.IsActionPressed("sprint"))
			{
				speed = maxSpeed * sprintMultiplier;
			}			
			else
			{
				speed = maxSpeed;
			}

			Vector3 target = rotation * speed;

			float currentAcceleration;
			if (rotation.Dot(planeVelocity) > 0 || IsOnFloor())
			{
				currentAcceleration = acceleration;
			}
			else
			{
				currentAcceleration = deacceleration;
			}

			planeVelocity = planeVelocity.LinearInterpolate(target, currentAcceleration * delta);

			
			velocity.x = planeVelocity.x;
			velocity.z = planeVelocity.z;

			velocity = MoveAndSlide(velocity, Vector3.Up, infiniteInertia: false);

			if (Input.IsActionPressed("jump") && IsOnFloor())
			{
				velocity.y = jumpSpeed;
			}
		}

		private void EnterVehicle(PlayerShip ship)
		{
			vehicle = ship;
			vehicle.Occupy(this);
			driving = true;
			Visible = false;
			CollisionShape.Disabled = true;
			Camera.Current = false;
		}

		private void AbandonVehicle()
		{	
			vehicle.Abandon();
			driving = false;
			GlobalTransform = new Transform(GlobalTransform.basis, vehicle.GlobalTransform.origin + new Vector3(0, 5, 0));
			Visible = true;
			CollisionShape.Disabled = false;
			Camera.Current = true;			
		}

		private void Fly(float delta)
		{
			// Reset rotation of the player
			rotation = new Vector3();

			// Get rotation of camera
			Basis facing = Camera.GetCameraTransform().basis;

			// Get input
			if (Input.IsActionPressed("move_forward"))
			{
				rotation -= facing.z;
			}
			if (Input.IsActionPressed("move_backward"))
			{
				rotation += facing.z;
			}
			if (Input.IsActionPressed("strafe_left"))
			{
				rotation -= facing.x;
			}
			if (Input.IsActionPressed("strafe_right"))
			{
				rotation += facing.x;
			}

			rotation = rotation.Normalized();
			Vector3 target = rotation * flySpeed;
			velocity = velocity.LinearInterpolate(target, flyAccerlation * delta);

			MoveAndSlide(velocity);
		}


	}
}
