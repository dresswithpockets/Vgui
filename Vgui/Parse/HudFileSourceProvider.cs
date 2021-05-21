using System.IO;

namespace Vgui.Parse
{
    public class HudFileSourceProvider : IFileSourceProvider
    {
        private readonly string _root;

        public HudFileSourceProvider(string root) => _root = root;

        public string ReadAllText(string file) => File.ReadAllText(Path.Combine(_root, file));
        
        public string[] ReadAllLines(string file) => File.ReadAllLines(Path.Combine(_root, file));
    }
}