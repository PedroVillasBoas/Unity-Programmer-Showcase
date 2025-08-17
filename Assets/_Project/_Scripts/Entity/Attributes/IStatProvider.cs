using GoodVillageGames.Core.Mediator;
using GoodVillageGames.Core.Enums.Attributes;

namespace GoodVillageGames.Core.Character.Attributes
{
    /// <summary>
    /// The 'Stats' class depend on this abstraction, not the concrete 'BaseStats' ScriptableObject.
    /// </summary>
    public interface IStatProvider
    {
        float GetStat(AttributeType type);
        T GetModifier<T>() where T : StatModifier;
        void AddModifier(StatModifier modifier);
        void RemoveModifiersFromSource(object source);
    }
}