using System;
using System.Text.RegularExpressions;

namespace StringCalculationCsharpImplementation
{
    public static class StringCalculationCsharp
    {
        public static int Answer(string prompt)
        {
            // Remove unnecessary words and spaces from the prompt
            prompt = prompt.Replace("What is ", "").TrimEnd('?');

            // Check for unsupported questions
            if (!Regex.IsMatch(prompt, @"^-?\d+(\s((plus|minus|multiplied by|divided by)\s(-)?\d+))*$"))
            {
                throw new ArgumentException("Unsupported question.");
            }

            // Split the prompt into tokens
            string[] tokens = prompt.Split(' ');

            // Perform the calculations
            int result = 0;
            int currentNumber = 0;
            string currentOperator = "+";

            for (int i = 0; i < tokens.Length; i++)
            {
                string token = tokens[i];

                if (int.TryParse(token, out int number))
                {
                    // Token is a number
                    currentNumber = number;

                    // Apply the previous operator to the result
                    result = ApplyOperator(result, currentNumber, currentOperator);
                }
                else
                {
                    // Token is an operator
                    currentOperator = GetOperator(token, tokens, i);

                    if (currentOperator == "*" || currentOperator == "/")
                        i++;
                }
            }

            return result;
        }

        private static int ApplyOperator(int a, int b, string op)
        {
            switch (op)
            {
                case "+":
                    return a + b;
                case "-":
                    return a - b;
                case "*":
                    return a * b;
                case "/":
                    return a / b;
                default:
                    throw new ArgumentException("Unsupported operation.");
            }
        }

        private static string GetOperator(string op, string[] tokens, int currentIndex)
        {
            switch (op)
            {
                case "plus":
                    return "+";
                case "minus":
                    return "-";
                case "multiplied":
                    if (currentIndex + 1 < tokens.Length && tokens[currentIndex + 1] == "by")
                    {
                        return "*";
                    }
                    throw new ArgumentException("Unsupported operation.");
                case "divided":
                    if (currentIndex + 1 < tokens.Length && tokens[currentIndex + 1] == "by")
                    {
                        return "/";
                    }
                    throw new ArgumentException("Unsupported operation.");
                default:
                    throw new ArgumentException("Unsupported operation.");
            }
        }
    }

    public class StringCalculationCsharpTests
    {
        public static void Main(string[] args)
        {
            RunTests();

            // Wait for user input before exiting
            Console.ReadLine();
        }

        public static void RunTests()
        {
            try
            {
                // Scenario 1: Calculate from a String
                Test("What is 5?", 5);
                Test("What is 1 plus 1?", 2);
                Test("What is 53 plus 2?", 55);
                Test("What is 4 minus -12?", 16);
                Test("What is -3 multiplied by 25?", -75);
                Test("What is 33 divided by -3?", -11);

                // Scenario 2: Multiple Calculations
                Test("What is 1 plus 1 plus 1?", 3);
                Test("What is 1 plus 5 minus -2?", 8);
                Test("What is -3 plus 7 multiplied by -2?", -8);
                Test("What is 2 multiplied by -12 divided by -3?", 8);

                // Scenario 3: Exception Handling
                TestThrows<ArgumentException>("What is 52 cubed?");
                TestThrows<ArgumentException>("What is Love?");
                TestThrows<ArgumentException>("What is the answer to the ultimate question of life, the universe, and everything?");
                TestThrows<ArgumentException>("What is 1 plus?");

                Console.WriteLine("All tests passed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
            }
        }

        private static void Test(string prompt, int expected)
        {
            try
            {
                int result = StringCalculationCsharp.Answer(prompt);
                Console.WriteLine($"Test passed: Prompt='{prompt}', Expected={expected}, Result={result}");
                if (result != expected)
                {
                    throw new Exception($"Assertion failed: Expected {expected}, but got {result}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed: Prompt='{prompt}', Expected={expected}, Exception={ex.Message}");
                throw;
            }
        }

        private static void TestThrows<TException>(string prompt) where TException : Exception
        {
            try
            {
                StringCalculationCsharp.Answer(prompt);
                Console.WriteLine($"Test failed: Prompt='{prompt}', Expected={typeof(TException).Name}, No exception was thrown.");
                throw new Exception($"Assertion failed: Expected {typeof(TException).Name} to be thrown, but no exception was thrown.");
            }
            catch (TException)
            {
                Console.WriteLine($"Test passed: Prompt='{prompt}', Expected={typeof(TException).Name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Test failed: Prompt='{prompt}', Expected={typeof(TException).Name}, Exception={ex.Message}");
                throw;
            }
        }
    }
}
