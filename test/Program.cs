using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class Foo
    {
        public virtual void Hello()
        {
            Console.WriteLine("HelloFoo");
        }
    }
    class Bar : Foo
    {
        public int a = 1;
        public override void Hello()
        {
            Console.WriteLine("HelloBar");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var c = new Bar();
            c.a += 12354;
            var a = 1;

            switch(a)
            {
                case 1:
                    Console.Write(1234);
                    break;
                case 2:
                    break;
            }
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
