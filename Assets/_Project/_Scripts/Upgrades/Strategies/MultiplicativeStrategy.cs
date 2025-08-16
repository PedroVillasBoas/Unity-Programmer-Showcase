namespace GoodVillageGames.Core.Attributes.Upgrades.Strategies
{
    public class MultiplicativeStrategy : IModificationStrategy
    { 
        public float Apply(float originalValue, float modificationValue) => originalValue * modificationValue;
    }
}