using System.IO;
using System.Reflection;

namespace ET
{
    public static class DllHelper
    {
        public static Assembly GetHotfixAssembly() => GetHotfixAssembly("Hotfix");

        public static Assembly GetHotfixAssembly(string name)
        {
            byte[] dllBytes = File.ReadAllBytes($"./{name}.dll");
            byte[] pdbBytes = File.ReadAllBytes($"./{name}.pdb");
            Assembly assembly = Assembly.Load(dllBytes, pdbBytes);

            return assembly;
        }
    }
}