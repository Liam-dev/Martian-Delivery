using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MartianDelivery
{
	public class PlayerShip : Ship
	{
		private int yawThrust = 150000;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			thrusters = new List<Thruster>()
			{
				new CentralThruster(this, Vector3.Back, 1000, new List<ThrustAction>(){ ThrustAction.All, ThrustAction.Forward })
			};
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
			if (Input.IsActionPressed("drive"))
			{
				ActivateThrusterSet(ThrustAction.Forward, 0.75f);
			}
			else
			{
				DeactivateThrusterSet(ThrustAction.Forward);
			}

			int yawDir = 0;
			if (Input.IsActionPressed("yaw_right"))
			{
				yawDir--;
			}
			if (Input.IsActionPressed("yaw_left"))
			{
				yawDir++;
			}

			AddTorque(Transform.basis.y * yawDir * yawThrust);
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _PhysicsProcess(float delta)
		{
			Control();
			base._PhysicsProcess(delta);
		}
	}
}
