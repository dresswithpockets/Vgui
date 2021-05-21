namespace Vgui.Parse
{
    public interface IFileSourceProvider
    {
        string ReadAllText(string file);
        string[] ReadAllLines(string file);
    }
}