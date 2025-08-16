namespace GoodVillageGames.Core.Attributes.Upgrades.Strategies
{
    public class AdditiveStrategy : IModificationStrategy
    { 
        public float Apply(float originalValue, float modificationValue) => originalValue + modificationValue;
    }
}