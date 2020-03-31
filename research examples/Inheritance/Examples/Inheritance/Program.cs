using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance
{
    class Program
    {
        static void Main(string[] args)
        {
            Plane boeing = new Plane("Boeing", "737");
            boeing.Move();
            boeing.Fly();
        }
    }

}
