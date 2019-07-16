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
        public override void Hello()
        {
            Console.WriteLine("HelloBar");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var a = new Bar();
            var b = (Foo)a;

            Console.WriteLine(a);
            a.Hello();
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
