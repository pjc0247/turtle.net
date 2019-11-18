turtle.net
====
.Net VM built on a top of .Net.<br>
for certain purposes such as smart-contract and custom script engine.


__`Hello World` on the turtle VM__
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

Getting Started
----
```cs
var vm = new VM();

// Needs preparation before run
vm.Build(program);
vm.Run(program.Assembly.EntryPoint, new object[] { args });
```

Storage Engine
----
You can implement your own Storage Engine for certain needs. (Blockchain would be a good example)

```cs
public interface IStorage
{
    void Set(long key, object value);
    object Get(long key);
    long GetNextKey();
}
```
blahblah<br>
Otherwise, [MemStorage](https://github.com/pjc0247/turtle.net/blob/master/Turtle/Storage/MemStorage.cs) will be used by default.

Running Tests
----
__turtle.net__ has its own test framework. Please refer to `unittest.runner` project.

FIR Compliation (Concept)
----
CIL is not aimed to achieve fast execution itself since they already have good JIT compiler to make it fast. However, the main purpose of this repository is getting rid of the JIT and machine-specific assemblies.<br>
So, we need to invent another instruction system which is performance-aware and having well-aligned structure.<br>
blahblah
