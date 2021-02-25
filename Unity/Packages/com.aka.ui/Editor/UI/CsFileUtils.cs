using System.IO;

namespace AkaUI.Editor
{
    public static class CsFileUtils
    {
        public static string GetNamespace(string path)
        {
            var lines = File.ReadLines(path);
            var result = "";
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                // 去除首尾空白字符
                var tmpLine = line.Trim();
                if (tmpLine.StartsWith("namespace"))
                {
                    result = tmpLine.Substring(10);
                    break;
                }
            }

            return result;
        }
    }
}