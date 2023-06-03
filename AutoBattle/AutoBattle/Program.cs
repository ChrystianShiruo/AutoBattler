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
        static void Main(string[] args)
        {
            Grid grid;
            //CharacterClass playerCharacterClass;
            //GridBox PlayerCurrentLocation;
            //GridBox EnemyCurrentLocation;
            Character playerCharacter;
            Character enemyCharacter;
            List<Character> allPlayers;
            int currentTurn;
            int numberOfPossibleTiles;
            Setup(); 


            void Setup()
            {
                grid = GetGrid();
                numberOfPossibleTiles = grid.grids.Count;
                allPlayers = new List<Character>();
                currentTurn = 0;

                GetPlayerChoice();
            }

            void GetPlayerChoice()
            {
                //asks for the player to choose between for possible classes via console.
                Console.WriteLine("Choose Between One of this Classes:\n");

                StringBuilder characterClassOptions = new StringBuilder();

                string[] characterClassNames = Enum.GetNames(typeof(CharacterClass));
                foreach(string name in characterClassNames)
                {
                    string classNumber = ((uint)Enum.Parse(typeof(CharacterClass),name)).ToString();
                    characterClassOptions.Append($"[{classNumber}] {name}, ");
                }
                //removing ", " at the end
                characterClassOptions.Remove(characterClassOptions.Length-2, 2);
                Console.WriteLine(characterClassOptions);

                //store the player choice in a variable
                string choice = Console.ReadLine();

                UInt32 choiceInt = 0;
                bool isValidValue = UInt32.TryParse(choice, out choiceInt) && Enum.IsDefined(typeof(CharacterClass), choiceInt);

                if (isValidValue)
                {
                    CreatePlayerCharacter((CharacterClass)UInt32.Parse(choice));
                }
                else
                {
                    Console.WriteLine($"Undefined class for value: {choice}");
                    GetPlayerChoice();
                }

            }

            Grid GetGrid()
            {
                //TODO: Implement variable grid size
                return new Grid(5, 5);
            }
            void CreatePlayerCharacter(CharacterClass characterClass)
            {
               
                //CharacterClass characterClass = (CharacterClass)classIndex;
                Console.WriteLine($"Player Class Choice: {characterClass}");
                playerCharacter = new Character(characterClass);
                playerCharacter.Health = 100;
                playerCharacter.BaseDamage = 20;
                playerCharacter.PlayerIndex = 0;
                
                CreateEnemyCharacter();

            }

            void CreateEnemyCharacter()
            {
                //randomly choose the enemy class and set up vital variables
                var rand = new Random();
                int randomInteger = rand.Next(1, 4);
                CharacterClass enemyClass = (CharacterClass)randomInteger;
                Console.WriteLine($"Enemy Class Choice: {enemyClass}");
                enemyCharacter = new Character(enemyClass);
                enemyCharacter.Health = 100;
                playerCharacter.BaseDamage = 20;
                playerCharacter.PlayerIndex = 1;
                StartGame();

            }

            void StartGame()
            {
                //populates the character variables and targets
                enemyCharacter.Target = playerCharacter;
                playerCharacter.Target = enemyCharacter;
                allPlayers.Add(playerCharacter);
                allPlayers.Add(enemyCharacter);
                AlocatePlayers();
                StartTurn();

            }

            void StartTurn(){

                if (currentTurn == 0)
                {
                    //AllPlayers.Sort();  
                }

                foreach(Character character in allPlayers)
                {
                    character.StartTurn(grid);
                }

                currentTurn++;
                HandleTurn();
            }

            void HandleTurn()
            {
                if(playerCharacter.Health == 0)
                {
                    return;
                } else if (enemyCharacter.Health == 0)
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
                GridBox RandomLocation = (grid.grids.ElementAt(random));
                Console.Write($"{random}\n");
                if (!RandomLocation.ocupied)
                {
                    GridBox PlayerCurrentLocation = RandomLocation;
                    RandomLocation.ocupied = true;
                    grid.grids[random] = RandomLocation;
                    playerCharacter.currentBox = grid.grids[random];
                    AlocateEnemyCharacter();
                } else
                {
                    AlocatePlayerCharacter();
                }
            }

            void AlocateEnemyCharacter()
            {
                int random = 24;
                GridBox RandomLocation = (grid.grids.ElementAt(random));
                Console.Write($"{random}\n");
                if (!RandomLocation.ocupied)
                {
                    //EnemyCurrentLocation = RandomLocation;
                    RandomLocation.ocupied = true;
                    grid.grids[random] = RandomLocation;
                    enemyCharacter.currentBox = grid.grids[random];
                    grid.drawBattlefield(5 , 5);
                }
                else
                {
                    AlocateEnemyCharacter();
                }

                
            }

        }
    }
}
