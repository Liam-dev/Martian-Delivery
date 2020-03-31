using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance
{
    class Vehicle
    {
        protected string make;
        protected string model;

        public void Move()
        {
            Console.WriteLine($"{make} is moving");
        }
    }

    class Plane : Vehicle
    {
        public Plane(string make, string model)
        {
            this.make = make;
            this.model = model;
        }

        public void Fly()
        {
            Console.WriteLine($"{make} {model} is flying");
        }
    }
}
