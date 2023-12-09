using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lisp_Interpreter
{

    public class Lisp
    {
        static bool HadError = false;

        public static void Main(string[] args)
        {
            try
            {
                if (args.Length > 1)
                {
                    Console.WriteLine("Usage: clox [script]");
                    Environment.Exit(64);
                }
                else if (args.Length == 1)
                {
                    RunFile(args[0]);
                }
                else
                {
                    RunPrompt();
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
                Environment.Exit(74);
            }
        }

        private static void RunFile(string path)
        {
            try
            {
                byte[] bytes = File.ReadAllBytes(path);
                string content = Encoding.Default.GetString(bytes);
                Run(content);

                if (HadError)
                {
                    Environment.Exit(65);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error reading file '{path}': {ex.Message}");
                Environment.Exit(74); // Exit code for IO error
            }
        }

        private static void RunPrompt()
        {
            try
            {
                for (; ; )
                {
                    Console.Write("> ");
                    string line = Console.ReadLine();
                    if (line == null) break;
                    Run(line);
                    HadError = false;
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error reading input: {ex.Message}");
                Environment.Exit(74);
            }
        }

        private static void Run(string source)
        {
            try
            {
                Scanner scanner = new Scanner(source);
                List<Token> tokens = scanner.ScanTokens();

                // Print the tokens for debugging
                Console.WriteLine("Tokens:");
                foreach (var token in tokens)
                {
                    Console.WriteLine(token);
                }

                // Create a parser and parse the tokens
                Parser parser = new Parser(tokens);
                SExpr ast = parser.Parse();

                // Evaluate the AST
                if (ast != null)
                {
                    Console.WriteLine("\nAST:");
                    PrintAST(ast, 0);

                    Console.WriteLine("\nResult:");
                    Interpreter interpreter = new Interpreter();
                    double result = interpreter.Evaluate(ast);
                    Console.WriteLine(result);
                }
            }
            catch (ParseException ex)
            {
                Console.WriteLine($"Parser error: {ex.Message}");
            }
        }



        private static void PrintAST(SExpr expr, int depth)
        {
            string padding = new string(' ', depth * 2);
            Console.WriteLine($"{padding}{expr.GetType().Name}");

            if (expr is SExpr.List list)
            {
                foreach (var element in list.Values)
                {
                    PrintAST(element, depth + 1);
                }
            }
        }

        public static void Error(int line, string message)
        {
            Report(line, "", message);
        }

        private static void Report(int line, string where, string message)
        {
            Console.Error.WriteLine($"[line {line}] Error{where}: {message}");
            HadError = true;
        }
    }

}
