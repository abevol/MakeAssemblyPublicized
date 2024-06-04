using Mono.Cecil;
using System;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("Usage: ChangeAccessibility <path_to_your_dll.dll>");
            return;
        }

        string assemblyPath = args[0];
        var assembly = AssemblyDefinition.ReadAssembly(assemblyPath);

        foreach (var type in assembly.MainModule.Types)
        {
            foreach (var method in type.Methods)
            {
                if (method.IsPrivate)
                    method.IsPublic = true;
            }

            foreach (var field in type.Fields)
            {
                if (field.IsPrivate)
                    field.IsPublic = true;
            }
        }

        assembly.Write(assemblyPath.Replace(".dll", "-Publicized.dll"));
    }
}
