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
		[Export] private int maxSpeed = 3;
		[Export] private float sprintMultiplier = 1.5f;
		[Export] private int acceleration = 4;
		[Export] private int deacceleration = 6;
		[Export] private int jumpSpeed = 3;      

		private Vector3 velocity;
		private Vector3 rotation;

		// Child Nodes
		public Spatial Neck { get { return (Spatial)GetNode("Neck"); } }
		public Spatial Head { get { return (Spatial)Neck.GetNode("Head"); } }
		public Camera Camera { get { return (Camera)Head.GetNode("Camera"); } }

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			Input.SetMouseMode(Input.MouseMode.Captured);
			mouseMotion = new InputEventMouseMotion();
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _PhysicsProcess(float delta)
		{
			Walk(delta);
			//Fly(delta);
			MouseMotion(mouseMotion);
		}

		public override void _UnhandledInput(InputEvent e)
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
			if (rotation.Dot(planeVelocity) > 0)
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
