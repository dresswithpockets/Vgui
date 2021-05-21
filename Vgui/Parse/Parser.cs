using System;
using System.Collections.Generic;
using System.Linq;

namespace Vgui.Parse
{
    public class Parser
    {
        private readonly Token[] _tokens;
        private int _index;
        private Token? Current => Peek(0);

        public Parser(Token[] tokens) => _tokens = tokens;

        private Token? Peek(int n = 1) => _tokens.ElementAtOrDefault(_index + n);

        private Token Eat(TokenType type)
        {
            var current = Current;
            if (current?.Type != type) throw new Exception($"Expected {type} but got {current?.Type} instead.");
            _index++;
            return current;
        }

        private Token Eat(params TokenType[] types)
        {
            var current = Current;
            if (current == null || !types.Contains(current.Type))
            {
                throw new Exception(
                    $"Expected one of ({string.Join(",", types)}) but got {current?.Type} instead.");
            }

            _index++;
            return current;
        }

        private IEnumerable<Value> NextValueList()
        {
            while (Current?.Type == TokenType.String || Current?.Type == TokenType.Name)
                yield return NextValue();
        }

        private Value NextValue()
        {
            var name = Eat(TokenType.String, TokenType.Name);
            if (Current?.Type == TokenType.LeftBrace)
                return new Value(name, null, NextValueBody());
            return new Value(name, Eat(TokenType.String), null);
        }

        private ValueBody NextValueBody() => new(NextValueList().ToArray());

        public Value ParseRoot() =>
            new(new Token(TokenType.Name, "Root"), null, new ValueBody(NextValueList().ToArray()));
    }

    public record Value(Token Name, Token? String, ValueBody? Body)
    {
        public Token Name { get; } = Name;
        public Token? String { get; } = String;
        public ValueBody? Body { get; } = Body;
    }

    public record ValueBody(Value[] Values)
    {
        public Value[] Values { get; } = Values;
    }
}