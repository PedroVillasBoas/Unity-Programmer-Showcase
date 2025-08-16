namespace GoodVillageGames.Core.Attributes.Upgrades.Strategies
{
    public class SubtractiveStrategy : IModificationStrategy
    { 
        public float Apply(float originalValue, float modificationValue) => originalValue - modificationValue;
    }
}