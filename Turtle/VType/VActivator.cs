using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                var obj = vm.AllocobjStack(vtype);

                if (ctor.DeclaringType is GenericInstanceType g)
                    obj.genericArgs = g.GenericArguments.ToArray();

                var ctorImpl = vtype.GetConstructor(
                    args.Select(x => x?.GetType()).ToArray());
                ctorImpl.Invoke(obj, args);

                return obj;
            }
            else {
                for (int i = 0; i < ctor.Parameters.Count; i++){
                    var paramTypeDef = ctor.Parameters[i].ParameterType.Resolve();
                    if (paramTypeDef.IsEnum)
                    {
                        var paramType = vm.typeResolver.Resolve(paramTypeDef);
                        args[i] = Enum.ToObject(paramType, args[i]);
                    }
                }
                return Activator.CreateInstance(type, args);
            }
        }
        public static object CreateInstance(VM vm, Type type, object[] args)
        {
            if (type is VType vtype)
            {
                var obj = vm.AllocobjStack(vtype);
                var ctor = type.GetConstructor(
                    args.Select(x => x?.GetType()).ToArray());

                ctor?.Invoke(obj, args);

                return obj;
            }
            else
                return Activator.CreateInstance(type, args);
        }
    }
}
