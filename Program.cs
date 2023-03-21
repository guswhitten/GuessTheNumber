using System;

namespace NumberGuesser
{
    class Program
    {
        static void Main(string[] args)
        {
            // print introductory lines to console
            WriteLineInColor("Number Guesser v1.0.0 by Whitten Oswald", ConsoleColor.Yellow);
            Console.Write("What is your name? : ");
            Console.WriteLine("Hello {0}, let's play a game...", Console.ReadLine());

            // initialize variables that will be consistent thru all rounds
            int correct = 0, attempts = 0;

            while (true) {
                // initialize variables that will reset after each round
                Random random = new Random();
                int correctNumber = random.Next(1, 10), guess = 0;
                Console.WriteLine("Guess a number between 1 and 10");

                while (guess != correctNumber) {
                    attempts++;
                    string input = Console.ReadLine();

                    // error checking: ensure user's input is numerical
                    if (!int.TryParse(input, out guess)) {
                        WriteLineInColor("Please enter a digit.", ConsoleColor.Red);
                        continue;
                    }

                    // check whether guess is correct
                    guess = Int32.Parse(input);
                    if (guess != correctNumber) {
                        WriteLineInColor("Wrong number. Guess again", ConsoleColor.Red);
                    }
                }
                correct++;
                WriteLineInColor("CORRECT!!", ConsoleColor.DarkYellow);
                Console.WriteLine("Play again? [Y / N]");
                string answer = Console.ReadLine().ToUpper();
                if (answer == "Y") {
                    continue;
                }
                else {
                    // calculate and print accuracy over all games played
                    WriteLineInColor("Game over", ConsoleColor.Red);
                    double percent = (double)correct / attempts;
                    Console.WriteLine("Your Accuracy ==> {0} / {1} = {2}%", correct, attempts, percent);
                    return;
                }
            }
        }

        static void WriteLineInColor(string line, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(line);
            Console.ResetColor();
        }
    }
}


