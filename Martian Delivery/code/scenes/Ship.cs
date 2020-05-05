using Godot;
using System;

namespace MartianDelivery
{
    public class Ship : RigidBody
    {
        [Export] public int Thrust;       

        // Child Nodes
        protected MeshInstance Mesh
        {
            get { return (MeshInstance)GetNode("Mesh"); }
        }
        protected CollisionShape CollisionShape
        {
            get { return(CollisionShape)GetNode("CollisionShape"); }
        }

        protected (Ve)

        public Ship() { }
        public Ship(int thrust)
        {
            Thrust = thrust;
        }

        public override void _Ready()
        {
            base._Ready();
        }

        public override void _Process(float delta)
        {
            base._Process(delta);
        }

        protected void Hover()
        {

        }

        protected void 
    }
}