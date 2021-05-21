using System.Text;

namespace Vgui.Parse
{
    public class Preprocessor
    {
        private readonly IFileSourceProvider _sourceProvider;

        public Preprocessor(IFileSourceProvider sourceProvider) => _sourceProvider = sourceProvider;

        public string Process(string file)
        {
            var input = _sourceProvider.ReadAllLines(file);
            var sb = new StringBuilder();
            foreach (var line in input)
            {
                var trimmed = line.Trim();
                if (trimmed.StartsWith("#base "))
                {
                    trimmed = trimmed.Substring("#base ".Length).Trim('"');
                    sb.Append(new Preprocessor(_sourceProvider).Process(trimmed) + "\n");
                }
                else
                {
                    sb.Append(line + "\n");
                }
            }

            return sb.ToString();
        }
    }
}