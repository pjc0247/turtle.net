using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Turtle
{
    struct Callframe
    {
        public MethodDefinition method;
        public object[] locals;
        public int bp;
        public Instruction cur;
    }
}
