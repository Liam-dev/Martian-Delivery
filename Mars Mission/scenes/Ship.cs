using Godot;
using System;

namespace MarsMission
{
    public class Ship : RigidBody, IDamageObject
    {
        // Signals      
        [Signal] delegate void SDestroyed();

        // Exported members
        [Export] public int MaxHealth;
        [Export] public int Thrust;       

        // Child Nodes
        protected Spatial Thrusters
        {
            get { return (Spatial)GetNode("Thrusters"); }
        }
        protected MeshInstance Mesh
        {
            get { return (MeshInstance)GetNode("Mesh"); }
        }
        protected CollisionShape CollisionShape
        {
            get { return(CollisionShape)GetNode("CollisionShape"); }
        }

        protected int health;

        public bool Destroyed
        {
            get { return (health <= 0); }
        }

        public Ship() { }
        public Ship(int heath, int thrust)
        {
            MaxHealth = heath;
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
    }
}