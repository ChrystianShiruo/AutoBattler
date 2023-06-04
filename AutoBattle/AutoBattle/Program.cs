using System;
using System.Text;
using static AutoBattle.Character;
using static AutoBattle.Grid;
using System.Collections.Generic;
using System.Linq;
using static AutoBattle.Types;

namespace AutoBattle
{
    class Program
    {
        public static List<Character> AliveCharacters { get; private set; }
        static void Main(string[] args)
        {
            Grid grid;

            int currentTurn;
            int numberOfPossibleTiles;
            int totalCharacterCount;
            KeyValuePair<string, uint>[] characterClasses;
            Setup();

            void Setup()
            {
                totalCharacterCount = 2;
                grid = GetGrid();
                numberOfPossibleTiles = grid.XLenght * grid.YLength;
                AliveCharacters = new List<Character>();
                currentTurn = 0;
                //caching Character classes for easy access
                characterClasses = Types.GetCharacterClasses();
                CreateCharacters();

                StartGame();
            }

            CharacterClass GetPlayerChoice()
            {
                Console.WriteLine("Choose Between One of this Classes:\n");

                StringBuilder characterClassOptions = new StringBuilder();

                foreach(KeyValuePair<string, uint> characterClass in characterClasses)
                {
                    //string classNumber = ((uint)Enum.Parse(typeof(CharacterClass), characterClass.Key)).ToString();
                    characterClassOptions.Append($"[{characterClass.Value}] {characterClass.Key}, ");
                }
                //removing ", " at the end
                characterClassOptions.Remove(characterClassOptions.Length - 2, 2);
                Console.WriteLine(characterClassOptions);

                //store the player choice in a variable
                string choice = Console.ReadLine();

                UInt32 choiceInt = 0;
                bool isValidValue = UInt32.TryParse(choice, out choiceInt) && Enum.IsDefined(typeof(CharacterClass), choiceInt);

                if(isValidValue)
                {
                    //CreatePlayerCharacter((CharacterClass)UInt32.Parse(choice));                   
                    return (CharacterClass)UInt32.Parse(choice);
                } else
                {
                    Console.WriteLine($"Undefined class for value: {choice}");
                    return GetPlayerChoice();

                }

            }

            Grid GetGrid()
            {
                uint x = 0;
                uint y = 0;
                Console.WriteLine("Battlefield builder\n");
                GetUint("Select Line count:", out x);
                GetUint("Select Collumn count:", out y);

                if(x * y < totalCharacterCount)
                {
                    Console.WriteLine($"battlefield is too small for {totalCharacterCount} characters.\n");
                    return GetGrid();
                }

                return new Grid((int)x, (int)y);

                void GetUint(string requestText, out uint i)
                {
                    while(true)
                    {
                        Console.WriteLine(requestText);
                        string xInput = Console.ReadLine();
                        if(uint.TryParse(xInput, out i))
                            break;
                        else Console.WriteLine($"{xInput} is an invalid value, please input a positive integer");
                    }
                }
            }


            void CreateCharacters()
            {
                CreateCharacter(GetPlayerChoice(), ColorScheme.Player);
                CreateEnemyCharacter();
            }
            void CreateCharacter(CharacterClass characterClass, ColorScheme color)
            {
                Console.WriteLine($"Character {AliveCharacters.Count} Class Choice: {characterClass}");
                Character character = new Character(characterClass, AliveCharacters.Count, color);
                AliveCharacters.Add(character);
            }
            void CreateEnemyCharacter()
            {
                var rand = new Random();
                int randomInteger = rand.Next(1, characterClasses.Length);

                CreateCharacter((CharacterClass)characterClasses[randomInteger].Value, ColorScheme.Enemy);
            }

            void StartGame()
            {
                AlocatePlayers();
                grid.OnBattlefieldChanged();
                StartTurn();
            }

            void StartTurn()
            {
                Console.Write(Environment.NewLine + Environment.NewLine);
                Console.WriteLine("Click on any key to start the next turn...\n");
                Console.Write(Environment.NewLine + Environment.NewLine);

                ConsoleKeyInfo key = Console.ReadKey();

                currentTurn++;

                Console.WriteLine($"\nStarting turn {currentTurn}...");
                AliveCharacters.ForEach(character => character.StartTurn());


                HandleTurn();
            }

            void HandleTurn()
            {
                UpdateAliveCharacters();
                if(AliveCharacters.Count <= 1)
                {
                    string winnerMessage = string.Empty;
                    if(AliveCharacters.Count == 1)
                    {
                        winnerMessage = $"{AliveCharacters[0].Name}  with {AliveCharacters[0].Health} hp left!";
                        Console.ForegroundColor = AliveCharacters[0].Color;
                    } else
                    {
                        winnerMessage = "No one!?";
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.ForegroundColor = ConsoleColor.Red;

                    }
                    Console.Write(Environment.NewLine + Environment.NewLine);
                    Console.WriteLine($"BATTLE ENDED!!\nWINNER: {winnerMessage}");
                    Console.ResetColor();
                    Console.Write(Environment.NewLine + Environment.NewLine);
                    return;
                }
                Console.WriteLine($"Turn {currentTurn} summary: \n");
                foreach(Character character in AliveCharacters)
                {
                    Messages.ColoredWriteLine($"{character.Name} hp: { character.Health.ToString("F2")}", character.Color);
                }
               
                StartTurn();

            }
            void UpdateAliveCharacters()
            {
                List<Character> newCharacterList = new List<Character>(AliveCharacters);
                foreach(Character character in AliveCharacters)
                {
                    if(character.Health <= 0)
                    {
                        newCharacterList.Remove(character);
                    }
                }
                AliveCharacters = newCharacterList;
            }

            List<T> Shuffle<T>(List<T> list)
            {

                List<T> temp = new List<T>();

                Random rand = new Random();
                for(int i = list.Count; i > 0; i--)
                {
                    int randomValue = rand.Next(0, i);
                    temp.Add(list[randomValue]);
                    list.RemoveAt(randomValue);
                }

                return temp;
            }

            void AlocatePlayers()
            {
                Console.WriteLine("Shuffling character order...\n");
                //Randomizing characters order
                AliveCharacters = Shuffle(AliveCharacters);
                AliveCharacters.ForEach(character => AllocatePlayers(character));
            }
            void AllocatePlayers(Character character)
            {
                int randomX = new Random().Next(0, grid.XLenght);
                int randomY = new Random().Next(0, grid.YLength);


                if(grid.GetCellCharacter(randomX, randomY) == null)
                {
                    Messages.ColoredWriteLine($"Allocating {character.Name} to position [{randomX},{randomY}]\n", character.Color);
                    //Console.Write($"Allocating {character.Name} to position [{randomX},{randomY}]\n");
                    character.PlaceOnGrid(grid, new Vector2Int(randomX, randomY));

                } else
                {
                    AllocatePlayers(character);
                }
            }

        }
    }
}
