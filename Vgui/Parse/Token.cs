namespace Vgui.Parse
{
    public record Token(TokenType Type, string Value)
    {
        public TokenType Type { get; } = Type;
        public string Value { get; } = Value;
    }

    public enum TokenType
    {
        LeftBrace,
        RightBrace,
        Flag,
        String,
        Name,
    }
}