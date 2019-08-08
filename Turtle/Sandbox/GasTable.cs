using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mono.Cecil.Cil;

namespace Turtle
{
    public static class GasTable
    {
        public static int GetGasFee(Code code)
        {
            switch (code)
            {
                case Code.Mul:
                case Code.Mul_Ovf:
                case Code.Mul_Ovf_Un:
                case Code.Add_Ovf:
                case Code.Add_Ovf_Un:
                case Code.Sub_Ovf:
                case Code.Sub_Ovf_Un:
                case Code.Div:
                case Code.Div_Un:
                case Code.Sub:
                case Code.Add: return 1;

                case Code.Stfld: return 100;
                case Code.Ldflda:
                case Code.Ldftn:
                case Code.Ldfld: return 10;

                default: return 0;
            }
        }
    }
}
