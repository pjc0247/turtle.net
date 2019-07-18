using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mono.Cecil;

namespace Turtle
{
    public class TurtleRunner
    {
        public static void Run(string executablePath, string[] args)
        {
            var test = ModuleDefinition.ReadModule(executablePath);

            var vm = new VM(".");
            var main = test.Assembly.EntryPoint.Body.Instructions;

            vm.Build(test);
            vm.Run(test.Assembly.EntryPoint, new object[] { args });
        }
    }
}
