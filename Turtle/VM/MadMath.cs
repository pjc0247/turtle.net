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

        public static object Neg(object a)
        {
            if (a.GetType().IsPrimitive)
            {
                if (a is double) return -(double)a;
                if (a is float) return -(float)a;
                if (a is Int64) return -(Int64)a;
                if (a is Int32) return -(Int32)a;
            }

            throw new InvalidOperationException();
        }

        public static bool G(object a, object b)
        {
            if (a.GetType().IsPrimitive)
            {
                if (a is double) return (double)a > (double)b;
                if (a is float) return (float)a > (float)b;
                if (a is Int64) return (Int64)a > (Int64)b;
                if (a is Int32) return (Int32)a > (Int32)b;
            }

            var method = GetGreaterMethod(a);
            if (method != null)
                return (bool)method.Invoke(null, new object[] { a, b });
            throw new InvalidOperationException();
        }
        public static bool GE(object a, object b)
        {
            if (a.GetType().IsPrimitive)
            {
                if (a is double) return (double)a >= (double)b;
                if (a is float) return (float)a >= (float)b;
                if (a is Int64) return (Int64)a >= (Int64)b;
                if (a is Int32) return (Int32)a >= (Int32)b;
            }

            var method = GetGreaterEqualMethod(a);
            if (method != null)
                return (bool)method.Invoke(null, new object[] { a, b });
            throw new InvalidOperationException();
        }
        public static bool L(object a, object b)
        {
            if (a.GetType().IsPrimitive)
            {
                if (a is double) return (double)a < (double)b;
                if (a is float) return (float)a < (float)b;
                if (a is Int64) return (Int64)a < (Int64)b;
                if (a is Int32) return (Int32)a < (Int32)b;
            }

            var method = GetLessMethod(a);
            if (method != null)
                return (bool)method.Invoke(null, new object[] { a, b });
            throw new InvalidOperationException();
        }
        public static bool LE(object a, object b)
        {
            if (a.GetType().IsPrimitive)
            {
                if (a is double) return (double)a <= (double)b;
                if (a is float) return (float)a <= (float)b;
                if (a is Int64) return (Int64)a <= (Int64)b;
                if (a is Int32) return (Int32)a <= (Int32)b;
            }

            var method = GetLessEqualMethod(a);
            if (method != null)
                return (bool)method.Invoke(null, new object[] { a, b });
            throw new InvalidOperationException();
        }
        public static bool Eq(object a, object b)
        {
            if (a.GetType().IsValueType)
                return a.Equals(b);
            if (a is string)
                return a.Equals(b);

            return a == b;
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

        private static MethodInfo GetEqMethod(object left)
            => left.GetType().GetMethod("op_Equality");
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
