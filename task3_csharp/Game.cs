using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace task3_csharp
{
    class Game
    {
        private static Dictionary<Int32, String> choices;
        Int32 computerChoice;
        Int32 playerChoice;
        Byte[] hmacKey;
        Byte[] hmac;

        public Game(string[] args)
        {
            Parser parser = new Parser(args);
            try
            {
                choices = parser.parse();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void StartGame()
        {
            GenerateComputerChoiceAndHMAC();
            PrintHMAC();
            PrintMenu();
            GetPlayerChoice();
            ProcessPlayerInput();
            Console.WriteLine("HMAC key: " + ConvertToString(hmacKey));
        }

        private void GenerateComputerChoiceAndHMAC()
        {
            GenerateComputerChoice();
            String moveString = choices[computerChoice];
            ASCIIEncoding encoding = new ASCIIEncoding();
            Byte[] encodedText = encoding.GetBytes(moveString);
            HMACSHA256 hash = new HMACSHA256(hmacKey);
            hmac = hash.ComputeHash(encodedText);
        }

        private void GenerateComputerChoice()
        {
            RandomNumberGenerator generator = RandomNumberGenerator.Create();
            hmacKey = new byte[16];
            generator.GetBytes(hmacKey);
            computerChoice = ChooseRandomMove();
        }
        private int ChooseRandomMove()
        {
            Random random = new Random();
            return random.Next(1, choices.Count + 1);
        }


        private void PrintHMAC()
        {
            Console.WriteLine("HMAC:");
            Console.WriteLine(ConvertToString(hmac));
        }

        private string ConvertToString(Byte[] hmac)
        {
            return BitConverter.ToString(hmac).Replace("-","").ToLower();
        }


        private void PrintMenu()
        {
            Console.WriteLine("Available moves: ");
            foreach (var choice in choices)
            {
                Console.WriteLine(choice.Key + " - " + choice.Value);
            }
            Console.WriteLine("0 - EXIT");
        }

        private void GetPlayerChoice()
        {
            bool isParsed;
            do
            {
                Console.Write("Enter you move: ");
                String stringChoice = Console.ReadLine();
                isParsed = Int32.TryParse(stringChoice, out playerChoice);
                if (isParsed)
                {
                    if (playerChoice < 0 || playerChoice > choices.Count)
                    {
                        isParsed = false;
                    }
                }
                if (!isParsed)
                {
                    Console.WriteLine("Enter number in [0," + choices.Count + "].");
                    PrintMenu();
                }
            } while (!isParsed);
        }

        private void ProcessPlayerInput()
        {
            if (playerChoice == 0)
            {
                Console.WriteLine("GoodBye!");
                Console.Read();
                return;
            }
            Console.WriteLine("Your move: " + choices[playerChoice]);
            Console.WriteLine("Computer move: " + choices[computerChoice]);
            Result result = CheckResult(computerChoice, playerChoice);
            PrintResult(result);
            
        }

        private void PrintResult(Result result)
        {
            switch (result)
            {
                case Result.WIN:
                    Console.WriteLine("You win!");
                    break;
                case Result.LOSE:
                    Console.WriteLine("You lose!");
                    break;
                case Result.DRAW:
                    Console.WriteLine("You draw!");
                    break;
                default:
                    break;
            }
        }

        private static Result CheckResult(int computerChoice, int playerChoice)
        {
            Result result;
            Int32 winIndecesLength = (choices.Count - 1) / 2;
            Int32 choicesDelta = playerChoice - computerChoice;
            if (choicesDelta < 0)
            {
                if (Math.Abs(choicesDelta) > winIndecesLength)
                {
                    result = Result.WIN;
                }
                else
                {
                    result = Result.LOSE;
                }
            }
            else if (choicesDelta > 0)
            {
                if (choicesDelta > winIndecesLength)
                {
                    result = Result.LOSE;
                }
                else
                {
                    result = Result.WIN;
                }

            }
            else
            {
                result = Result.DRAW;
            }

            return result;
        }

    }
    enum Result
    {
        WIN, LOSE, DRAW
    }
}
