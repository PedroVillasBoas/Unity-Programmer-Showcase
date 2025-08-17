using GoodVillageGames.Core.Mediator;
using GoodVillageGames.Core.Interfaces;
using GoodVillageGames.Core.Enums.Attributes;

namespace GoodVillageGames.Core.Character.Attributes
{
    /// <summary>
    /// This class implements IStatProvider and IUpdatable to define its contract clearly.
    /// It encapsulates the logic for querying stats and managing the mediator.
    /// </summary>
    public class CharacterStats : IStatProvider, IUpdatable
    {

        private readonly StatsMediator _mediator;
        private readonly BaseStats _baseStats;
        private readonly Query _reusableQuery = new();

        public CharacterStats(StatsMediator mediator, BaseStats baseStats)
        {
            _mediator = mediator;
            _baseStats = baseStats;
        }

        /// <summary>
        ///  Centralizes the logic for fetching a base stat and running it through the mediator's query pipeline.
        /// </summary>
        public float GetStat(AttributeType type)
        {
            float baseValue = GetBaseValue(type);

            // Reusing the query object to prevent GC allocation ;)
            _reusableQuery.StatType = type;
            _reusableQuery.Value = baseValue;

            _mediator.PerformQuery(this, _reusableQuery);
            return _reusableQuery.Value;
        }

        public void AddModifier(StatModifier modifier)
        {
            _mediator.AddModifier(modifier);
        }

        public void RemoveModifiersFromSource(object source)
        {
            _mediator.RemoveModifiersFromSource(source);
        }

        /// <summary>
        /// Utility method to retrieve a specific modifier if needed.
        /// Useful if I manage to add the system that might want to check for the presence of a buff.
        /// If only I had more time... ;(
        /// </summary>
        public T GetModifier<T>() where T : StatModifier
        {
            return _mediator.GetModifier<T>();
        }

        public void Tick(float deltaTime)
        {
            _mediator.Update(deltaTime);
        }

        private float GetBaseValue(AttributeType type)
        {
            return type switch
            {
                AttributeType.Health => _baseStats.Health,
                AttributeType.Speed => _baseStats.Speed,
                AttributeType.Acceleration => _baseStats.Acceleration,
                AttributeType.Deceleration => _baseStats.Deceleration,
                AttributeType.JumpForce => _baseStats.JumpForce,
                AttributeType.DoubleJumpCooldown => _baseStats.DoubleJumpCooldown,
                AttributeType.InteractRange => _baseStats.InteractRange,
                AttributeType.CollectRange => _baseStats.CollectRange,
                AttributeType.Damage => _baseStats.Damage,
                AttributeType.AttackSpeed => _baseStats.AttackSpeed,
                AttributeType.DashTime => _baseStats.DashTime,
                AttributeType.DashSpeed => _baseStats.DashSpeed,
                AttributeType.DashRechargeRate => _baseStats.DashRechargeRate,
                _ => 0f
            };
        }


    }
}