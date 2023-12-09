using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lisp_Interpreter
{
    public abstract class SExpr
    {
        public abstract R Accept<R>(IVisitor<R> visitor);

        public interface IVisitor<R>
        {
            R VisitAtomSExpr(Atom expr);
            R VisitListSExpr(List expr);
            R VisitAddition(Addition expr);
            R VisitSubtraction(Subtraction expr);
            R VisitMultiplication(Multiplication expr);
            R VisitDivision(Division expr);
        }
        public class Atom : SExpr
        {
            public readonly object Value;

            public Atom(object value)
            {
                Value = value;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitAtomSExpr(this);
            }

            public override string ToString()
            {
                if (Value is string s)
                {
                    return $"\"{s}\"";
                }
                else if (Value is Token t)
                {
                    return t.Lexeme;
                }
                else
                {
                    return Value.ToString();
                }
            }
        }
        public class List : SExpr
        {
            public readonly List<SExpr> Values;

            public List(List<SExpr> values)
            {
                Values = values;
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitListSExpr(this);
            }

            public override string ToString()
            {
                string output = "(";

                for (int i = 0; i < Values.Count; i++)
                {
                    output += Values[i].ToString();

                    if (i != Values.Count - 1)
                    {
                        output += " ";
                    }
                }

                output += ")";
                return output;
            }
        }
        public abstract class BinaryOperator : SExpr
        {
            public readonly SExpr Left;
            public readonly SExpr Right;

            public BinaryOperator(SExpr left, SExpr right)
            {
                Left = left;
                Right = right;
            }
        }

        public class Addition : BinaryOperator
        {
            public Addition(SExpr left, SExpr right) : base(left, right)
            {
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitAddition(this);
            }
        }

        public class Subtraction : BinaryOperator
        {
            public Subtraction(SExpr left, SExpr right) : base(left, right)
            {
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitSubtraction(this);
            }
        }

        public class Multiplication : BinaryOperator
        {
            public Multiplication(SExpr left, SExpr right) : base(left, right)
            {
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitMultiplication(this);
            }
        }

        public class Division : BinaryOperator
        {
            public Division(SExpr left, SExpr right) : base(left, right)
            {
            }

            public override R Accept<R>(IVisitor<R> visitor)
            {
                return visitor.VisitDivision(this);
            }
        }


    }
}
