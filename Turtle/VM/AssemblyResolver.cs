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
    internal class AssemblyResolver
    {
        public static Assembly GetAssembly(AssemblyDefinition assemblyDef)
        {
            var asm = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.FullName == assemblyDef.FullName)
                .FirstOrDefault();

            if (asm != null)
                return asm;

            return Assembly.Load(assemblyDef.FullName);
        }
    }
}
