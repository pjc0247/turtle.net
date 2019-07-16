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

        public override Type BaseType => throw new NotImplementedException();

        public override Type UnderlyingSystemType => this;

        private VM vm;
        private TypeDefinition type;

        private List<VConstructorInfo> ctors = new List<VConstructorInfo>();
        private List<VMethodInfo> methods = new List<VMethodInfo>();
        private List<VFieldInfo> fields = new List<VFieldInfo>();

        public VType(VM vm, TypeDefinition type)
        {
            this.vm = vm;
            this.type = type;

            _fullName = type.FullName;
            _name = type.Name;

            foreach (var method in type.Methods)
            {
                if (method.IsConstructor)
                    AddCtor(method);
                else
                    AddMethod(method);
            }
            foreach (var field in type.Fields)
                AddField(field);
        }

        public VConstructorInfo AddCtor(MethodDefinition method)
        {
            var vMethod = new VConstructorInfo(vm, method);
            ctors.Add(vMethod);
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
            throw new NotImplementedException();
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
            return ctors.FirstOrDefault();
        }

        protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
        {
            return
                methods.Where(x => x.Name == name)
                .FirstOrDefault();
        }

        protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
        {
            throw new NotImplementedException();
        }

        protected override bool HasElementTypeImpl()
        {
            throw new NotImplementedException();
        }

        protected override bool IsArrayImpl()
        {
            throw new NotImplementedException();
        }

        protected override bool IsByRefImpl()
        {
            throw new NotImplementedException();
        }

        protected override bool IsCOMObjectImpl()
        {
            throw new NotImplementedException();
        }

        protected override bool IsPointerImpl()
        {
            throw new NotImplementedException();
        }

        protected override bool IsPrimitiveImpl()
        {
            throw new NotImplementedException();
        }
    }
}
