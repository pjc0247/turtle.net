using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Turtle
{
    public class VM
    {
        private OpCode[] ops;
        private Instruction cur;

        private object[] stack;
        private Stack<Callframe> callstack;
        private object ret;
        private int sp;
        private int bp;

        private object s1 {
            get => stack[sp - 1];
            set => stack[sp - 1] = value;
        }
        private object s2
        {
            get => stack[sp - 2];
            set => stack[sp - 2] = value;
        }
        private object s3
        {
            get => stack[sp - 3];
            set => stack[sp - 3] = value;
        }
        private object arg0 => stack[bp];

        private MethodDefinition method => callstack.Peek().method;
        private object[] locals => callstack.Peek().locals;

        internal TypeResolver typeResolver { get; private set; }

        public VM()
        {
            stack = new object[1024];
            callstack = new Stack<Callframe>();

            typeResolver = new TypeResolver();
        }

        public void Build(ModuleDefinition module)
        {
            foreach (var type in module.Types)
                BuildType(type);    
        }
        private void BuildType(TypeDefinition type)
        {
            var vtype = new VType(this, type);
            typeResolver.AddType(vtype);
        }

        public object Run(MethodDefinition method, object[] args)
        {
            foreach (var arg in args)
                Push(arg);

            PushMethod(method);
            Run(method.Body.Instructions.ToArray());

            return ret;
        }

        private void Run(Instruction[] op)
        {
            cur = op[0];
            while (cur != null)
            {
                Run(cur);

                cur = cur?.Next;
            }
        }
        private void Run(Instruction op)
        {
            Console.WriteLine(op.OpCode);

            switch (op.OpCode.Code)
            {
                case Code.Ldnull: RunLdnull(op); break;
                case Code.Ldstr: RunLdstr(op); break;
                case Code.Ldtoken: RunLdtoken(op); break;

                case Code.Ldarg_0: Push(arg0); break;

                case Code.Ldc_I4_0: Push(0); break;
                case Code.Ldc_I4_1: Push(1); break;
                case Code.Ldc_I4_2: Push(2); break;
                case Code.Ldc_I4_3: Push(3); break;
                case Code.Ldc_I4_4: Push(4); break;
                case Code.Ldc_I4_5: Push(5); break;
                case Code.Ldc_I4_6: Push(6); break;
                case Code.Ldc_I4_7: Push(7); break;
                case Code.Ldc_I4_8: Push(8); break;
                case Code.Ldc_I4_M1: Push(-1); break;
                case Code.Ldc_I4: 
                case Code.Ldc_I4_S:
                case Code.Ldc_R4:
                case Code.Ldc_R8:
                case Code.Ldc_I8: Push(op.Operand); break;

                case Code.Ldloc_0: Push(locals[0]); break;
                case Code.Ldloc_1: Push(locals[1]); break;
                case Code.Ldloc_2: Push(locals[2]); break;
                case Code.Ldloc_3: Push(locals[3]); break;
                case Code.Stloc_0: locals[0] = s1; Pop(); break;
                case Code.Stloc_1: locals[1] = s1; Pop(); break;
                case Code.Stloc_2: locals[2] = s1; Pop(); break;
                case Code.Stloc_3: locals[3] = s1; Pop(); break;

                case Code.Ldfld: RunLdfld(op); break;
                case Code.Stfld: RunStfld(op); break;
                case Code.Ldsfld: RunLdsfld(op); break;
                case Code.Stsfld: RunStsfld(op); break;
                case Code.Ldelem_I4: RunLdelem(op); break;
                case Code.Stelem_I4: RunStelem(op); break;

                case Code.Newobj: RunNewobj(op); break;
                case Code.Newarr: RunNewarr(op); break;

                case Code.Dup: Push(s1); break;
                case Code.Add: RunAdd(op); break;
                case Code.Sub: RunSub(op); break;
                case Code.Neg: RunNeg(op); break;

                case Code.Cgt: RunCgt(op); break;
                case Code.Clt: RunClt(op); break;
                case Code.Ceq: RunCeq(op); break;

                case Code.Br_S:
                case Code.Br: cur = (Instruction)op.Operand; break;
                case Code.Brfalse: RunBrfalse(op); break;
                case Code.Brfalse_S: RunBrfalse(op); break;
                case Code.Brtrue: RunBrtrue(op); break;
                case Code.Brtrue_S: RunBrtrue(op); break;

                case Code.Castclass: RunCastclass(op); break;
                case Code.Box: RunBox(op); break;
                case Code.Unbox: RunUnbox(op); break;

                case Code.Call: RunCall(op); break;
                case Code.Callvirt: RunCallvirt(op); break;

                case Code.Switch: RunSwitch(op); break;
                case Code.Throw: RunThrow(op); break;
                case Code.Ret: RunRet(op); break;
            }
        }

        private void Push(object n)
        {
            stack[sp] = n;
            sp++;
        }
        private object Pop() => stack[--sp];
        private void Pop(int n) => sp -= n;

        private void RunLdnull(Instruction op)
            => Push(null);
        private void RunLdstr(Instruction op)
        {
            Push(op.Operand);
        }
        private void RunLdtoken(Instruction op)
        {
            var fd = (FieldDefinition)op.Operand;
            var type = typeResolver.Resolve(fd.DeclaringType);
            Push(type.GetField(fd.Name, fd.GetBindingFlags()).FieldHandle);
        }

        private void RunLdelem(Instruction op)
        {
            var array = (Array)s2;
            var index = (int)s1;
            Push(array.GetValue(index));
        }
        private void RunStelem(Instruction op)
        {
            var array = (Array)s3;
            var index = (int)s2;
            array.SetValue(Pop(), index);
        }
        private void RunLdfld(Instruction op)
        {
            var obj = (VObject)s2;
            var fieldRef  = (FieldReference)op.Operand;
            var field = obj.type.GetField(fieldRef.Name);
            s1 = field.GetValue(obj);
        }
        private void RunStfld(Instruction op)
        {
            var obj = (VObject)s2;
            var fieldRef = (FieldReference)op.Operand;
            var field = obj.type.GetField(fieldRef.Name);
            field.SetValue(obj, s1);
            Pop(2);
        }
        private void RunLdsfld(Instruction op)
        {
            var fieldRef = (FieldReference)op.Operand;
            var field = typeResolver.Resolve(fieldRef.DeclaringType)
                .GetField(fieldRef.Name);
            Push(field.GetValue(null));
        }
        private void RunStsfld(Instruction op)
        {
            var fieldRef = (FieldReference)op.Operand;
            var field = typeResolver.Resolve(fieldRef.DeclaringType)
                .GetField(fieldRef.Name);
            field.SetValue(null, s1);
            Pop();
        }

        internal VObject Allocobj(VType type)
        {
            var obj = new VObject(type);
            Push(obj);
            return obj;
        }
        internal void Newobj(MethodReference ctor)
        {
            var type = typeResolver.Resolve(ctor.DeclaringType);
            var args = GetStack(ctor.Parameters.Count);
            VActivator.CreateInstance(this, type, args);
        }
        private void RunNewobj(Instruction op)
        {
            var ctor = (MethodReference)op.Operand;
            var typeRef = ctor.DeclaringType;
            var type = typeResolver.Resolve(typeRef);
            var args = GetStack(ctor.Parameters.Count);

            VActivator.CreateInstance(this, type, args);
        }
        private void RunNewarr(Instruction op)
        {
            var size = (int)Pop();
            var type = (TypeReference)op.Operand;

            var array = Array.CreateInstance(
                typeResolver.Resolve(type), size);

            Push(array);
        }

        private void RunAdd(Instruction op)
        {
            s2 = MadMath.Add(s2, s1);
            sp--;
        }
        private void RunSub(Instruction op)
        {
            s2 = MadMath.Sub(s2, s1);
            sp--;
        }
        private void RunNeg(Instruction op)
        {
            s1 = MadMath.Neg(s1);
        }

        private void RunCgt(Instruction op)
        {
            s2 = MadMath.G(s2, s1);
            sp--;
        }
        private void RunClt(Instruction op)
        {
            s2 = MadMath.L(s2, s1);
            sp--;
        }
        private void RunCeq(Instruction op)
        {
            s2 = MadMath.Eq(s2, s1);
            sp--;
        }

        private void RunBrfalse(Instruction op)
        {
            var _s1 = s1;
            if ((_s1 is bool b && !b) ||
                (_s1 is object o && o == null) ||
                (_s1.Equals(0)))
            {
                var inst = (Instruction)op.Operand;
                cur = (Instruction)op.Operand;
            }
        }
        private void RunBrtrue(Instruction op)
        {
            var _s1 = s1;
            if ((_s1 is bool b && b) ||
                (_s1 is object o && o != null) ||
                (!_s1.Equals(0)))
            {
                var inst = (Instruction)op.Operand;
                cur = (Instruction)op.Operand;
            }
        }

        private void RunCastclass(Instruction op)
        {
            var typeRef = (TypeReference)op.Operand;
            var type = typeResolver.Resolve(typeRef);
            s1 = Convert.ChangeType(s1, type);
        }
        private void RunBox(Instruction op)
        {
             s1 = (object)s1;
        }
        private void RunUnbox(Instruction op)
        {
            s1 = (object)s1;
        }

        private void RunCall(Instruction op)
        {
            var methodDef = ((MethodReference)op.Operand).Resolve();
            var asm = AssemblyResolver.GetAssembly(methodDef.Module.Assembly);

            //var type = asm.GetType(methodDef.DeclaringType.FullName);
            var type = typeResolver.Resolve(methodDef.DeclaringType);
            var argTypes = methodDef.Parameters
                .Select(x => x.ParameterType)
                .ToArray()
                .ToTypes(typeResolver);

            MethodBase method = null;
            if (methodDef.IsConstructor)
                method = type.GetConstructor(argTypes);
            else
                method = type.GetMethod(methodDef.Name, argTypes);

            if (methodDef.IsStatic)
            {
                var args = GetStack(method.GetParameters().Length);
                method.Invoke(null, args);
                Pop(args.Length);
            }
            else
            {
                var args = GetStack(method.GetParameters().Length);
                var _this = stack[sp - method.GetParameters().Length - 1];
                method.Invoke(_this, args);
                Pop(args.Length + 1);
            }
        }
        private void RunCallvirt(Instruction op)
        {
            RunCall(op);
        }

        private void RunSwitch(Instruction op)
        {
            ;
        }
        private void RunThrow(Instruction op)
        {
            throw (Exception)Pop();
        }
        private void RunRet(Instruction op)
        {
            if (method.ReturnType.FullName != typeof(void).FullName)
            {
                ret = s1;
                Pop();
            }

            PopMethod();
        }

        private object[] GetStack(int n)
        {
            var v = new object[n];
            for (int i = sp - n; i <= sp - 1; i++)
                v[i - (sp - n)] = stack[i];
            return v;
        }

        private void PushMethod(MethodDefinition method)
        {
            callstack.Push(new Callframe()
            {
                method = method,
                locals = new object[method.Body.Variables.Count],
                bp = bp,
                cur = cur
            });

            bp = sp - (method.Parameters.Count +
                (method.IsStatic ? 0 : 1));
            //method.Body.MaxStackSize
        }
        private void PopMethod()
        {
            var callframe = callstack.Pop();
            bp = callframe.bp;
            cur = callframe.cur;
        }
    }
}
