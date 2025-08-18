using UnityEngine;
using GoodVillageGames.Core.Enums.Attributes;
using GoodVillageGames.Core.Character.Attributes;
using GoodVillageGames.Core.Character.Attributes.Modifiers;
using GoodVillageGames.Core.Attributes.Upgrades.Strategies;

namespace GoodVillageGames.Core.Attributes.Upgrades
{
    /// <summary>
    /// This class acts as a factory for creating and applying stat modifiers.
    /// It uses the STRATEGY pattern to determine the modification logic, making it
    /// extensible without changing this class's code.
    /// </summary>
    [System.Serializable]
    public class Upgrade
    {
        [SerializeField] private AttributeType _statType;
        [SerializeField] private OperatorType _operatorType;
        [SerializeField] private float _value;

        // Duration is part of the upgrade definition, -1 or 0 means permanent ;)
        [SerializeField] private float _duration = -1f;

        public AttributeType StatType => _statType;
        public OperatorType OperatorType => _operatorType;
        public float Value => _value;

        // We hold a reference to a strategy object
        private IModificationStrategy _strategy;

        public void ApplyUpgrade(IStatProvider statProvider, object source)
        {
            _strategy = GetStrategy();

            // The 'source' is now passed when creating the new modifier.
            var modifier = new FunctionalStatModifier(source, _statType, _duration, (v) => _strategy.Apply(v, _value));

            statProvider.AddModifier(modifier);
        }

        private IModificationStrategy GetStrategy()
        {
            return _operatorType switch
            {
                OperatorType.Add => new AdditiveStrategy(),
                OperatorType.Multiply => new MultiplicativeStrategy(),
                OperatorType.Subtract => new SubtractiveStrategy(),
                _ => throw new System.ArgumentOutOfRangeException(nameof(_operatorType), "Invalid operator type specified.")
            };
        }
    }

    public enum OperatorType
    {
        Add,
        Multiply,
        Subtract
    }
}