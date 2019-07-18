using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Turtle
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = ModuleDefinition.ReadModule("..\\..\\..\\unittest\\bin\\Debug\\unittest.exe");
            var module = ModuleDefinition.ReadModule(
                typeof(Console).Assembly.Location
                );

            var vm = new VM("..\\..\\..\\unittest\\bin\\Debug\\");
            var main = test.Assembly.EntryPoint.Body.Instructions;

            vm.Build(test);
            vm.Run(test.Assembly.EntryPoint, new object[] { new string[] { } });
            /*
            vm.Run(new Instruction[]
            {
                Instruction.Create(OpCodes.Ldc_I4, 123),
                Instruction.Create(OpCodes.Ldc_I4, 456),
                Instruction.Create(OpCodes.Call, module.GetType("System.Console")
                        .Methods
                        .Where(x => x.Name == "WriteLine")
                        .Where(x => x.Parameters.Count == 1 && x.Parameters[0].ParameterType.Name == typeof(int).Name)
                        .FirstOrDefault())
            });
            */
        }
    }
}
