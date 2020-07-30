using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MartianDelivery
{
	public class PlayerShip : Ship, ISelectable
	{
		[Export] public string TooltipDescription { get; set; }

		[Export] private int yawThrust = 150000;

		public MeshInstance Exhaust { get { return GetNode<MeshInstance>("Exhaust"); } }
		private static List<Curve> colourCurves = new List<Curve>();
		private static List<(int[], int)> exhaustColours = new List<(int[], int)>
		{ 
			(new int[] { 68, 68, 68 }, 0),
			(new int[] { 237, 57, 12 }, 1) 
		};

		private bool occupied = false;
		private Player driver;
		[Signal] public delegate void Occupied();
		[Signal] public delegate void Abandoned();

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			base._Ready();

			thrusters = new List<Thruster>()
			{
				new CentralThruster(this, Vector3.Back, 1000, new List<ThrustAction>(){ ThrustAction.All, ThrustAction.Forward })
			};
			
			foreach ((int[] colour, int speed) speedColour in exhaustColours)
			{
				for (int i = 0; i < 3; i++)
				{
					colourCurves.Add(new Curve());
					colourCurves[i].AddPoint(new Vector2(speedColour.speed, speedColour.colour[i]));
				}
			}

			foreach(Curve curve in colourCurves)
			{
				curve.Bake();
			}
		}

		private void ActivateThrusterSet(ThrustAction action, float power)
		{
			foreach (Thruster thruster in thrusters)
			{
				if (thruster.Actions.Contains(action))
				{
					thruster.Activate(power);
				}
			}
		}

		private void DeactivateThrusterSet(ThrustAction action)
		{
			foreach (Thruster thruster in thrusters)
			{
				if (thruster.Actions.Contains(action))
				{
					thruster.Deactivate();
				}
			}
		}

		private void Control()
		{
			if (Input.IsActionPressed("move_forward"))
			{
				ActivateThrusterSet(ThrustAction.Forward, 0.75f);
			}
			else
			{
				DeactivateThrusterSet(ThrustAction.Forward);
			}

			int yawDir = 0;
			if (Input.IsActionPressed("strafe_right"))
			{
				yawDir--;
			}
			if (Input.IsActionPressed("strafe_left"))
			{
				yawDir++;
			}

			AddTorque(Transform.basis.y * yawDir * yawThrust);
		}

		private void UpdateExhaust()
		{
			int[] colour = { 68, 68, 68 };

			for (int i = 0; i < 3; i++)
			{
				colour[i] = (int)colourCurves[i].Interpolate(LinearVelocity.Length() / 10);
			}

			SpatialMaterial exhaustMaterial = (SpatialMaterial)Exhaust.GetSurfaceMaterial(0);
			//exhaustMaterial.AlbedoColor = new Color(colour[0], colour[1], colour[2]);
			exhaustMaterial.AlbedoColor = new Color(68, 68, 68);
			//Exhaust.SetSurfaceMaterial(0, exhaustMaterial);
		}

		public void Occupy(Player driver)
		{
			occupied = true;
			this.driver = driver;
			EmitSignal(nameof(Occupied));
		}

		public void Abandon()
		{
			occupied = false;
			driver = null;
			EmitSignal(nameof(Abandoned));
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _PhysicsProcess(float delta)
		{ 	
			if (occupied)
			{
				Control();
			}
			
			//UpdateExhaust();
			base._PhysicsProcess(delta);
		}
	}
}
