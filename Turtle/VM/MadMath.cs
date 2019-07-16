using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Turtle
{
    internal class MadMath
    {
        public static object Add(object a, object b)
        {
            if (a.GetType().IsPrimitive)
            {
                (a, b) = Promote(a, b);

                if (a is double) return (double)a + (double)b;
                if (a is float) return (float)a + (float)b;
                if (a is Int64) return (Int64)a + (Int64)b;
                if (a is Int32) return (Int32)a + (Int32)b;
            }
            else if (a is string)
                return (string)a + b.ToString();

            var method = GetAddMethod(a);
            return method?.Invoke(null, new object[] { a, b }) 
                ?? throw new InvalidOperationException();
        }
        public static object Sub(object a, object b)
        {
            if (a.GetType().IsPrimitive)
            {
                (a, b) = Promote(a, b);

                if (a is double) return (double)a - (double)b;
                if (a is float) return (float)a - (float)b;
                if (a is Int64) return (Int64)a - (Int64)b;
                if (a is Int32) return (Int32)a - (Int32)b;
            }

            var method = GetSubMethod(a);
            return method?.Invoke(null, new object[] { a, b })
                ?? throw new InvalidOperationException();
        }

        public static object G(object a, object b)
        {
            if (a.GetType().IsPrimitive)
            {
                if (a is double) return (double)a > (double)b;
                if (a is float) return (float)a > (float)b;
                if (a is Int64) return (Int64)a > (Int64)b;
                if (a is Int32) return (Int32)a > (Int32)b;
            }

            var method = GetGreaterMethod(a);
            return method?.Invoke(null, new object[] { a, b })
                ?? throw new InvalidOperationException();
        }
        public static object GE(object a, object b)
        {
            if (a.GetType().IsPrimitive)
            {
                if (a is double) return (double)a >= (double)b;
                if (a is float) return (float)a >= (float)b;
                if (a is Int64) return (Int64)a >= (Int64)b;
                if (a is Int32) return (Int32)a >= (Int32)b;
            }

            var method = GetGreaterEqualMethod(a);
            return method?.Invoke(null, new object[] { a, b })
                ?? throw new InvalidOperationException();
        }
        public static object L(object a, object b)
        {
            if (a.GetType().IsPrimitive)
            {
                if (a is double) return (double)a < (double)b;
                if (a is float) return (float)a < (float)b;
                if (a is Int64) return (Int64)a < (Int64)b;
                if (a is Int32) return (Int32)a < (Int32)b;
            }

            var method = GetLessMethod(a);
            return method?.Invoke(null, new object[] { a, b })
                ?? throw new InvalidOperationException();
        }
        public static object LE(object a, object b)
        {
            if (a.GetType().IsPrimitive)
            {
                if (a is double) return (double)a <= (double)b;
                if (a is float) return (float)a <= (float)b;
                if (a is Int64) return (Int64)a <= (Int64)b;
                if (a is Int32) return (Int32)a <= (Int32)b;
            }

            var method = GetLessEqualMethod(a);
            return method?.Invoke(null, new object[] { a, b })
                ?? throw new InvalidOperationException();
        }

        private static (object, object) Promote(object a, object b)
        {
            if (a.GetType() == b.GetType())
                return (a, b);

            if (a is double || b is double)
                return ((double)a, (double)b);
            if (a is float || b is float)
                return ((float)a, (float)b);
            if (a is Int64 || b is Int64)
                return ((Int64)a, (Int64)b);
            if (a is Int32 || b is Int32)
                return ((Int32)a, (Int32)b);

            return (a, b);
        }

        private static MethodInfo GetAddMethod(object left)
            => left.GetType().GetMethod("op_Addition");
        private static MethodInfo GetSubMethod(object left)
            => left.GetType().GetMethod("op_Subtraction");

        private static MethodInfo GetGreaterMethod(object left)
            => left.GetType().GetMethod("op_GreaterThan");
        private static MethodInfo GetLessMethod(object left)
            => left.GetType().GetMethod("op_LessThan");
        private static MethodInfo GetGreaterEqualMethod(object left)
            => left.GetType().GetMethod("op_GreaterThanOrEqual");
        private static MethodInfo GetLessEqualMethod(object left)
            => left.GetType().GetMethod("op_LessThanOrEqual");
    }
}
