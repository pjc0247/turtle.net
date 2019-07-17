using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unittest
{
    class Operation
    {
        public static object Sum()
        {
            var a = 10;
            var b = 20;
            return a + b;
        }
        public static object Sub()
        {
            var a = 10;
            var b = 20;
            return a - b;
        }
        public static object Mul()
        {
            var a = 10;
            var b = 20;
            return a * b;
        }
        public static object Div()
        {
            var a = 10;
            var b = 20;
            return a / b;
        }
    }
}
