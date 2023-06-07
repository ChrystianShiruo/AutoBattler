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
        protected List<IStatusEffect> _statuses;
        protected float _maxHp;

        public Character(CharacterClassInfo characterClassInfo, int id, ColorScheme color, int teamId)
        {
            _maxHp = 100f + characterClassInfo.HpModifier;
            Health = _maxHp;
            PlayerIndex = id;
            _damageMultiplier = 1f;
            CharacterClassInfo = characterClassInfo;
            _baseDamage = characterClassInfo.ClassDamage;
            Name = $"{id}. {characterClassInfo.Name}";
            AttackRange = characterClassInfo.AttackRange;
            _attackablePositions = CacheTargetablePositions(AttackRange);
            _statuses = new List<IStatusEffect>();

            Team = teamId;
            _grid = null;
            Color = (ConsoleColor)color;
        }
        //public Character()
        //{

        //}
        protected bool _incapacitated => _statuses.Exists(statusE => statusE._Status == Status.Stun);
        public int PlayerIndex { get; protected set; }
        public string Name { get; protected set; }
        public float Health { get; protected set; }
        public float MaxDamage { get => GetDamage(); }
        public int AttackRange { get; protected set; }
        public int Team { get; protected set; }
        public Vector2Int CurrentPosition { get; protected set; }
        public ConsoleColor Color { get; protected set; }
        public Action OnTurnStart { get; set; }
        public CharacterClassInfo CharacterClassInfo { get; set; }


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

        public void HealDamage(float amount)
        {
            float max = _maxHp - Health;
            amount = amount > max ? max : amount;
            Messages.ColoredWriteLine($"Character {Name} was healed for {amount} is now with {Health} hp", Color);
        }
        public bool TakeDamage(float amount)
        {
            if(amount < 0)
            {
                return false;
            }
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

        public void RemoveStatus(IStatusEffect statusEffect)
        {
            Messages.ColoredWriteLine($"Removed {statusEffect._Status} from {Name}!", Color);
            _statuses.Remove(statusEffect);
        }
        public void AddStatus(IStatusEffect statusEffect)
        {
            Messages.ColoredWriteLine($"Added {statusEffect._Status} to {Name}!", Color);
            _statuses.Add(statusEffect);
            statusEffect.OnApply(this);
        }

        public virtual void StartTurn()
        {
            OnTurnStart?.Invoke();

            if(Health <= 0 || _incapacitated)
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
            if(_target != null)
            {
                TryToUseSkills();
            }


            Messages.ColoredWriteLine($"{Name} finished his turn.", Color);

        }

        // Check in x and y directions if there is any character close enough to be a target.
        protected Character CheckCloseTargets(Grid battlefield)
        {
            if(_target != null && _target.Health > 0 && AttackRange >= Vector2Int.Distance(_target.CurrentPosition, CurrentPosition))
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

        private void TryToUseSkills()
        {
            foreach(CharacterSkills skill in CharacterClassInfo.Skills)
            {
                StatusEffect statusEffect;
                Data.StatusEffectData.StatusEffects.TryGetValue(skill.specialEffect, out statusEffect);
                if(statusEffect != null && _target != null)
                {
                    //switch(skill.specialEffect)
                    //{
                    //    case Status.Bleed:
                    //        statusEffect = new Bleed();
                    //        statusEffect.StatusValue;
                    //        break;
                    //    case Status.Stun:
                    //        break;
                    //    case Status.Heal:
                    //        break;
                    //}
                    statusEffect.Target = _target;
                    statusEffect.StatusValue = skill.damage;
                    //statusEffect.StatusValue = skill.damage;

                    statusEffect.OnApply(_target);
                    //statusEffect.OnApply(this);
                }

            }
        }

        public enum ColorScheme
        {
            Player = (int)ConsoleColor.Green,
            Enemy = (int)ConsoleColor.Red,
            Ally = (int)ConsoleColor.Cyan
        }
    }
}
