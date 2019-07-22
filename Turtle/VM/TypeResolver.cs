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
        private AssemblyResolver assemblyResolver;
        private string basepath;

        public TypeResolver(string basepath)
        {
            this.basepath = basepath;

            assemblyResolver = new AssemblyResolver(basepath);
        }

        public void AddType(VType type)
        {
            vtypes[type.FullName] = type;
        }
        public Type Resolve(TypeReference typeRef)
        {
            var type = _Resolve(typeRef);
            if (typeRef.IsByReference)
                return type.MakeByRefType();
            return type;
        }
        public Type _Resolve(TypeReference typeRef)
        {
            if (typeRef is GenericInstanceType g &&
                g.HasGenericArguments)
            {
                var typeDef = Resolve(g.ElementType);
                var c = g.GenericArguments.Select(x => Resolve(x)).ToArray();
                return typeDef.MakeGenericType(c);
            }

            if (typeRef.IsArray)
            {
                return Resolve(typeRef.GetElementType())
                    .MakeArrayType();
            }

            VType vtype = null;
            if (vtypes.TryGetValue(typeRef.FullName, out vtype))
                return vtype;

            var asm = assemblyResolver.GetAssembly(typeRef.Module.Assembly);
            var type = asm.GetType(typeRef.FullName);

            if (type == null)
                type = typeof(int).Assembly.GetType(typeRef.FullName);
            if (type == null)
                type = typeof(System.Linq.Enumerable).Assembly.GetType(typeRef.FullName);

            if (type == null)
                throw new InvalidOperationException("Type not resolved");

            return type;
        }
    }
}
