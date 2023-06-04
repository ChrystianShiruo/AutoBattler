using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static AutoBattle.Types;

namespace AutoBattle
{
    public class Character
    {

        public Vector2Int CurrentPosition;

        private string _name;
        private float _baseDamage;
        private float _damageMultiplier;
        private int _attackRange;
        private Character _target;
        private Vector2Int[] _attackablePositions;

        public Character(CharacterClass characterClass, int id)
        {
            Health = 100f;
            _baseDamage = 20f;
            PlayerIndex = id;
            _name = $"{id}. {characterClass}";
            _damageMultiplier = 1f;
            _attackRange = 1;
            _attackablePositions = CacheTargetablePositions(_attackRange);
        }

        public float Health { get; private set; }
        public int PlayerIndex { get; private set; }

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

        public void StartTurn(Grid battlefield)
        {
            _target = CheckCloseTargets(battlefield);
            if(_target != null)
            {
                Attack(_target);


                return;
            } else
            {
                MoveTowards(FindNearestTargetPosition(battlefield), battlefield);
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
                battleField.SetCellCharacter(CurrentPosition, null);
                //CurrentPosition.occupied = null;
                battleField.SetCellCharacter(candidatePosition, this);
                //candidatePosition.occupied = this;
                CurrentPosition = candidatePosition;
                Console.Write($"\n || {battleField.GetCellCharacter(candidatePosition.x, candidatePosition.y)} ||");
                battleField.OnBattlefieldChanged();
            } else
            {
                //did not move
            }

            //if(battleField.Grid2D[candidatePosition.x, candidatePosition.y].occupied !=null)
            //{

            //}
            // if there is no target close enough, calculates in wich direction this character should move to be closer to a possible target
            //if(this.CurrentBox.xIndex > _target.CurrentBox.xIndex)
            //{
            //    if((battlefield.grids.Exists(x => x.Index == CurrentBox.Index - 1)))
            //    {
            //        CurrentBox.occupied = false;
            //        battlefield.grids[CurrentBox.Index] = CurrentBox;
            //        CurrentBox = (battlefield.grids.Find(x => x.Index == CurrentBox.Index - 1));
            //        CurrentBox.occupied = true;
            //        battlefield.grids[CurrentBox.Index] = CurrentBox;
            //        Console.WriteLine($"Player {_playerIndex} walked left\n");
            //        battlefield.DrawBattlefield();

            //        return;
            //    }
            //} else if(CurrentBox.xIndex < _target.CurrentBox.xIndex)
            //{
            //    CurrentBox.occupied = false;
            //    battlefield.grids[CurrentBox.Index] = CurrentBox;
            //    CurrentBox = (battlefield.grids.Find(x => x.Index == CurrentBox.Index + 1));
            //    CurrentBox.occupied = true;
            //    return;
            //    battlefield.grids[CurrentBox.Index] = CurrentBox;
            //    Console.WriteLine($"Player {_playerIndex} walked right\n");
            //    battlefield.DrawBattlefield();
            //}

            //if(this.CurrentBox.yIndex > _target.CurrentBox.yIndex)
            //{
            //    battlefield.DrawBattlefield();
            //    this.CurrentBox.occupied = false;
            //    battlefield.grids[CurrentBox.Index] = CurrentBox;
            //    this.CurrentBox = (battlefield.grids.Find(x => x.Index == CurrentBox.Index - battlefield.XLenght));
            //    this.CurrentBox.occupied = true;
            //    battlefield.grids[CurrentBox.Index] = CurrentBox;
            //    Console.WriteLine($"Player {_playerIndex} walked up\n");
            //    return;
            //} else if(this.CurrentBox.yIndex < _target.CurrentBox.yIndex)
            //{
            //    this.CurrentBox.occupied = true;
            //    battlefield.grids[CurrentBox.Index] = this.CurrentBox;
            //    this.CurrentBox = (battlefield.grids.Find(x => x.Index == CurrentBox.Index + battlefield.XLenght));
            //    this.CurrentBox.occupied = false;
            //    battlefield.grids[CurrentBox.Index] = CurrentBox;
            //    Console.WriteLine($"Player {_playerIndex} walked down\n");
            //    battlefield.DrawBattlefield();

            //    return;
            //}
        }
        public void Attack(Character target)
        {
            var rand = new Random();
            target.TakeDamage(rand.Next(0, GetDamage()));
            Console.WriteLine($"Player {PlayerIndex} is attacking the player {_target.PlayerIndex} and did {_baseDamage} damage\n");
        }
    }
}
