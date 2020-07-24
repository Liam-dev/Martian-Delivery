using Godot;
using System;

namespace MartianDelivery
{
	public class HoverSensor : Spatial
	{
		[Export] private float strengh = 250;
		[Export] private float hoverHeight = 8;

		protected RayCast Ray
		{
			get { return (RayCast)GetNode("Ray"); }
		}

		protected Ship Parent
		{
			get { return (Ship)GetParent(); }
		}

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			Ray.CastTo = Vector3.Down * hoverHeight;
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _PhysicsProcess(float delta)
		{
			if (Ray.IsColliding())
			{
				Vector3 ground = GlobalTransform.XformInv(Ray.GetCollisionPoint());
				float displacement = (Ray.CastTo - ground).Length();
				Vector3 force = Vector3.Up * displacement * Parent.Mass * strengh * delta;
				Vector3 globalForce = Parent.GlobalTransform.basis.Xform(force);
				Parent.AddForce(globalForce, GlobalTransform.origin - Parent.GlobalTransform.origin);
			}				
		}
	}
}
