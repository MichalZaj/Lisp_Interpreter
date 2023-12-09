using System;
using System.Collections.Generic;
using static Lisp_Interpreter.SExpr;

namespace Lisp_Interpreter
{
    public class Parser
    {
        private readonly List<Token> tokens;
        private int current = 0;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        public SExpr Parse()
        {
            try
            {
                return Expression();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Parser error: {ex.Message}");
                return null;
            }
        }

        private SExpr Expression()
        {
            if (Match(TokenType.LEFT_PAREN))
            {
                return ParseList();
            }
            else if (Match(TokenType.NUMBER, TokenType.STRING, TokenType.IDENTIFIER))
            {
                return new SExpr.Atom(Previous().Literal);
            }
            else if (Match(TokenType.PLUS, TokenType.MINUS, TokenType.STAR, TokenType.SLASH))
            {
                return ParseBinary();
            }
            else if (Match(TokenType.EQUAL, TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL))
            {
                throw Error("Unexpected token");
            }
            else
            {
                throw Error("Unexpected token");
            }
        }

        private SExpr ParseBinary()
        {
            Token op = Previous();
            SExpr left = Expression();
            SExpr right = Expression();

            // Use the appropriate class based on the operator
            switch (op.Type)
            {
                case TokenType.PLUS:
                    return new Addition(left, right);
                case TokenType.MINUS:
                    return new Subtraction(left, right);
                case TokenType.STAR:
                    return new Multiplication(left, right);
                case TokenType.SLASH:
                    return new Division(left, right);
                default:
                    throw Error("Invalid binary operator");
            }
        }

        private SExpr ParseList()
        {
            List<SExpr> elements = new List<SExpr>();

            while (!Check(TokenType.RIGHT_PAREN) && !IsAtEnd())
            {
                elements.Add(Expression());
            }

            Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");

            return new SExpr.List(elements);
        }

        private Token Consume(TokenType type, string message)
        {
            if (Check(type))
                return Advance();

            throw Error(message);
        }

        private bool Match(params TokenType[] types)
        {
            foreach (var type in types)
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }

            return false;
        }

        private bool Check(TokenType type)
        {
            if (IsAtEnd())
                return false;
            return Peek().Type == type;
        }

        private Token Advance()
        {
            if (!IsAtEnd())
                current++;
            return Previous();
        }

        private Token Peek()
        {
            return tokens[current];
        }

        private bool IsAtEnd()
        {
            return Peek().Type == TokenType.EOF;
        }

        private Token Previous()
        {
            return tokens[current - 1];
        }

        private ParseException Error(string message)
        {
            return new ParseException(Peek(), message);
        }
    }

    public class ParseException : Exception
    {
        public readonly Token Token;

        public ParseException(Token token, string message) : base(message)
        {
            Token = token;
        }
    }
}

