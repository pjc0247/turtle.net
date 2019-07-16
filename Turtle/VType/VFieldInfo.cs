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
    public class VFieldInfo : FieldInfo
    {
        public override RuntimeFieldHandle FieldHandle => throw new NotImplementedException();

        public override Type FieldType => _FieldType;
        private Type _FieldType;

        public override Type DeclaringType => _DeclaringType;
        private Type _DeclaringType;

        public override System.Reflection.FieldAttributes Attributes => throw new NotImplementedException();

        public override string Name => _Name;
        private string _Name;

        public override Type ReflectedType => throw new NotImplementedException();

        private VM vm;
        private FieldDefinition field;
        private int ptr;

        public VFieldInfo (VM vm, FieldDefinition field, int ptr)
        {
            this.vm = vm;
            this.field = field;
            this.ptr = ptr;

            _Name = field.Name;
            _DeclaringType = vm.typeResolver.Resolve(field.DeclaringType);
            _FieldType = vm.typeResolver.Resolve(field.FieldType);
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            throw new NotImplementedException();
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

        public override object GetValue(object obj)
        {
            if (obj is VObject vobj)
                return vobj.fields[ptr];
            else
                throw new ArgumentException(nameof(obj));
        }
        public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture)
        {
            if (obj is VObject vobj)
                vobj.fields[ptr] = value;
            else
                throw new ArgumentException(nameof(obj));
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }
    }
}
