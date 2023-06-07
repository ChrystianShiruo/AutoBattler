using System;
using System.Collections.Generic;

namespace AutoBattle
{
    public class Character : ICharacter
    {
        protected float _baseDamage;
        protected float _damageMultiplier;
        protected Character _target;
        protected Vector2Int[] _attackablePositions;
        protected Grid _grid;
        protected CharacterClassInfo _characterClassInfo;
        public Character(CharacterClassInfo characterClassInfo, int id, ColorScheme color, int teamId)
        {
            Health = 100f + characterClassInfo.hpModifier;
            PlayerIndex = id;
            _damageMultiplier = 1f;
            _characterClassInfo = characterClassInfo;
            _baseDamage = characterClassInfo.classDamage;
            Name = $"{id}. {characterClassInfo.characterClass}";
            AttackRange = characterClassInfo.attackRange;
            _attackablePositions = CacheTargetablePositions(AttackRange);

            Team = teamId;
            _grid = null;
            Color = (ConsoleColor)color;
        }

        public int PlayerIndex { get; protected set; }
        public string Name { get; protected set; }
        public float Health { get; protected set; }
        public float MaxDamage { get => GetDamage(); }
        public int AttackRange { get; protected set; }
        public int Team { get; protected set; }
        public Vector2Int CurrentPosition { get; protected set; }
        public ConsoleColor Color { get; protected set; }

        //caching so that we do not recalculate
        protected Vector2Int[] CacheTargetablePositions(int range)
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
            Health -= amount;
            Messages.ColoredWriteLine($"Character {Name} is now with {Math.Max(Health, 0)} hp", Color);

            if(Health <= 0)
            {
                Die();
                return true;
            }
            return false;
        }
        protected int GetDamage()
        {
            return (int)(_baseDamage * _damageMultiplier);
        }
        public void Die()
        {
            if(_grid.TryRemoveCharacter(CurrentPosition, this))
            {
                Messages.ColoredWriteLine($"Character {Name} died!", Color);
                _grid.OnBattlefieldChanged();
            }
        }


        public void StartTurn()
        {
            if(Health <= 0)
            {
                return;
            }
            Messages.ColoredWriteLine($"{Name} is acting...", Color);
            _target = CheckCloseTargets(_grid);
            if(_target != null)
            {
                Attack(_target);
            } else if(!MoveTowards(FindNearestTargetPosition(_grid), _grid))
            {
                Messages.ColoredWriteLine($"{Name} Is idle.", Color);
            }

            Messages.ColoredWriteLine($"{Name} finished his turn.", Color);

        }

        // Check in x and y directions if there is any character close enough to be a target.
        protected Character CheckCloseTargets(Grid battlefield)
        {
            if(_target != null && _target.Health > 0  && AttackRange >= Vector2Int.Distance(_target.CurrentPosition, CurrentPosition))
            {
                return _target;
            }

            foreach(Vector2Int position in _attackablePositions)
            {
                Character target = battlefield.GetCellCharacter(CurrentPosition.x + position.x, CurrentPosition.y + position.y);
                if(target != null && target.Team != Team)
                {
                    return target;
                }
            }

            return null;
        }
        protected Vector2Int FindNearestTargetPosition(Grid battlefield)
        {
            Vector2Int targetPosition = CurrentPosition;
            int distance = Vector2Int.Distance(new Vector2Int(0, 0), new Vector2Int(battlefield.XLenght, battlefield.YLength));//max possible distance
            foreach(Character character in Program.TeamCharacters[Team == 1 ? 0 : 1])
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
        protected bool MoveTowards(Vector2Int targetPosition, Grid battleField)//TODO: create & implement _movementRange; smarter AI pathing to coordinate?
        {
            if(targetPosition == CurrentPosition)
            {
                return false;
            }

            Vector2Int candidatePosition = battleField.MoveTowards(CurrentPosition, targetPosition, 1);
            if(candidatePosition != CurrentPosition && battleField.TryMoveCharacter(CurrentPosition, candidatePosition))
            {
                Messages.ColoredWriteLine
                    ($"{Name} moved from [{CurrentPosition.x},{CurrentPosition.y}] to [{candidatePosition.x},{candidatePosition.y}]", Color);

                CurrentPosition = candidatePosition;
                battleField.OnBattlefieldChanged();
                return true;
            } else
            {
                return false;
                //did not move
            }
        }

        protected void Attack(Character target)
        {
            var rand = new Random();
            float maxDamage = GetDamage();
            float damageDealt = (float)rand.NextDouble() * maxDamage;

            Messages.ColoredWrite($"Character {Name}", Color);
            Console.Write(" is attacking ");
            Messages.ColoredWrite($"Character {target.Name}", target.Color);
            Console.Write($" and did {damageDealt} damage \n");

            target.TakeDamage(damageDealt);
        }

        public enum ColorScheme
        {
            Player = (int)ConsoleColor.Green,
            Enemy = (int)ConsoleColor.Red,
            Ally = (int)ConsoleColor.Cyan
        }
    }
}
