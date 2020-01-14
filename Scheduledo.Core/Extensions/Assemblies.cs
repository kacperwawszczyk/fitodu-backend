using System.IO;
using System.Reflection;

namespace Scheduledo.Core.Extensions
{
    public static class Assemblies
    {
        public static Stream GetStream(this Assembly assembly, string namespaceString)
        {
            var stream = assembly?.GetManifestResourceStream(namespaceString);

            return stream;
        }

        public static string GetResource(this Assembly assembly, string namespaceString)
        {
            using (var stream = GetStream(assembly, namespaceString))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
