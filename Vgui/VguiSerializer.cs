using System.Diagnostics;
using System.Linq;
using Vgui.Parse;

namespace Vgui
{
    public static class VguiSerializer
    {
        public static VguiObject FromFile(string rootPath, string file) =>
            FromFile(file, new HudFileSourceProvider(rootPath));

        public static VguiObject FromFile(string file, IFileSourceProvider sourceProvider)
        {
            var preprocessed = new Preprocessor(sourceProvider).Process(file);
            var tokens = new Lexer(preprocessed).GetTokens();
            var root = new Parser(tokens.ToArray()).ParseRoot();
            return FromValue(root);
        }

        public static VguiObject FromValue(Value value)
        {
            if (value.String != null) return new VguiObject(value.Name.Value, value.String.Value);
            var obj = new VguiObject(value.Name.Value);
            Debug.Assert(value.Body != null);
            foreach (var subValue in value.Body.Values)
                obj.MergeOrAddProperty(FromValue(subValue));

            return obj;
        }
    }
}