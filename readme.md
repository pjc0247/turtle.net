turtle.net
====
MSIL VM for certain purposes such as smart-contract and custom script engine.

```cs
using Turtle;

var vm = new VM();
vm.Run(new Instruction[]
{
    Instruction.Create(OpCodes.Ldstr, "HelloWorld"),
    Instruction.Create(OpCodes.Call, module.GetType("System.Console")
        .Methods
        .Where(x => x.Name == "WriteLine")
        .Where(x => x.Parameters.Count == 1 && x.Parameters[0]
            .ParameterType.Name == typeof(string).Name)
        .FirstOrDefault())
});
```