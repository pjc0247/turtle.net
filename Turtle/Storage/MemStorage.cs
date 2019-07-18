using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turtle
{
    public class MemStorage : IStorage
    {
        private Dictionary<long, object> mem = new Dictionary<long, object>();
        private long nextKey;

        public void Set(long key, object value)
        {
            Console.WriteLine($"$SET {key} = {value}");
            mem[key] = value;
        }
        public object Get(long key)
        {
            Console.WriteLine($"$GET {key}");
            return mem[key];
        }

        public long GetNextKey()
        {
            // everything is not thread-safe at this moment
            return nextKey++;
        }
    }
}
