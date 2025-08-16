using System;
using GoodVillageGames.Core.Mediator;
using GoodVillageGames.Core.Enums.Attributes;

namespace GoodVillageGames.Core.Character.Attributes.Modifiers
{
    /// <summary>
    /// It applies a functional operation to a stat. This is a highly reusable and THE specific implementation of a StatModifier.
    /// </summary>
    public class FunctionalStatModifier : StatModifier
    {
        private readonly AttributeType _type;
        private readonly Func<float, float> _operation;

        public FunctionalStatModifier(AttributeType type, float duration, Func<float, float> operation) : base(duration)
        {
            _type = type;
            _operation = operation;
        }

        public override void Handle(object sender, Query query)
        {
            if (query.StatType == _type)
            {
                query.Value = _operation(query.Value);
            }
        }
    }
}