using Godot;
using System;
using System.Collections.Generic;

namespace MartianDelivery
{
	public class Ship : RigidBody
	{   
		// Child Nodes
		protected MeshInstance Mesh
		{
			get { return (MeshInstance)GetNode("Mesh"); }
		}
		protected CollisionShape CollisionShape
		{
			get { return(CollisionShape)GetNode("CollisionShape"); }
		}

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

		public override void _Process(float delta)
		{
			base._Process(delta);            
		}
	}
}
