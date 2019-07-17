using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mono.Cecil;

namespace Turtle
{
    public struct VObject
    {
        public VType type;
        public object[] fields;
        public TypeReference[] genericArgs;

        public VObject(VType type)
        {
            this.type = type;
            this.genericArgs = null;

            fields = new object[type.GetFields().Length];
        }

        public override string ToString()
        {
            return type.FullName;
        }
    }
}