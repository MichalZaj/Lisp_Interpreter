# Lisp_Interpreter
Lisp Interpreter written in C#
# What is Working/Not
- I have TokenTypes of Lisp. ex. LEFT_PAREN, DEFINE, STRING, NUMBER
- Scanner works 
- Parser works.
- Interpreter file to evaluate expressions.
- I was able to get a printout of the AST Tree.
- Can do basic arithmetic operations like: `(* (+ 1 2) (* 4 1))`
- Atom and List are both implemented in the Expr.cs file.
- The code can be built and ran. But it lacks several commands.
- Pitfalls: CONS, COND, CAR, CDR, AND?, OR?, NOT?, Equality (Don't exist/work)
# Install/Build
- Ensure that you have the .NET SDK installed on your machine. You can download it from the official [.NET website](https://dotnet.microsoft.com/en-us/download).
- Install Visual Studio on the [official Visual Studio website](https://visualstudio.microsoft.com/).
- In Visual Studio, click on the "Get Started" tab and go to "Open a project or solution." Find the "Lisp_Interpeter" folder that is in this repository.
- In the "Build" tab, click "clean solution" and then "build solution".
# Usage 
- Complete Install/Build.
- Run the interpreter by selecting the "Start" icon.
- After clicking start, a terminal window will showup where you can code in Lisp.
# Testing
- The test outputs are in the Lisp_Tests.txt file.
- Scanner was Tested.
- Parser was Tested.
- AST Tree printout is provided with the outputs.
- Arithmetic expressions tested.
