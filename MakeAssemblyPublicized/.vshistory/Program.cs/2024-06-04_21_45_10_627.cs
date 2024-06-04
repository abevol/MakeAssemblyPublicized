using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace MakeLibPublicized
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: MakeLibPublicized <path_to_your_dll.dll>");
                return;
            }

            string assemblyPath = args[0];
            var assembly = AssemblyDefinition.ReadAssembly(assemblyPath);

            foreach (var type in assembly.MainModule.Types)
            {
                MakePublic(type);
            }

            assembly.Write(assemblyPath.Replace(".dll", "-Publicized.dll"));
        }

        static void MakePublic(TypeDefinition type)
        {
            if (type.IsNested)
            {
                if (type.IsNestedPrivate || type.IsNestedAssembly || type.IsNestedFamilyAndAssembly)
                {
                    type.IsNestedPrivate = false;
                    type.IsNestedAssembly = false;
                    type.IsNestedFamilyAndAssembly = false;
                    type.IsNestedPublic = true;
                }
            }
            else
            {
                if (type.IsNotPublic)
                {
                    type.IsNotPublic = false;
                    type.IsPublic = true;
                }
            }

            foreach (var method in type.Methods)
            {
                if (method.IsAssembly || method.IsPrivate || method.IsFamily)
                {
                    method.IsAssembly = false;
                    method.IsPrivate = false;
                    method.IsFamily = false;
                    method.IsPublic = true;
                }
            }

            foreach (var field in type.Fields)
            {
                if (field.IsAssembly || field.IsPrivate || field.IsFamily)
                {
                    field.IsAssembly = false;
                    field.IsPrivate = false;
                    field.IsFamily = false;
                    field.IsPublic = true;
                }
            }

            foreach (var nestedType in type.NestedTypes)
            {
                MakePublic(nestedType);
            }
        }
    }
}