using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mono.Cecil;

namespace Turtle
{
    struct Callframe
    {
        public MethodDefinition method;
        public object[] locals;
        public int bp;
    }
}
