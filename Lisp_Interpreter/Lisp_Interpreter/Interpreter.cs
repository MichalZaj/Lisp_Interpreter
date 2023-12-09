using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lisp_Interpreter.SExpr;

namespace Lisp_Interpreter
{
    public class Interpreter : SExpr.IVisitor<double>
    {
        public double Evaluate(SExpr expr)
        {
            return expr.Accept(this);
        }

        public double VisitAtomSExpr(SExpr.Atom expr)
        {
            if (expr.Value is double)
            {
                return (double)expr.Value;
            }
            else
            {
                throw new InvalidOperationException("Invalid atomic value for evaluation.");
            }
        }

        public double VisitListSExpr(SExpr.List expr)
        {
            if (expr.Values.Count == 0)
            {
                throw new InvalidOperationException("Empty list encountered.");
            }

            SExpr first = expr.Values[0];

            // Check if the first element is an operator
            if (first is Addition || first is Subtraction || first is Multiplication || first is Division)
            {
                return EvaluateBinaryOperation(first, expr.Values.Skip(1).ToList());
            }

            if (!(first is SExpr.Atom firstAtom))
            {
                throw new InvalidOperationException("First element of the list must be an atom or operator.");
            }

            // Check for special forms or function calls based on the first atom in the list
            switch (firstAtom.Value)
            {
                case "+":
                    return EvaluateAddition(expr.Values.Skip(1).ToList());
                // Add cases for other functions or special forms here
                default:
                    throw new InvalidOperationException($"Unsupported function, special form, or operator: {firstAtom.Value}");
            }
        }

        private double EvaluateBinaryOperation(SExpr operation, List<SExpr> args)
        {
            switch (operation)
            {
                case Addition addition:
                    return Evaluate(addition.Left) + Evaluate(addition.Right);
                case Subtraction subtraction:
                    return Evaluate(subtraction.Left) - Evaluate(subtraction.Right);
                case Multiplication multiplication:
                    return Evaluate(multiplication.Left) * Evaluate(multiplication.Right);
                case Division division:
                    double right = Evaluate(division.Right);
                    if (right == 0)
                    {
                        throw new DivideByZeroException("Division by zero.");
                    }
                    return Evaluate(division.Left) / right;
                default:
                    throw new InvalidOperationException($"Invalid binary operation: {operation}");
            }
        }

        private double EvaluateAddition(List<SExpr> args)
        {
            if (args.Count < 2)
            {
                throw new InvalidOperationException("Addition requires at least two arguments.");
            }

            double result = 0;
            foreach (var arg in args)
            {
                result += Evaluate(arg);
            }

            return result;
        }

        public double VisitAddition(Addition expr)
        {
            return Evaluate(expr.Left) + Evaluate(expr.Right);
        }

        public double VisitSubtraction(Subtraction expr)
        {
            return Evaluate(expr.Left) - Evaluate(expr.Right);
        }

        public double VisitMultiplication(Multiplication expr)
        {
            return Evaluate(expr.Left) * Evaluate(expr.Right);
        }

        public double VisitDivision(Division expr)
        {
            double right = Evaluate(expr.Right);
            if (right == 0)
            {
                throw new DivideByZeroException("Division by zero.");
            }
            return Evaluate(expr.Left) / right;
        }

    }
}
