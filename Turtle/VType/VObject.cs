using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turtle
{
    public struct VObject
    {
        public object[] fields;

        public VObject(VType type)
        {
            fields = new object[type.GetFields().Length];
        }
    }
}