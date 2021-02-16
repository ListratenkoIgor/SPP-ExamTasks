using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Expressions
{
    public delegate bool PredicateOfInteger(int value);

    class Program
    {
        public static void CreateExpressionTreeFromCSharpLambda()
        {
            Expression<PredicateOfInteger> lambda = num => num < 5;

            PredicateOfInteger procedure = lambda.Compile();
            bool r = procedure(4);
        }

        public static void CreateExpressionTreeUsingAPI()
        {
            ParameterExpression numParam = Expression.Parameter(typeof(int), "num");
            ConstantExpression five = Expression.Constant(5, typeof(int));
            BinaryExpression numLessThanFive = Expression.LessThan(numParam, five);
            Expression<Func<int, bool>> lambda = Expression.Lambda<Func<int, bool>>(
                numLessThanFive, numParam); 

            Func<int, bool> procedure = lambda.Compile();
            bool r = procedure(4);
        }

        public static void CreateExpressionTreeFromCSharpLambda2()
        {
            Expression<Action<int>> lambda = (arg) => Console.WriteLine(arg);

            Action<int> procedure = lambda.Compile();
            procedure(4);
        }

        public static void CreateExpressionTreeUsingAPI2()
        {
            // Creating a parameter for the expression tree.
            ParameterExpression param = 
                Expression.Parameter(typeof(int), "arg");
            // Creating an expression for the method call and specifying its parameter.
            MethodCallExpression methodCall = 
                Expression.Call(
                    typeof(Console).GetMethod("WriteLine", new Type[] { typeof(int) }), param);
            // Compiling and invoking the methodCall expression.
            Expression<Action<int>> lambda = 
                Expression.Lambda<Action<int>>(methodCall, param);

            Action<int> procedure = lambda.Compile();
            procedure(4);
        }


        static int CSharpFactorial(int value)
        {
            int result = 1;
            while (value > 1)
            {
                result *= value--;
            }
            return result;
        }

        public static Func<int, int> CreateFactorialProcedure()
        {
            // Creating a parameter expression.
            ParameterExpression value = 
                Expression.Parameter(typeof(int), "value");

            // Creating an expression to hold a local variable. 
            ParameterExpression result = 
                Expression.Parameter(typeof(int), "result");

            // Creating a label to jump to from a loop.
            LabelTarget label = Expression.Label(typeof(int));

            // Creating a method body.
            BlockExpression block = Expression.Block(
                // Adding a local variable. 
                new[] { result },
                // Assigning a constant to a local variable: result = 1
                Expression.Assign(result, Expression.Constant(1)),
                    // Adding a loop.
                    Expression.Loop(
                        // Adding a conditional block into the loop.
                        Expression.IfThenElse(
                            // Condition: value > 1
                            Expression.GreaterThan(value, 
                                Expression.Constant(1)),
                            // If true: result *= value--
                            Expression.MultiplyAssign(result,
                                Expression.PostDecrementAssign(value)),
                            // If false, exit the loop and go to the label.
                            Expression.Break(label, result)
                       ),
                    // Label to jump to.
                    label)
            );

            // Compile and execute an expression tree. 
            Func<int, int> procedure = 
                Expression.Lambda<Func<int, int>>(block, value).Compile();
            return procedure;
        }

        static void Main(string[] args)
        {
            Func<int, int> factorial = CreateFactorialProcedure();
            Console.WriteLine("Факториал числа 6 = {0}", factorial(6));
            Console.ReadLine();
        }
    }
}
