using Godot;
using System;
using System.Collections.Generic;

namespace MartianDelivery
{
	public class Ship : RigidBody
	{
		//[Export] private float hoverHeight = 5;

		// Child Nodes
		protected MeshInstance Mesh
		{
			get { return (MeshInstance)GetNode("Mesh"); }
		}

		/*
		protected RayCast HoverSensor
		{
			get { return (RayCast)GetNode("HoverSensor"); }
		}
		*/

		protected List<Thruster> thrusters;

		public Ship() { }
		public Ship(int thrust, List<Thruster> thrusters)
		{
			this.thrusters = thrusters;
		}

		public override void _Ready()
		{
			base._Ready();
		}


		// OLD
		/*
		// Allign the ship's y-axis with a direction
		protected Transform AlignYAxis(Vector3 direction)
		{
			Transform transform = GlobalTransform;
			transform.basis.y = direction;
			transform.basis.x = -transform.basis.z.Cross(direction);
			transform.basis = transform.basis.Orthonormalized();

			return transform;
		}
		
		protected void AlignWithGround(float smoothness)
		{
			Vector3 normal = HoverSensor.GetCollisionNormal();
			Transform transform = AlignYAxis(normal);	
			GlobalTransform = GlobalTransform.InterpolateWith(transform, smoothness);			
		}

		protected void ModulateHeight(float strength)
		{
			Vector3 ground = GlobalTransform.XformInv(HoverSensor.GetCollisionPoint());
			float displacement = (HoverSensor.CastTo - ground).Length();
			Vector3 force = Vector3.Up * displacement * Mass * strength;
			Vector3 globalForce = GlobalTransform.basis.Xform(force);
			AddCentralForce(globalForce);
		}

		protected void Hover()
		{
			if (HoverSensor.IsColliding())
			{				
				//AlignWithGround(0.1f);
				ModulateHeight(30f);
			}
		}
		*/

		public override void _IntegrateForces(PhysicsDirectBodyState state)
		{
			base._IntegrateForces(state);
		}
	}
}
