using System;
using System.Text;
using static AutoBattle.Character;
using System.Collections.Generic;
using System.Linq;

namespace AutoBattle
{
    class Program
    {
        private static List<Character> CharacterOrder { get; set; }

        //probably more efficient to have both list than to iterate through CharacterOrder all the time we need Teams 
        public static List<Character>[] TeamCharacters { get; private set; }

        static void Main(string[] args)
        {
            Grid grid;

            int currentTurn;
            int teamSizeLimit;
            CharacterClassInfo[] characterClasses;
            int totalCharacterCount = 0;
            Setup();

            void Setup()
            {
                TeamCharacters = new List<Character>[2] { new List<Character>(), new List<Character>() };
                teamSizeLimit = 5;
                //caching Character classes for easy access
                characterClasses = Data.CharacterClassInfoData.SetupClassesInfo();
                CreateCharacters();




                grid = GetGrid();

                currentTurn = 0;
                StartGame();
            }

            CharacterClassInfo GetPlayerChoice()
            {
                Console.WriteLine("Choose Between One of this Classes:\n");

                StringBuilder characterClassOptions = new StringBuilder();

                for(int i = 0; i < characterClasses.Length; i++)
                {
                    characterClassOptions.Append($"[{(int)characterClasses[i].characterClass}] {characterClasses[i].characterClass}, ");
                }
                //removing ", " at the end
                characterClassOptions.Remove(characterClassOptions.Length - 2, 2);
                Console.WriteLine(characterClassOptions);

                string choice = Console.ReadLine();

                UInt32 choiceInt = 0;
                bool isValidInt = UInt32.TryParse(choice, out choiceInt);
                CharacterClassInfo selectedClass = characterClasses.First<CharacterClassInfo>(_ => (int)_.characterClass == choiceInt);

                if(isValidInt && selectedClass != null)
                {
                    return selectedClass;
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

            }



            void CreateCharacters()
            {
                uint teamSize;
                GetUint($"Select team size (must be between 1 and {teamSizeLimit}):", out teamSize);

                if(teamSize < 1 || teamSize > teamSizeLimit)
                {
                    Console.WriteLine($"Team size cannot be {teamSize}, select a value between 1 and {teamSizeLimit}");
                    CreateCharacters();
                }

                int playerTeam = 0;
                CreateCharacter(GetPlayerChoice(), ColorScheme.Player, playerTeam);

                var rand = new Random();
                for(int teamsI = 0; teamsI < TeamCharacters.Length; teamsI++)
                {
                    while(TeamCharacters[teamsI].Count < teamSize)
                    {
                        int randomInteger = rand.Next(1, characterClasses.Length);
                        CreateCharacter(characterClasses[randomInteger], teamsI == playerTeam ? ColorScheme.Ally : ColorScheme.Enemy, teamsI);
                    }
                }

            }
            void CreateCharacter(CharacterClassInfo characterClassInfo, ColorScheme color, int teamId)
            {
                Character character = new Character(characterClassInfo, totalCharacterCount, color, teamId);
                Messages.ColoredWriteLine($"Created {character.Name} for Team {character.Team} || hp:{character.Health} || MaxDamage:{character.MaxDamage} || Range:{character.AttackRange}", character.Color);


                TeamCharacters[teamId].Add(character);
                totalCharacterCount++;
            }
            void CreateEnemyCharacter()
            {
                var rand = new Random();
                int randomInteger = rand.Next(1, characterClasses.Length);

                CreateCharacter(characterClasses[randomInteger], ColorScheme.Enemy, 1);
            }

            void StartGame()
            {
                OrderPlayers();
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
                CharacterOrder.ForEach(character => character.StartTurn());


                HandleTurn();
            }

            void HandleTurn()
            {
                UpdateAliveCharacters();
                List<int> teamsLeft = new List<int>();
                for(int i = 0; i < TeamCharacters.Length; i++)
                {
                    if(TeamCharacters[i].Count > 0)
                    {
                        teamsLeft.Add(i);
                    }
                }

                if(teamsLeft.Count <= 1)
                {
                    string winnerMessage = string.Empty;
                    if(teamsLeft.Count == 1)
                    {
                        winnerMessage = $"TEAM {teamsLeft[0]}!";
                        Console.ForegroundColor = TeamCharacters[teamsLeft[0]][0].Color;
                        //winnerMessage = $"{TeamCharacters[0].Name}  with {TeamCharacters[0].Health} hp left!";
                        //Console.ForegroundColor = TeamCharacters[0].Color;
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
                foreach(List<Character> team in TeamCharacters)
                {
                    foreach(Character character in team)
                    {
                        Messages.ColoredWriteLine($"{character.Name} hp: { character.Health.ToString("F2")}", character.Color);
                    }
                }
                StartTurn();

            }
            void UpdateAliveCharacters()
            {
                List<Character> newCharacterList = new List<Character>(CharacterOrder);
                foreach(Character character in CharacterOrder)
                {
                    if(character.Health <= 0)
                    {
                        newCharacterList.Remove(character);
                        TeamCharacters[character.Team].Remove(character);
                    }
                }
                CharacterOrder = newCharacterList;
            }

            void OrderPlayers()
            {
                List<Character> allCharacters = new List<Character>();
                foreach(List<Character> cl in TeamCharacters)
                {
                    allCharacters.AddRange(cl);
                };
                CharacterOrder = allCharacters;

                Console.WriteLine("Shuffling character order...\n");
                //Randomizing characters order
                CharacterOrder = Shuffle(allCharacters);


                CharacterOrder.ForEach(character => AllocatePlayers(character));
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

            #region Utils
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
            #endregion


        }
    }
}
