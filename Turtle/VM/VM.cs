using System;
using System.IO;
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
        internal Assembly assembly;
        private string basepath;

        private OpCode[] ops;
        private Instruction cur;

        private object[] stack;
        private Stack<Callframe> callstack;
        private int sp;
        private int bp;

        private TypeReference[] genericBounds;
        private TypeReference[] typeGenericBounds;
        private TypeReference[] methodGenericBounds;

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
        private object arg1 => stack[bp + 1];
        private object arg2 => stack[bp + 2];
        private object arg3 => stack[bp + 3];

        private MethodDefinition method => callstack.Peek().method;
        private object[] locals => callstack.Peek().locals;

        internal TypeResolver typeResolver { get; private set; }
        internal IStorage storage { get; private set; }

        private List<VType> types = new List<VType>();

        public VM(string basepath)
        {
            this.basepath = basepath;

            stack = new object[1024];
            callstack = new Stack<Callframe>();

            typeResolver = new TypeResolver(basepath);
            storage = new MemStorage();
        }

        public void Build(ModuleDefinition module)
        {
            assembly = Assembly.LoadFile(Path.Combine(Path.GetFullPath(basepath), module.FileName));

            Console.WriteLine("===BUILD===");
            foreach (var type in module.Types)
            {
                types.Add(BuildType(type));
                foreach (var type2 in type.NestedTypes)
                    types.Add(BuildType(type2));
            }
            foreach (var type in types)
                type.Initialize();
            Console.WriteLine("===BUILD END===");
        }
        private VType BuildType(TypeDefinition type)
        {
            var vType = new VType(this, type);
            typeResolver.AddType(vType);
            return vType;
        }

        public object Run(MethodDefinition method, object _this, object[] args)
        {
            if (_this == null)
                throw new NullReferenceException();

            Push(_this);
            if (_this is VObject vo)
                genericBounds = vo.genericArgs;

            return Run(method, args);
        }
        public object Run(MethodDefinition method, object[] args)
        {
            foreach (var arg in args)
                Push(arg);

            PushMethod(method);
            Run(method.Body.Instructions.ToArray());

            if (method.ReturnType.FullName == typeof(void).FullName)
                return null;
            return s1;
        }

        private void Run(Instruction[] ops)
        {
            cur = ops[0];
            while (cur != null)
            {
                var op = cur;

                Run(op);

                if (op.OpCode.Code == Code.Ret)
                    return;
                if (cur == op)
                    cur = op?.Next;
            }
        }
        private void Run(Instruction op)
        {
            Console.WriteLine(method.FullName + " / " +  op.OpCode + " / " + sp);

            switch (op.OpCode.Code)
            {
                case Code.Ldnull: RunLdnull(op); break;
                case Code.Ldstr: RunLdstr(op); break;
                case Code.Ldtoken: RunLdtoken(op); break;
                case Code.Ldlen: RunLdlen(op); break;
                case Code.Ldftn: RunLdftn(op); break;

                case Code.Ldarg_0: Push(arg0); break;
                case Code.Ldarg_1: Push(arg1); break;
                case Code.Ldarg_2: Push(arg2); break;
                case Code.Ldarg_3: Push(arg3); break;

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
                case Code.Ldloca: Push(locals[((VariableDefinition)op.Operand).Index]); break;
                case Code.Ldloca_S: Push(locals[((VariableDefinition)op.Operand).Index]); break;

                case Code.Ldfld: RunLdfld(op); break;
                case Code.Stfld: RunStfld(op); break;
                case Code.Ldsfld: RunLdsfld(op); break;
                case Code.Stsfld: RunStsfld(op); break;
                case Code.Ldelem_I4: RunLdelem(op); break;
                case Code.Stelem_I4: RunStelem(op); break;

                case Code.Newobj: RunNewobj(op); break;
                case Code.Newarr: RunNewarr(op); break;
                case Code.Initobj: RunInitobj(op); break;

                case Code.Dup: Push(s1); break;
                case Code.Add: RunAdd(op); break;
                case Code.Sub: RunSub(op); break;
                case Code.Mul: RunMul(op); break;
                case Code.Div: RunDiv(op); break;
                case Code.Mul_Ovf: RunMod(op); break;
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
                case Code.Blt:
                case Code.Blt_S: RunBlt(op); break;
                case Code.Bgt:
                case Code.Bgt_S: RunBlt(op); break;

                case Code.Castclass: RunCastclass(op); break;
                case Code.Box: RunBox(op); break;
                case Code.Unbox: RunUnbox(op); break;

                case Code.Call: RunCall(op); break;
                case Code.Callvirt: RunCallvirt(op); break;

                case Code.Switch: RunSwitch(op); break;
                case Code.Throw: RunThrow(op); break;
                case Code.Ret: RunRet(op); break;

                default:
                    Console.WriteLine("Unknown opcode: " + op.OpCode.Code);
                    break;
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
            if (op.Operand is GenericParameter g)
            {
                var type = typeResolver.Resolve(genericBounds[g.Position]);
                Push(type.TypeHandle);
            }
            else if (op.Operand is FieldDefinition fd)
            {
                var type = assembly.GetType(fd.DeclaringType.Name);
                //var type = typeResolver.Resolve(fd.DeclaringType);
                Push(type.GetField(fd.Name, fd.GetBindingFlags()).FieldHandle);
            }
        }
        private void RunLdlen(Instruction op)
        {
            var array = (Array)s1;
            s1 = array.Length;
        }
        private void RunLdftn(Instruction op)
        {
            var methodRef = (MethodReference)op.Operand;
            var type = typeResolver.Resolve(methodRef.DeclaringType);
            var method = type.GetMethod(methodRef.Name);

            var types = method.GetParameters().Select(x => x.ParameterType);
            if (method.ReturnType.FullName != typeof(void).FullName)
                types = types.Concat(new Type[] { method.ReturnType });

            (var del, var ptr) = CapturedDelegateFactory.Create(
                types.ToArray(),
                this, s1, (MethodDefinition)methodRef);

            s1 = del;
            Push(ptr);
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
            var obj = (VObject)s1;
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
            return obj;
        }
        internal VObject AllocobjStack(VType type)
        {
            var obj = new VObject(type);
            Push(obj);
            return obj;
        }
        internal void Newobj(MethodReference ctor)
        {
            var type = typeResolver.Resolve(ctor.DeclaringType);
            var args = GetStack(ctor.Parameters.Count);
            Push(VActivator.CreateInstance(this, ctor, type, args));
        }
        internal void Dup()
        {
            Push(s1);
        }
        private void RunNewobj(Instruction op)
        {
            var ctor = (MethodReference)op.Operand;
            Newobj(ctor);
        }
        private void RunNewarr(Instruction op)
        {
            var size = (int)Pop();
            var type = (TypeReference)op.Operand;

            var array = Array.CreateInstance(
                typeResolver.Resolve(type), size);

            Push(array);
        }
        private void RunInitobj(Instruction op)
        {
            Pop();
            /*
            var typeRef = (TypeReference)op.Operand;
            var type = typeResolver.Resolve(typeRef);
            var addr = (int)Pop();

            var obj = VActivator.CreateInstance(this, type, new object[] { });

            locals[addr] = obj;
            */
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
        private void RunMul(Instruction op)
        {
            s2 = MadMath.Mul(s2, s1);
            sp--;
        }
        private void RunDiv(Instruction op)
        {
            s2 = MadMath.Div(s2, s1);
            sp--;
        }
        private void RunMod(Instruction op)
        {
            s2 = MadMath.Mod(s2, s1);
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
                (_s1 != null && !_s1.Equals(0)))
            {
                var inst = (Instruction)op.Operand;
                cur = (Instruction)op.Operand;
            }
        }
        private void RunBlt(Instruction op)
        {
            if (MadMath.L(s2, s1))
                cur = (Instruction)op.Operand;
        }
        private void RunBgt(Instruction op)
        {
            if (MadMath.G(s2, s1))
                cur = (Instruction)op.Operand;
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
            var methodRef = (MethodReference)op.Operand;
            var methodDef = methodRef.Resolve();

            if (methodRef is GenericInstanceMethod gMethod)
            {
                methodGenericBounds = new TypeReference[gMethod.GenericArguments.Count];
                for (int i = 0; i < gMethod.GenericArguments.Count; i++)
                    methodGenericBounds[i] = gMethod.GenericArguments[i];
            }
            if (methodRef.DeclaringType is GenericInstanceType g)
            {
                // Fixme
                genericBounds = new TypeReference[g.GenericArguments.Count];
                for (int i = 0; i < g.GenericArguments.Count; i++)
                {
                    genericBounds[i] = g.GenericArguments[i];
                }
            }

            Console.WriteLine("CALL " + methodDef.Name + " / " + sp);

            //var type = asm.GetType(methodDef.DeclaringType.FullName);
            var type = typeResolver.Resolve(methodRef.DeclaringType);
            var argTypes = methodRef.Parameters
                .Select(x => x.ParameterType)
                .Select(x => ResolveGenericParamter(x))
                .ToArray()
                .ToTypes(typeResolver);

            MethodBase method = null;
            if (methodDef.IsConstructor)
            {
                method = type.GetConstructor(argTypes);
                if (method == null)
                    return;
            }
            else
                method = type.GetMethodExt(methodDef.Name, argTypes);

            MethodBase methodToCall = method;
            if (method.ContainsGenericParameters)
            {
                var methodInfo = (MethodInfo)method;
                methodToCall = methodInfo.MakeGenericMethod(methodGenericBounds.ToTypes(typeResolver));
            }

            if (methodDef.IsStatic)
            {
                var args = GetStack(method.GetParameters().Length);
                var ret = methodToCall.Invoke(null, args);
                Pop(args.Length);

                if (((MethodInfo)method).ReturnType.FullName != typeof(void).FullName)
                    Push(ret);
            }
            else
            {
                var args = GetStack(method.GetParameters().Length);
                var _this = stack[sp - method.GetParameters().Length - 1];
                var ret = methodToCall.Invoke(_this, args);

                if (method is MethodInfo methodInfo &&
                    methodInfo.ReturnType.FullName != typeof(void).FullName)
                    Push(ret);

                Pop(args.Length + 1);
            }
        }
        private TypeReference ResolveGenericParamter(TypeReference typeRef)
        {
            TypeReference _ref = typeRef;
            if (typeRef.Name.StartsWith("!!"))
                _ref = methodGenericBounds[int.Parse(typeRef.Name.Substring(2, 1))];
            else if (typeRef.Name.StartsWith("!"))
                _ref = methodGenericBounds[int.Parse(typeRef.Name.Substring(1, 1))];

            if (typeRef.Name.EndsWith("&"))
                _ref = _ref.ToRefType();

            return _ref;
        }

        private void RunCallvirt(Instruction op)
        {
            var methodRef = (MethodReference)op.Operand;
            var _this = stack[sp - methodRef.Parameters.Count - 1];

            if (_this is VObject vo)
            {
                var argTypes = methodRef.Parameters
                    .Select(x => x.ParameterType)
                    .Select(x => x.Name.StartsWith("!") ? genericBounds[int.Parse(x.Name.Substring(1))] : x)
                    .ToArray()
                    .ToTypes(typeResolver);
                var method = vo.type.GetMethod(methodRef.Name, argTypes);
                var args = GetStack(method.GetParameters().Length);
                method.Invoke(_this, args);
                Pop(args.Length + 1);

                return;
            }

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
            object ret = null;
            var hasReturn = method.ReturnType.FullName != typeof(void).FullName;
            if (hasReturn)
                ret = s1;

            PopMethod();

            if (hasReturn)
                Push(ret);
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

            InitLocals();
            //method.Body.MaxStackSize
        }
        private void PopMethod()
        {
            var callframe = callstack.Pop();
            bp = callframe.bp;
            cur = callframe.cur;

            var method = callframe.method;
            sp -= (method.Parameters.Count +
                (method.IsStatic ? 0 : 1));

            // temp
            //Array.Clear(stack, sp, 64);
        }
        private void InitLocals()
        {
            var vars = method.Body.Variables;
            for (int i = 0; i < vars.Count; i++)
            {
                var typeRef = vars[i].VariableType;
                var isStruct = typeRef.IsValueType && !typeRef.IsPrimitive;
                if (isStruct)
                {
                    var type = typeResolver.Resolve(typeRef);
                    if (type is VType vType)
                        locals[i] = Allocobj(vType);
                    else
                        locals[i] = Activator.CreateInstance(type);
                }
            }
        }
    }
}
