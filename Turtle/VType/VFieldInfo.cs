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

        public override System.Reflection.FieldAttributes Attributes => _Attributes;
        private System.Reflection.FieldAttributes _Attributes;

        public override string Name => _Name;
        private string _Name;

        public object InitialValue => field.InitialValue;

        public override Type ReflectedType => throw new NotImplementedException();

        private VM vm;
        private FieldDefinition field;
        private long ptr;

        public VFieldInfo (VM vm, FieldDefinition field, long ptr)
        {
            this.vm = vm;
            this.field = field;
            this.ptr = ptr;

            _Name = field.Name;
            _DeclaringType = vm.typeResolver.Resolve(field.DeclaringType);
            _FieldType = vm.typeResolver.Resolve(field.FieldType);

            BuildAttributes();

            if (IsStatic)
                this.ptr = vm.storage.GetNextKey();
        }

        public void InitializeCustomAttributes()
        {
            foreach (var attr in field.CustomAttributes)
            {
                var args = attr.ConstructorArguments.Select(x => x.Value).ToArray();
                vm.Newobj(attr.Constructor);
                vm.Run(attr.Constructor.Resolve(), args);
            }
        }

        private void BuildAttributes()
        {
            if (field.IsStatic) _Attributes |= System.Reflection.FieldAttributes.Static;

            if (field.IsPublic) _Attributes |= System.Reflection.FieldAttributes.Public;
            else _Attributes |= System.Reflection.FieldAttributes.Private;
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
            else if (IsStatic)
                return vm.storage.Get(ptr);
            else
                throw new ArgumentException(nameof(obj));
        }
        public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture)
        {
            if (obj is VObject vobj)
                vobj.fields[ptr] = value;
            else if (IsStatic)
                vm.storage.Set(ptr, value);
            else
                throw new ArgumentException(nameof(obj));
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }
    }
}
