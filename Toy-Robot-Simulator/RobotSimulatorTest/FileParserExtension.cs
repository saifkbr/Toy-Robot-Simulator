namespace RobotSimulatorTest
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public static class FileParserExtension
    {
        public static async Task<string> GetEmbeddedResourceStreamAsync
            (this Assembly assembly, string relativeResourcePath)
        {
            if (string.IsNullOrEmpty(relativeResourcePath))
                throw new ArgumentNullException("relativeResourcePath");

            var resourcePath = String.Format("{0}.{1}",
                Regex.Replace(assembly.ManifestModule.Name, @"\.(exe|dll)$",
                      string.Empty, RegexOptions.IgnoreCase), relativeResourcePath);

            var stream = assembly.GetManifestResourceStream(resourcePath);

            if (stream == null)
                throw new ArgumentException($"The specified embedded resource '{relativeResourcePath}' is not found.");

            return await ReadTextAsync(stream);
        }

        private static async Task<string> ReadTextAsync(Stream stream)
        {
            var sb = new StringBuilder();

            byte[] buffer = new byte[0x1000];
            int numRead;
            while ((numRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
            {
                string text = Encoding.Default.GetString(buffer, 0, numRead);
                sb.Append(text.Replace("\r\n", ""));
            }

            return sb.ToString();
        }
    }
}
