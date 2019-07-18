using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mono.Cecil;

namespace Turtle
{
    public class VActivator
    {
        public static object CreateInstance(VM vm, MethodReference ctor, Type type, object[] args)
        {
            if (type is VType vtype)
            {
                var obj = vm.Allocobj(vtype);

                if (ctor.DeclaringType is GenericInstanceType g)
                    obj.genericArgs = g.GenericArguments.ToArray();

                var ctorImpl = vtype.GetConstructor(
                    args.Select(x => x.GetType()).ToArray());
                ctorImpl.Invoke(obj, args);

                return obj;
            }
            else
                return Activator.CreateInstance(type, args);
        }
    }
}
