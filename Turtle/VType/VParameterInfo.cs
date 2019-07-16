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
        private VM vm;
        private ParameterDefinition parameter;

        public override Type ParameterType => _ParameterType;
        private Type _ParameterType;

        public VParameterInfo(VM vm, ParameterDefinition parameter)
        {
            this.vm = vm;
            this.parameter = parameter;

            _ParameterType = vm.typeResolver.Resolve(parameter.ParameterType);
        }
    }
}
