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

    class Program
    {
        int A() => 1;

        static void Main(string[] args)
        {
            var cc = new Program();
            Console.WriteLine(cc.A());
            Console.WriteLine(cc.A());
            Console.WriteLine(cc.A());
            Console.WriteLine(cc.A());
            Console.WriteLine(cc.A());
            Console.WriteLine(cc.A());

            return;

            var c = new Bar();

            Console.Write(c.bb);
            Console.Write(c.bb);
            Console.Write(c.bb);
            Console.Write(c.bb);
            Console.Write(c.bb);
            Console.Write(c.bb);
            Console.Write(c.bb);

            c.bb = 12314;
            Console.Write(c.bb);

            
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
    }
}
