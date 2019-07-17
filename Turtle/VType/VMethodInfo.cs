using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Globalization;

using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Turtle
{
    public class VMethodInfo : MethodInfo
    {
        public override System.Reflection.ICustomAttributeProvider ReturnTypeCustomAttributes => throw new NotImplementedException();

        public override RuntimeMethodHandle MethodHandle => throw new NotImplementedException();

        public override System.Reflection.MethodAttributes Attributes => System.Reflection.MethodAttributes.Public;

        public override string Name => _Name;
        private string _Name;

        public override Type DeclaringType => throw new NotImplementedException();

        public override Type ReflectedType => throw new NotImplementedException();

        private VM vm;
        private MethodDefinition method;
        private VParameterInfo[] parameters;

        public VMethodInfo(VM vm, MethodDefinition method)
        {
            this.vm = vm;
            this.method = method;

            _Name = method.Name;

            parameters = new VParameterInfo[method.Parameters.Count];
            for (int i = 0; i < method.Parameters.Count; i++)
                parameters[i] = new VParameterInfo(vm, method.Parameters[i]);
        }

        public override MethodInfo GetBaseDefinition()
        {
            throw new NotImplementedException();
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            throw new NotImplementedException();
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

        public override System.Reflection.MethodImplAttributes GetMethodImplementationFlags()
        {
            throw new NotImplementedException();
        }

        public override ParameterInfo[] GetParameters() => parameters;

        public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
        {
            return vm.Run(method, obj, parameters);
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }
    }
}
