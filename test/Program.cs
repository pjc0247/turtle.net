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
        [Test]
        public int bb { get; set; } = 1;

        public int a = 1;
        public override void Hello()
        {
            Console.WriteLine("HelloBar");
        }
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
            Foo a = null;
            a.Hello();

            return;
        }

        static void Foo()
        {
            object o = new Program();

            Console.WriteLine(o);
        }
    }
}
