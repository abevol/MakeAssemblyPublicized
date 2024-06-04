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

            // 保存修改后的程序集
            assembly.Write(assemblyPath.Replace(".dll", "_modified.dll"));
        }
    }
}
