using Godot;

namespace MarsMission
{
    public class Player : KinematicBody
    {
        // Camera varibles
        private const float mouseSensitivity = 0.006F;
        private float cameraAngle = 0;
        private InputEventMouseMotion mouseMotion;

        // Fly kinematic variables
        private const int flySpeed = 5;
        private const float flyAccerlation = 1.5f;

        // Walk variables
        private const float gravity = -9.8f;
        private const int maxSpeed = 3;
        private const float sprintMultiplier = 1.5f;
        private const int acceleration = 4;
        private const int deacceleration = 6;
        private const int jumpSpeed = 4;

        private Vector3 velocity;
        private Vector3 rotation;

        // Child Nodes
        public Spatial Neck { get { return (Spatial)GetNode("Neck"); } }
        private Spatial Head { get { return (Spatial)Neck.GetNode("Head"); } }
        public Camera Camera { get { return (Camera)Head.GetNode("Camera"); } }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            Input.SetMouseMode(Input.MouseMode.Captured);
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _PhysicsProcess(float delta)
        {
            Walk(delta);
            MouseMotion(mouseMotion);
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event is InputEventMouseMotion motion && Input.GetMouseMode() == Input.MouseMode.Captured)
            {
                mouseMotion = motion;
            }
        }

        private void MouseMotion(InputEventMouseMotion motion)
        {
            if (motion.Relative.Length() > 0)
            {
                //Neck.RotateY(-motion.Relative.x * mouseSensitivity);
                RotateY(-motion.Relative.x * mouseSensitivity);

                float angleChange = -motion.Relative.y * mouseSensitivity;
                if ((angleChange + cameraAngle) < Mathf.Pi / 2 && (angleChange + cameraAngle) > -Mathf.Pi / 2)
                {
                    Neck.RotateX(angleChange);
                    cameraAngle += angleChange;
                }

                mouseMotion.Relative = new Vector2();
            }
        }

        private void Walk(float delta)
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

            velocity = MoveAndSlide(velocity, Vector3.Up);

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