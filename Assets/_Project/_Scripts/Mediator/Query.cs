using GoodVillageGames.Core.Enums.Attributes;

namespace GoodVillageGames.Core.Mediator
{
    /// <summary>
    /// It removes the massive overhead of GameObject/Component instantiation for what should be a simple data transfer object ¬¬
    /// </summary>
    public class Query
    {
        public AttributeType StatType { get; set; }
        public float Value { get; set; }

        // Constructor has no parameters to allow for object reuse
        public Query() { }

        public Query(AttributeType statType, float value)
        {
            StatType = statType;
            Value = value;
        }
    }
}