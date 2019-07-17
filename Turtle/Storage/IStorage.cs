using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turtle
{
    public interface IStorage
    {
        void Set(long key, object value);
        object Get(long key);

        long GetNextKey();
    }
}
