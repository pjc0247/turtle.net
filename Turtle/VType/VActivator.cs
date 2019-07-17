using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turtle
{
    public class VActivator
    {
        public static object CreateInstance(VM vm, Type type, object[] args)
        {
            if (type is VType vtype)
            {
                var obj = vm.Allocobj(vtype);
                vm.Dup();
                var ctor = vtype.GetConstructor(
                    args.Select(x => x.GetType()).ToArray());
                ctor.Invoke(args);

                return obj;
            }
            else
                return Activator.CreateInstance(type, args);
        }
    }
}
