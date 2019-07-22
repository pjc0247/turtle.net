using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    struct BOO
    {
        public int a;
    }

    class Foo
    {
        public virtual void Hello()
        {
            Console.WriteLine("HelloFoo");
        }
    }
    class Bar : Foo
    {
        public int a { get; set; }
        /*
        public override void Hello()
        {
            Console.WriteLine("HelloBar");
        }
        */
    }

    class TestAttribute : Attribute
    {
        public TestAttribute()
        {
            Console.Write("TESTSTSTST");
        }
    }

    class G<T>
    {
        public G()
        {
            Console.WriteLine(typeof(T));
        }

        public static void Hello()
        {
            Console.WriteLine(typeof(T));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var a = new Func<int>(() =>
            {
                return 10;
            });
            Console.WriteLine(a());

            return;
        }

        static void Foo()
        {
            object o = new Program();

            Console.WriteLine(o);
        }
    }
}
