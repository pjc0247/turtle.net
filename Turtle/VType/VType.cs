using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Turtle
{
    public class VType : Type
    {
        public override Guid GUID => throw new NotImplementedException();

        public override Module Module => throw new NotImplementedException();

        public override Assembly Assembly => throw new NotImplementedException();

        public override string FullName => _fullName;
        private string _fullName;
        public override string Name => _name;
        private string _name;

        public override string Namespace => throw new NotImplementedException();

        public override string AssemblyQualifiedName => throw new NotImplementedException();

        public override Type BaseType => typeof(VType);

        public override Type UnderlyingSystemType => this;

        private VM vm;
        private TypeDefinition type;

        private VConstructorInfo cctor;
        private List<VConstructorInfo> ctors = new List<VConstructorInfo>();
        private List<VMethodInfo> methods = new List<VMethodInfo>();
        private List<VFieldInfo> fields = new List<VFieldInfo>();
        private List<VPropertyInfo> properties = new List<VPropertyInfo>();

        public VType(VM vm, TypeDefinition type)
        {
            this.vm = vm;
            this.type = type;

            _fullName = type.FullName;
            _name = type.Name;
        }

        public VConstructorInfo AddCtor(MethodDefinition method)
        {
            var vMethod = new VConstructorInfo(vm, method);
            ctors.Add(vMethod);

            if (method.IsStatic)
                cctor = vMethod;

            return vMethod;
        }
        public VMethodInfo AddMethod(MethodDefinition method)
        {
            var vMethod = new VMethodInfo(vm, method);
            methods.Add(vMethod);
            return vMethod;
        }
        public VFieldInfo AddField(FieldDefinition field)
        {
            var vField = new VFieldInfo(vm, field, fields.Count);
            fields.Add(vField);
            return vField;
        }
        public VPropertyInfo AddProperty(PropertyDefinition property)
        {
            var vProperty = new VPropertyInfo(vm, property);
            properties.Add(vProperty);
            return vProperty;
        }

        public void Initialize()
        {
            Console.WriteLine(type.FullName);
            foreach (var method in type.Methods)
            {
                Console.WriteLine("  " + method.FullName);
                if (method.IsConstructor)
                    AddCtor(method);
                else
                    AddMethod(method);
            }
            foreach (var field in type.Fields)
                AddField(field);
            foreach (var property in type.Properties)
                AddProperty(property);

            foreach (var field in fields)
            {
                if (field.IsStatic) {
                    object v = null;
                    if (field.FieldType.IsValueType)
                        v = Activator.CreateInstance(field.FieldType);
                    field.SetValue(null, v);
                }
            }

            if (cctor != null)
                cctor.Invoke(new object[] { });
        }

        public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
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

        public override Type GetElementType()
        {
            throw new NotImplementedException();
        }

        public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        public override EventInfo[] GetEvents(BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        public override FieldInfo GetField(string name, BindingFlags bindingAttr)
        {
            return fields.Where(x => x.Name == name).FirstOrDefault();
        }
        public override FieldInfo[] GetFields(BindingFlags bindingAttr)
        {
            return fields.ToArray();
        }

        public override Type GetInterface(string name, bool ignoreCase)
        {
            throw new NotImplementedException();
        }

        public override Type[] GetInterfaces()
        {
            throw new NotImplementedException();
        }

        public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
        {
            return methods.ToArray();
        }

        public override Type GetNestedType(string name, BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        public override Type[] GetNestedTypes(BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
        {
            throw new NotImplementedException();
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

        protected override System.Reflection.TypeAttributes GetAttributeFlagsImpl()
        {
            throw new NotImplementedException();
        }

        protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
        {
            return ctors.Where(x => x.IsStatic == false).FirstOrDefault();
        }

        protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
        {
            return
                methods.Where(x => x.Name == name)
                .FirstOrDefault();
        }

        protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
        {
            return properties
                .Where(x => x.Name == name)
                .FirstOrDefault();
        }

        protected override bool HasElementTypeImpl() => false;
        protected override bool IsArrayImpl() => false;
        protected override bool IsByRefImpl() => false;
        protected override bool IsCOMObjectImpl() => false;
        protected override bool IsPointerImpl() => false;
        protected override bool IsPrimitiveImpl() => false;
    }
}
