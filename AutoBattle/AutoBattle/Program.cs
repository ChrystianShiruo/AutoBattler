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
        public static List<Character> AllPlayers { get; private set; }
        static void Main(string[] args)
        {
            Grid grid;
            //CharacterClass playerCharacterClass;
            //GridBox PlayerCurrentLocation;
            //GridBox EnemyCurrentLocation;
            Character playerCharacter;
            Character enemyCharacter;
            int currentTurn;
            int numberOfPossibleTiles;
            KeyValuePair<string, uint>[] characterClasses;
            Setup();


            void Setup()
            {
                grid = GetGrid();
                numberOfPossibleTiles = grid.grids.Count;
                AllPlayers = new List<Character>();
                currentTurn = 0;
                //caching Character classes for easy access
                characterClasses = Types.GetCharacterClasses();
                CreatePlayerCharacter(GetPlayerChoice());
                CreateEnemyCharacter();

                StartGame();
            }





            CharacterClass GetPlayerChoice()
            {
                //asks for the player to choose between for possible classes via console.
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



            void CreatePlayerCharacter(CharacterClass characterClass)
            {

                //CharacterClass characterClass = (CharacterClass)classIndex;
                Console.WriteLine($"Player Class Choice: {characterClass}");
                playerCharacter = new Character(characterClass, 0, ColorScheme.Player);
                AllPlayers.Add(playerCharacter);
                //CreateEnemyCharacter();

            }

            void CreateEnemyCharacter()
            {
                //randomly choose the enemy class and set up vital variables
                var rand = new Random();
                int randomInteger = rand.Next(1, characterClasses.Length);
                CharacterClass enemyClass = (CharacterClass)characterClasses[randomInteger].Value;
                Console.WriteLine($"Enemy Class Choice: {enemyClass}");
                enemyCharacter = new Character(enemyClass, 1, ColorScheme.Enemy);
                AllPlayers.Add(enemyCharacter);

                //StartGame();

            }

            void StartGame()
            {


                AlocatePlayers();
                StartTurn();

            }

            void StartTurn()
            {

                if(currentTurn == 0)
                {
                    //AllPlayers.Sort();  
                }
                Console.WriteLine($"\nStarting turn {currentTurn}...");

                foreach(Character character in AllPlayers)
                {
                    character.StartTurn();
                }

                currentTurn++;

                HandleTurn();
            }

            void HandleTurn()
            {
                if(playerCharacter.Health == 0)
                {

                    return;
                } else if(enemyCharacter.Health == 0)
                {
                    Console.Write(Environment.NewLine + Environment.NewLine);

                    // endgame?

                    Console.Write(Environment.NewLine + Environment.NewLine);

                    return;
                } else
                {
                    Console.Write(Environment.NewLine + Environment.NewLine);
                    Console.WriteLine("Click on any key to start the next turn...\n");
                    Console.Write(Environment.NewLine + Environment.NewLine);

                    ConsoleKeyInfo key = Console.ReadKey();
                    StartTurn();
                }
            }

            int GetRandomInt(int min, int max)
            {
                var rand = new Random();
                int index = rand.Next(min, max);
                return index;
            }

            void AlocatePlayers()
            {
                AlocatePlayerCharacter();

            }

            void AlocatePlayerCharacter()
            {
                int random = 0;
                GridCell RandomLocation = (grid.grids.ElementAt(random));
                Console.Write($"{random}\n");
                if(RandomLocation.occupied == null)
                {
                    GridCell PlayerCurrentLocation = RandomLocation;
                    RandomLocation.occupied = playerCharacter;
                    grid.grids[random] = RandomLocation;
                    playerCharacter.PlaceOnGrid(grid, grid.grids[random].position);
                    AlocateEnemyCharacter();
                } else
                {
                    AlocatePlayerCharacter();
                }
            }

            void AlocateEnemyCharacter()
            {
                int random = 24;//TODO Random between 0 and grid size
                GridCell RandomLocation = (grid.grids.ElementAt(random));
                Console.Write($"{random}\n");
                if(RandomLocation.occupied == null)
                {
                    //EnemyCurrentLocation = RandomLocation;
                    RandomLocation.occupied = enemyCharacter;
                    grid.grids[random] = RandomLocation;
                    enemyCharacter.PlaceOnGrid(grid, grid.grids[random].position);
                    grid.DrawBattlefield();
                } else
                {
                    AlocateEnemyCharacter();
                }


            }

        }
    }
}
