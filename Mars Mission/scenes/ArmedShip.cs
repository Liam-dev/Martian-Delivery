using Godot;
using System;

namespace MarsMission
{
    public class ArmedShip : Ship, IShoot
    {   
        // Signals
        [Signal] delegate void SShoot();

        // Exported members
        [Export] public float CannonCooldown;
        [Export] public PackedScene Laser;

        // Child Nodes
        protected Timer CannonTimer
        {
            get { return (Timer)GetNode("CannonTimer"); }
        }

        protected bool canShoot;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            base._Ready();
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(float delta)
        {
            base._Process(delta);
        }
    }
}