using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MartianDelivery
{
    public enum ThrustAction
    {
        All,
        Ascend,
        Forward,
        Decend
    }

    public abstract class Thruster : Spatial
    {
        protected Ship parent;
        protected int maxThrust;
        public List<ThrustAction> Actions;

        protected Vector3 thrust;
        protected bool activated = false;

        public abstract void Activate(float power);
        public abstract void Deactivate();
    }

    public class CentralThruster : Thruster
    {
        private Vector3 direction;

        public CentralThruster(Ship parent, Vector3 direction, int thrust, List<ThrustAction> actions)
        {
            this.parent = parent;
            this.direction = direction;
            maxThrust = thrust;
            Actions = actions;
        }

        public override void Activate(float power)
        {/*
            if (!activated)
            {
            */
                Vector3 local = power * maxThrust * -1 * direction;
                thrust = parent.GlobalTransform.basis.Xform(local);
                parent.ApplyCentralImpulse(thrust);
                activated = true;
            //}      
        }

        public override void Deactivate()
        {
            if (activated)
            {
                parent.AddCentralForce(-thrust);
                activated = false;
            }
            
        }
    }
}
