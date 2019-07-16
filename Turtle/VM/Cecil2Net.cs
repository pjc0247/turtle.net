using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Turtle
{
    internal static class Cecil2Net
    {
        public static BindingFlags GetBindingFlags(this MethodDefinition method)
        {
            BindingFlags flags = BindingFlags.Default;

            if (method.IsStatic) flags |= BindingFlags.Static;
            else flags |= BindingFlags.Instance;

            if (method.IsPublic) flags |= BindingFlags.Public;
            else flags |= BindingFlags.NonPublic;

            return flags;
        }
        public static BindingFlags GetBindingFlags(this FieldDefinition method)
        {
            BindingFlags flags = BindingFlags.Default;

            if (method.IsStatic) flags |= BindingFlags.Static;
            else flags |= BindingFlags.Instance;

            if (method.IsPublic) flags |= BindingFlags.Public;
            else flags |= BindingFlags.NonPublic;

            return flags;
        }

        public static Type[] ToTypes(this TypeReference[] refs, TypeResolver typeResolver)
        {
            var types = new Type[refs.Length];
            for (int i = 0; i < refs.Length; i++)
                types[i] = typeResolver.Resolve(refs[i]);
            return types;
        }
    }
}
