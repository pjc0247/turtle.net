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
    internal class TypeResolver
    {
        private Dictionary<string, VType> vtypes = new Dictionary<string, VType>();

        public void AddType(VType type)
        {
            vtypes[type.FullName] = type;
        }
        public Type Resolve(TypeReference typeRef)
        {
            if (typeRef.FullName == typeof(int).FullName)
                return typeof(int);
            if (typeRef.FullName == typeof(object).FullName)
                return typeof(object);

            VType vtype = null;
            if (vtypes.TryGetValue(typeRef.FullName, out vtype))
                return vtype;

            var asm = AssemblyResolver.GetAssembly(typeRef.Module.Assembly);
            return asm.GetType(typeRef.FullName);
        }
    }
}
