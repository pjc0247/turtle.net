using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turtle.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            TurtleRunner.Run(args[0], args.Skip(1).ToArray());
        }
    }
}
