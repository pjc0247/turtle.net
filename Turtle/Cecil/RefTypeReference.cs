using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mono.Cecil;

namespace Turtle
{
    class RefTypeReference : TypeReference
    {
        public override TypeReference DeclaringType
        {
            get => typeRef.DeclaringType;
            set => typeRef.DeclaringType = value;
        }
        public override bool IsByReference => true;
        public override TypeReference GetElementType() => typeRef;

        private TypeReference typeRef;

        public RefTypeReference(TypeReference typeRef)
            : base(typeRef.Namespace, typeRef.Name, typeRef.Module, typeRef.Scope)
        {
            this.typeRef = typeRef;
        }
    }
}
