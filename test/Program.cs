using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args[0].Length >= args[1].Length)

            Foo();
        }

        static void Foo()
        {
            object o = new Program();

            Console.WriteLine(o);
        }

        public Program()
        {
            Console.WriteLine(1 >= 4);
        }
    }
}
