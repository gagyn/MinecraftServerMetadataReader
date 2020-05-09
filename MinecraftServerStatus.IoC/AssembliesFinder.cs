using System.Collections.Generic;
using System.Reflection;

namespace MinecraftServerStatus.IoC
{
    public class AssembliesFinder
    {
        public List<Assembly> GetAllReferencedAssemblies(Assembly startAssembly, string nameOfSolution)
        {
            var solutionAssemblies = new List<Assembly>();
            var list = new List<string>();
            var stack = new Stack<Assembly>();
            stack.Push(startAssembly);

            while (stack.Count > 0)
            {
                var asm = stack.Pop();
                if (asm.FullName.StartsWith(nameOfSolution))
                {
                    solutionAssemblies.Add(asm);
                }

                foreach (var reference in asm.GetReferencedAssemblies())
                {
                    if (!list.Contains(reference.FullName))
                    {
                        stack.Push(Assembly.Load(reference));
                        list.Add(reference.FullName);
                    }
                }
            }
            return solutionAssemblies;
        }
    }
}
