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
    public sealed class VParameterInfo : ParameterInfo
    {
        public override Type ParameterType => _ParameterType;
        private Type _ParameterType;

        private VM vm;
        private ParameterDefinition parameter;

        public VParameterInfo(VM vm, ParameterDefinition parameter)
        {
            this.vm = vm;
            this.parameter = parameter;

            if (parameter.ParameterType.IsGenericParameter)
                _ParameterType = null;
            else
                _ParameterType = vm.typeResolver.Resolve(parameter.ParameterType);
        }
    }
}
