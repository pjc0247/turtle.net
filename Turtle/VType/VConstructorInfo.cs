﻿using System;
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
    public class VConstructorInfo : ConstructorInfo
    {
        public override RuntimeMethodHandle MethodHandle => throw new NotImplementedException();

        public override System.Reflection.MethodAttributes Attributes => _Attributes;
        private System.Reflection.MethodAttributes _Attributes;

        public override string Name => throw new NotImplementedException();

        public override Type DeclaringType => throw new NotImplementedException();

        public override Type ReflectedType => throw new NotImplementedException();

        private VM vm;
        private MethodDefinition method;
        private VParameterInfo[] parameters;

        public VConstructorInfo(VM vm, MethodDefinition method)
        {
            this.vm = vm;
            this.method = method;

            parameters = new VParameterInfo[method.Parameters.Count];
            for (int i = 0; i < method.Parameters.Count; i++)
                parameters[i] = new VParameterInfo(vm, method.Parameters[i]);

            BuildAttributes();
        }
        private void BuildAttributes()
        {
            if (method.IsStatic) _Attributes |= System.Reflection.MethodAttributes.Static;

            if (method.IsPublic) _Attributes |= System.Reflection.MethodAttributes.Public;
            else _Attributes |= System.Reflection.MethodAttributes.Private;
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

        public override ParameterInfo[] GetParameters()
            => parameters.ToArray();

        public override object Invoke(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
        {
            return vm.Run(method, parameters);
        }

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
