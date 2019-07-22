using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;

using Mono.Cecil;

namespace Turtle
{
    class InitializeArrayShim : CallInterceptor
    {
        /*
        public static object Call(VM vm, object[] args, MethodReference method)
        {
            vm.assembly.GetType
            RuntimeHelpers.InitializeArray()

            return null;
        }
        */
    }
}
