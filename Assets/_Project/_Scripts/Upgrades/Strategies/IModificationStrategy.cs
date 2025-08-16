namespace GoodVillageGames.Core.Attributes.Upgrades.Strategies
{
    /// <summary>
    /// Interface for the Strategy pattern. Each strategy will define a
    /// specific mathematical operation to be applied to a stat.
    /// </summary>
    public interface IModificationStrategy
    {
        float Apply(float originalValue, float modificationValue);
    }
}