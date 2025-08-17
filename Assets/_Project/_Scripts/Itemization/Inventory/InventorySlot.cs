namespace GoodVillageGames.Core.Itemization
{
    public class InventorySlot
    {
        public ItemData itemData;
        public int quantity;

        public InventorySlot(ItemData item, int amount)
        {
            itemData = item;
            quantity = amount;
        }

        public void AddToStack(int amount)
        {
            quantity += amount;
        }
    }
}
