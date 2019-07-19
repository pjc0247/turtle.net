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
    public class VPropertyInfo : PropertyInfo
    {
        public override string Name => _Name;
        private string _Name;

        public override System.Reflection.PropertyAttributes Attributes => _Attributes;
        private System.Reflection.PropertyAttributes _Attributes;

        public override bool CanRead => _CanRead;
        private bool _CanRead;
        public override bool CanWrite => _CanWrite;
        private bool _CanWrite;

        public override Type PropertyType => _PropertyType;
        private Type _PropertyType;
        public override Type DeclaringType => _DeclaringType;
        private VType _DeclaringType;

        public override Type ReflectedType => throw new NotImplementedException();

        private VM vm;
        private PropertyDefinition property;
        private MethodInfo getMethod, setMethod;

        public VPropertyInfo(VM vm, PropertyDefinition property, VType declaringType)
        {
            this.vm = vm;
            this.property = property;

            _Name = property.Name;
            _DeclaringType = declaringType;
            _CanRead = property.GetMethod != null;
            _CanWrite = property.SetMethod != null;

            if (_CanRead)
                getMethod = declaringType.GetMethod(property.GetMethod.Name);
            if (_CanWrite)
                setMethod = declaringType.GetMethod(property.SetMethod.Name);

            BuildAttributes();
        }

        private void BuildAttributes()
        {
        }

        public override MethodInfo[] GetAccessors(bool nonPublic)
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

        public override MethodInfo GetGetMethod(bool nonPublic)
        {
            if (getMethod == null) return null;
            if (getMethod.IsPublic == false && nonPublic == false) return null;
            return getMethod;
        }
        public override MethodInfo GetSetMethod(bool nonPublic)
        {
            if (setMethod == null) return null;
            if (setMethod.IsPublic == false && nonPublic == false) return null;
            return setMethod;
        }

        public override ParameterInfo[] GetIndexParameters()
        {
            throw new NotImplementedException();
        }

        public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

        public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
