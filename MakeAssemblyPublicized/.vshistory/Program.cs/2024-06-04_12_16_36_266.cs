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
            // 检查是否提供了DLL文件的路径
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: ChangeAccessibility <path_to_your_dll.dll>");
                return;
            }

            string assemblyPath = args[0]; // 从命令行参数获取DLL文件的路径
            var assembly = AssemblyDefinition.ReadAssembly(assemblyPath);

            foreach (var type in assembly.MainModule.Types)
            {
                type.IsNotPublic = false;
                type.IsPublic = true;

                foreach (var method in type.Methods)
                {
                    method.IsAssembly = false;
                    method.IsPrivate = false;
                    method.IsFamily = false;
                    method.IsPublic = true;
                }

                foreach (var field in type.Fields)
                {
                    if (field.IsPrivate)
                    {
                        field.IsPrivate = false;
                        field.IsPublic = true;
                    }

                    if (field.IsFamily)
                    {
                        field.IsFamily = false;
                        field.IsPublic = true;
                    }

                    method.IsAssembly = false;
                    method.IsPrivate = false;
                    method.IsFamily = false;
                    method.IsPublic = true;
                }
            }

            // 保存修改后的程序集
            assembly.Write(assemblyPath.Replace(".dll", "-Publicized.dll"));
        }
    }
}