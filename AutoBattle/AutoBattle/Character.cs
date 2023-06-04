using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static AutoBattle.Types;

namespace AutoBattle
{
    public class Character
    {


        private string _name;
        private float _baseDamage;
        private float _damageMultiplier;
        private int _attackRange;
        private Character _target;
        private Vector2Int[] _attackablePositions;
        private Grid _grid;

        public Character(CharacterClass characterClass, int id, ColorScheme color)
        {
            Health = 100f;
            _baseDamage = 20f;
            PlayerIndex = id;
            _name = $"{id}. {characterClass}";
            _damageMultiplier = 1f;
            _attackRange = 1;
            _attackablePositions = CacheTargetablePositions(_attackRange);
            _grid = null;
            Color = (ConsoleColor)color;
        }

        public float Health { get; private set; }
        public int PlayerIndex { get; private set; }
        public Vector2Int CurrentPosition { get; private set; }
        public ConsoleColor Color { get; private set; }

        //caching so that we only calculate when needed
        private Vector2Int[] CacheTargetablePositions(int range)
        {
            List<Vector2Int> targetablePositionsList = new List<Vector2Int>();
            //adding x and y axis for each range depth
            for(int i = 1; i <= range; i++)
            {
                targetablePositionsList.AddRange(new List<Vector2Int>() { new Vector2Int(-i, 0), new Vector2Int(i, 0),
                    new Vector2Int(0,-i),new Vector2Int(0,i)});
            }
            return targetablePositionsList.ToArray();
        }
        public void PlaceOnGrid(Grid grid, Vector2Int position)
        {
            if(grid.TrySetCharacter(position, this))
            {
                _grid = grid;
                CurrentPosition = position;
            }
        }

        public bool TakeDamage(float amount)
        {
            if((Health -= _baseDamage) <= 0)
            {
                Die();
                return true;
            }
            return false;
        }
        private int GetDamage()
        {
            return (int)(_baseDamage * _damageMultiplier);
        }
        public void Die()
        {
            //TODO >> maybe kill him?
        }

        public void WalkTO(bool CanWalk)
        {

        }

        public void StartTurn()
        {
            _target = CheckCloseTargets(_grid);
            if(_target != null)
            {
                Attack(_target);


                return;
            } else
            {
                MoveTowards(FindNearestTargetPosition(_grid), _grid);
            }
        }

        // Check in x and y directions if there is any character close enough to be a target.
        private Character CheckCloseTargets(Grid battlefield)
        {
            //check if current target is within range
            if(_target != null && _attackRange >= Vector2Int.Distance(_target.CurrentPosition, CurrentPosition))
            {
                return _target;
            }

            foreach(Vector2Int position in _attackablePositions)
            {
                Character target = battlefield.GetCellCharacter(CurrentPosition.x + position.x, CurrentPosition.y + position.y);
                if(target != null)
                {
                    return target;
                }
            }

            return null;
        }
        private Vector2Int FindNearestTargetPosition(Grid battlefield)
        {
            Vector2Int targetPosition = CurrentPosition;
            int distance = Vector2Int.Distance(new Vector2Int(0, 0), new Vector2Int(battlefield.XLenght, battlefield.YLength));//max possible distance
            foreach(Character character in Program.AllPlayers)
            {
                if(character == this)
                {
                    continue;
                }
                int d = Vector2Int.Distance(CurrentPosition, character.CurrentPosition);
                if(d < distance)
                {
                    distance = d;
                    targetPosition = character.CurrentPosition;
                }
            }

            return targetPosition;
        }
        private void MoveTowards(Vector2Int targetPosition, Grid battleField)
        {
            if(targetPosition == CurrentPosition)
            {
                return;
            }

            //Vector2Int candidatePosition = Vector2Int.MoveTowards(CurrentBox.position, targetPosition, 1);//TODO: create & implement _movementRange
            Vector2Int candidatePosition = battleField.MoveTowards(CurrentPosition, targetPosition, 1);//TODO: create & implement _movementRange
            //battleField.MoveTowards(CurrentBox.position, targetPosition, 1).occupied = null;
            if(candidatePosition != CurrentPosition)
            {
                Console.Write($"{_name} moved from [{CurrentPosition.x},{CurrentPosition.y}] to [{candidatePosition.x},{candidatePosition.y}]");
                //battleField.TryMoveCharacter(CurrentPosition, null);
                //battleField.TryMoveCharacter(candidatePosition, this);

                if(battleField.TryMoveCharacter(CurrentPosition,candidatePosition))
                {
                    CurrentPosition = candidatePosition;
                    battleField.OnBattlefieldChanged();
                }
            } else
            {
                //did not move
            }
        }
        public void Attack(Character target)
        {
            var rand = new Random();
            target.TakeDamage(rand.Next(0, GetDamage()));
            Console.WriteLine($"Player {PlayerIndex} is attacking the player {_target.PlayerIndex} and did {_baseDamage} damage\n");
        }

        public enum ColorScheme
        {
            Player = (int)ConsoleColor.Green,
            Enemy = (int)ConsoleColor.Red,
            Ally = (int)ConsoleColor.Cyan
        }
    }
}
