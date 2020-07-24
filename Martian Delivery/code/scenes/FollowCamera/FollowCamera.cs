using Godot;
using System;

namespace MartianDelivery
{
	public class FollowCamera : InterpolatedCamera
	{
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{

		}

		//// Called every frame. 'delta' is the elapsed time since the previous frame.
		//public override void _Process(float delta)
		//{
			
		//}

		private void _on_PlayerShip_Occupied()
		{
			Current = true;
		}

		private void _on_PlayerShip_Abandoned()
		{
			Current = false;
		}
	}
}
