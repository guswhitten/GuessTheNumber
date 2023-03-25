using System;
using System.Drawing;
using System.Runtime.InteropServices;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace NumberGuesser {
    class Program {
        static void Main(string[] args) {

            // Mongo collection details
            var client = new MongoClient("mongodb://127.0.0.1:27017");
            var db = client.GetDatabase("number_guesser_db");
            var collection = db.GetCollection<PlayerModel>("players");

            // print introductory lines to console
            WriteLineInColor("Number Guesser v1.0.1 by Whitten Oswald", ConsoleColor.Yellow);
            Console.Write("\nWhat is your name?  ");

            // check whether this player has played before
            string name = Console.ReadLine();
            BsonDocument player = FindAPlayer(collection, name);
            if (player == null)
            {
                collection.InsertOne(new PlayerModel { Name = name, Correct = 0, Attempts = 0, Accuracy = 0.0 });
                player = FindAPlayer(collection, name);
                Console.WriteLine($"Nice to meet you {name}, I want to play a game...");
            }
            else
            {
                Console.WriteLine($"\nWelcome back {name}!\n\t\t\tSTATS:");
                PrintStats(player);
            }

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
                        WriteLineInColor("Please enter a digit.\n", ConsoleColor.DarkRed);
                        continue;
                    }

                    // check whether guess is correct
                    guess = Int32.Parse(input);
                    if (guess != correctNumber) { WriteLineInColor("Wrong number. Guess again\n", ConsoleColor.DarkRed); }
                }
                // loop ends when player has guessed correctly
                correct++;
                WriteLineInColor("\nCORRECT!! ", ConsoleColor.Yellow);
                Console.Write("Play again? [Y / N] ");
                if (Console.ReadLine().ToUpper() == "Y") { continue; }
                else {
                    WriteLineInColor("\nGAME OVER\n", ConsoleColor.Red);
                    // calculate new stats
                    double session_accuracy = ((double)correct / attempts) * 100;
                    correct += player["Correct"].ToInt32(); attempts += player["Attempts"].ToInt32();
                    double new_accuracy = ((double)correct / attempts) * 100;

                    // update player's stats in mongo
                    var filter = Builders<PlayerModel>.Filter.Eq("Name", name);
                    var update = Builders<PlayerModel>.Update.Set("Correct", correct).Set("Attempts", attempts).Set("Accuracy", new_accuracy);
                    collection.FindOneAndUpdate(filter, update);

                    PrintAccuracyResults(player["Accuracy"].ToDouble(), session_accuracy, new_accuracy);
                    return;
                }
            }
        }

        static void WriteLineInColor(string line, ConsoleColor color) {
            Console.ForegroundColor = color;
            Console.Write(line);
            Console.ResetColor();
        }

        private static BsonDocument FindAPlayer(IMongoCollection<PlayerModel> collection, string name)
        {
            var mongo_query = Builders<PlayerModel>.Filter.Eq(pm => pm.Name, name);
            var player = collection.Find(mongo_query).FirstOrDefault().ToBsonDocument();

            return player;
        }

        private static void PrintStats(BsonDocument player)
        {
            Console.WriteLine($"\tCorrect\t\tTotal\t\tAccuracy\n\t  {player["Correct"]} \t\t {player["Attempts"]} \t\t  {player["Accuracy"].ToDouble().ToString("0.#")}%\n");
        }

        private static void PrintAccuracyResults(double old, double session, double curr)
        {
            Console.Write($"\tSession\t\tAccuracy\t +/-\n\t{session.ToString("0.##")}%\t\t{curr.ToString("0.##")}%\t\t");
            Console.ForegroundColor = curr > old ? ConsoleColor.Green : ConsoleColor.Red;
            Console.Write($"{(curr - old).ToString("0.##")}%\n");
            Console.ResetColor();
        }
    }
}


