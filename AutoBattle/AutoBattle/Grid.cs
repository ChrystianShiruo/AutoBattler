using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static AutoBattle.Types;

namespace AutoBattle
{
    public class Grid
    {
        private GridCell[,] _grid2D;

        public Action OnBattlefieldChanged;// TODO: call when moving, dying, killing etc
        public Grid(int Lines, int Columns)// TODO: change to uint
        {
            OnBattlefieldChanged += DrawBattlefield;
            XLenght = Lines;
            YLength = Columns;
            grids = new List<GridCell>();
            _grid2D = new GridCell[XLenght, YLength];
            Console.WriteLine("Creating battle field\n");
            for(int i = 0; i < Lines; i++)
            {

                for(int j = 0; j < Columns; j++)
                {
                    GridCell newBox = new GridCell(i, j, null, (Columns * i + j));
                    grids.Add(newBox);
                    _grid2D[i, j] = newBox;
                    Console.Write($"{newBox.Index}\n");
                }
            }
            Console.WriteLine("The battle field has been created\n");
        }
        public List<GridCell> grids { get; private set; }
        public int XLenght { get; private set; }
        public int YLength { get; private set; }


        // prints the matrix that indicates the tiles of the battlefield
        public void DrawBattlefield()
        {
            Console.Write("\n");
            for(int i = 0; i < XLenght; i++)
            {
                for(int j = 0; j < YLength; j++)
                {
                    //GridCell currentGrid = _grid2D[i,j];
                    if(_grid2D[i, j].occupied != null)
                    {
                        //if()
                        Character character = _grid2D[i, j].occupied;
                        Console.ForegroundColor = character.Color;
                        Console.Write($"[{String.Format("{0:00}", character.PlayerIndex)},hp: {character.Health}]\t");
                        Console.ResetColor();
                    } else
                    {
                        Console.Write($"[{_grid2D[i, j].position.x},{_grid2D[i, j].position.y}]\t");
                    }
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
            Console.Write(Environment.NewLine + Environment.NewLine);
        }


        public bool TryMoveCharacter(Vector2Int current, Vector2Int target)
        {
            if(_grid2D[current.x, current.y].occupied != null)
            {
                _grid2D[target.x, target.y].occupied = _grid2D[current.x, current.y].occupied;
                _grid2D[current.x, current.y].occupied = null;
                return true;
            }

            return false;
        }
        public bool TrySetCharacter(Vector2Int position, Character character)
        {
            if(_grid2D[position.x, position.y].occupied == null)
            {
                SetCellCharacter(position, character);
                return true;
            }
            return false;
        }
        public void SetCellCharacter(Vector2Int vector2Int, Character character)
        {
            _grid2D[vector2Int.x, vector2Int.y].occupied = character;
        }
        public Character GetCellCharacter(Vector2Int vector2Int)
        {
            return GetCellCharacter(vector2Int.x, vector2Int.y);
        }
        public Character GetCellCharacter(int x, int y)
        {
            if(!IsWithinBounds(x, y))
            {
                return null;
            }

            return _grid2D[x, y].occupied;
        }

        public bool IsWithinBounds(Vector2Int vector2Int)
        {
            return IsWithinBounds(vector2Int.x, vector2Int.y);
        }
        public bool IsWithinBounds(int x, int y)
        {
            return (x >= 0 && y >= 0 && XLenght > x && YLength > y);
        }
        public Vector2Int MoveTowards(Vector2Int start, Vector2Int target, int coveredDistance)
        {


            Vector2Int relativePosition = target - start;
            Vector2Int newPosition = start;
            for(int i = 0; i < coveredDistance; i++)
            {
                if(newPosition == target)
                {
                    break;
                }

                if(newPosition.x != target.x)
                {
                    Vector2Int newCandidatePosition = new Vector2Int(newPosition.x + Math.Sign(target.x - newPosition.x), newPosition.y);
                    //Vector2Int newCandidatePosition = new Vector2Int(Math.Sign(newPosition.x - target.x ), newPosition.y);
                    if(GetCellCharacter(newCandidatePosition) == null)//not occupied
                    {
                        newPosition = newCandidatePosition;
                        continue;
                    }
                }
                if(newPosition.y != target.y)
                {
                    Vector2Int newCandidatePosition = new Vector2Int(newPosition.x, newPosition.y + Math.Sign(target.y - newPosition.y));
                    //Vector2Int newCandidatePosition = new Vector2Int(newPosition.x, Math.Sign(newPosition.y - target.y));
                    if(GetCellCharacter(newCandidatePosition) == null)//not occupied
                    {
                        newPosition = newCandidatePosition;
                        continue;
                    }
                }

                break;//could not fix way to get closer directly
            }

            return _grid2D[newPosition.x, newPosition.y].position;

        }
    }

}
