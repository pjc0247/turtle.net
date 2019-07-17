using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace unittest
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
                GenerateTestcases();
            else
                RunTestcase(args);
        }

        private static void GenerateTestcases()
        {
            var methods = Assembly.GetEntryAssembly()
                .GetTypes()
                .Where(x => x.Namespace == nameof(unittest))
                .SelectMany(x => x.GetMethods())
                .Where(x => x.IsPublic && x.IsStatic);

            foreach (var method in methods)
            {
                var path = $"testcase\\{method.DeclaringType.Name}.{method.Name}.txt";
                Console.WriteLine(path);
                File.WriteAllText(path, method.Invoke(null, new object[] { }).ToString());
            }
        }
        private static void RunTestcase(string[] args)
        {
            var path = args[0];
            var typename = path.Split('.')[0];
            var methodname = path.Split('.')[1];

            var method = Assembly.GetEntryAssembly()
                .GetTypes().Where(x => x.Name == typename)
                .FirstOrDefault()
                .GetMethod(methodname);

            var result = method.Invoke(null, new object[] { });
            Console.WriteLine(result);
        }
    }
}
