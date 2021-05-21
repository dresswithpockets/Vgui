using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Vgui.Parse
{
    public class Lexer
    {
        private static readonly char[] Newlines = { '\n', '\r', '\f' };

        private readonly string _input;
        private int _index;
        private char? Current => Peek(0);

        public Lexer(string input) => _input = input;
        
        private char? Peek(int n = 1) => _input.Length > _index + n ? _input[_index + n] : (char?)null;

        private void Advance(int n = 1) => _index += n;

        private void SkipWhitespace()
        {
            char? current;
            while ((current = Peek(0)) != null && char.IsWhiteSpace(current.Value))
                Advance();
        }

        private void SkipLine()
        {
            char? current;
            var hasReachedNewLine = false;
            while ((current = Peek(0)) != null && (!hasReachedNewLine || Newlines.Contains(current.Value)))
            {
                if (Newlines.Contains(current.Value)) hasReachedNewLine = true;
                Advance();
            }
        }

        private Token GetStringToken()
        {
            var sb = new StringBuilder();
            char? current;
            while ((current = Peek(0)) != null && current != '"')
            {
                sb.Append(current);
                Advance();
            }

            Advance();
            return new Token(TokenType.String, sb.ToString());
        }

        private Token GetNameToken()
        {
            var sb = new StringBuilder();
            char? current;
            while ((current = Peek(0)) != null && char.IsLetterOrDigit(current.Value))
            {
                sb.Append(current);
                Advance();
            }
            return new Token(TokenType.Name, sb.ToString());
        }

        private Token GetFlagToken()
        {
            var sb = new StringBuilder();
            char? current;
            while ((current = Peek(0)) != null && current != ']')
            {
                sb.Append(current);
                Advance();
            }

            Advance();
            return new Token(TokenType.Flag, sb.ToString());
        }

        public IEnumerable<Token> GetTokens()
        {
            while (Current != null)
            {
                SkipWhitespace();
                
                // skip over comments
                if (Current == '/' && Peek() == '/')
                {
                    SkipLine();
                    continue;
                }
                
                // skip over directives like #base - these should be handled by the preprocessor
                if (Current == '#')
                {
                    SkipLine();
                    continue;
                }

                if (Current == '"')
                {
                    Advance();
                    yield return GetStringToken();
                    continue;
                }

                Debug.Assert(Current != null, nameof(Current) + " != null");
                if (char.IsLetterOrDigit(Current.Value))
                {
                    yield return GetNameToken();
                    continue;
                }

                if (Current == '[')
                {
                    yield return GetFlagToken();
                    continue;
                }

                if (Current == '{')
                {
                    yield return new Token(TokenType.LeftBrace, "{");
                    continue;
                }

                if (Current == '}')
                {
                    yield return new Token(TokenType.RightBrace, "}");
                    continue;
                }
            }
        }
    }
}