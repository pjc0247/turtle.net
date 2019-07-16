using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turtle
{
    public struct VObject
    {
        public VType type;
        public object[] fields;

        public VObject(VType type)
        {
            this.type = type;

            fields = new object[type.GetFields().Length];
        }

        public override string ToString()
        {
            return type.FullName;
        }
    }
}