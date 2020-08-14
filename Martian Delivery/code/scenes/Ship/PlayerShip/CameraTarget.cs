using Godot;
using System;

namespace MartianDelivery
{
	public class CameraTarget : Spatial
	{
		// Declare member variables here. Examples:
		// private int a = 2;
		// private string b = "text";

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{

		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(float delta)
		{				
			Vector3 eulerAngles = GlobalTransform.basis.GetEuler();
			Vector3 newAngles = new Vector3(eulerAngles.x, eulerAngles.y, 0);
			Basis basis = new Basis(newAngles);
			GlobalTransform = new Transform(basis, GlobalTransform.origin);			
		}
	}
}
