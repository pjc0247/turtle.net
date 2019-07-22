using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Mono.Cecil;

namespace Turtle
{
    class CapturedDelegateFactory
    {
        public static (object, IntPtr) Create(Type[] types, VM vm, object _this, MethodDefinition method)
        {
            Type dType = null;
            IntPtr ptr;

            if (method.ReturnType.FullName == typeof(void).FullName)
            {
                dType = typeof(CapturedDelegateAction<>).MakeGenericType(types);
                ptr = GetMethodPtrAction(types);
            }
            else
            {
                dType = typeof(CapturedDelegateFunc<>).MakeGenericType(types);
                ptr = GetMethodPtrFunc(types);
            }

            var del = Activator.CreateInstance(dType, new object[] { vm, _this, method });
            return (del, ptr);
        }
        public static IntPtr GetMethodPtrFunc(Type[] types)
        {
            return typeof(CapturedDelegateFunc<>)
                .MakeGenericType(types)
                .GetMethod(nameof(CapturedDelegateFunc<int>.Invoke))
                .MethodHandle.GetFunctionPointer();
        }
        public static IntPtr GetMethodPtrAction(Type[] types)
        {
            return typeof(CapturedDelegateAction<>)
                .MakeGenericType(types)
                .GetMethod(nameof(CapturedDelegateAction<int>.Invoke))
                .MethodHandle.GetFunctionPointer();
        }
    }

    class CapturedDelegate
    {
        protected VM vm { get; private set; }
        protected object _this { get; private set; }
        protected MethodDefinition method { get; private set; }

        public CapturedDelegate(VM vm, object _this, MethodDefinition method)
        {
            this.vm = vm;
            this._this = _this;
            this.method = method;
        }
    }

    class CapturedDelegateFunc<T1> : CapturedDelegate
    {
        public CapturedDelegateFunc(VM vm, object _this, MethodDefinition method) :
            base(vm, _this, method)
        { }
        public object Invoke() => vm.Run(method, _this, new object[] { });
    }
    class CapturedDelegateAction<T1> : CapturedDelegate
    {
        public CapturedDelegateAction(VM vm, object _this, MethodDefinition method) :
            base(vm, _this, method) { }
        public object Invoke(T1 a) => vm.Run(method, _this, new object[] { a });
    }
    class CapturedDelegate<T1, T2> : CapturedDelegate
    {
        public CapturedDelegate(VM vm, object _this, MethodDefinition method) :
            base(vm, _this, method)
        { }
        public object Invoke(T1 a, T2 b) => vm.Run(method, _this, new object[] { a, b });
    }
    class CapturedDelegate<T1, T2, T3> : CapturedDelegate
    {
        public CapturedDelegate(VM vm, object _this, MethodDefinition method) :
            base(vm, _this, method)
        { }
        public object Invoke(T1 a, T2 b, T3 c) => vm.Run(method, _this, new object[] { a, b, c });
    }
}
