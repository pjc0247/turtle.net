using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mono.Cecil;

namespace Turtle
{
    static class CecilExt
    {
        public static RefTypeReference ToRefType(this TypeReference typeRef)
        {
            return new RefTypeReference(typeRef);
        }
    }
}
