using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace GoodVillageGames.Core.Itemization.Looting
{

    /// <summary>
    /// A serializable class that defines a single potential item drop in the loot table.
    /// </summary>
    [System.Serializable]
    public class LootDrop
    {
        public ItemData ItemData;
        [Range(1, 100)]
        public int Weight = 100;
        [Range(1, 100)]
        public int MinQuantity = 1;
        [Range(1, 100)]
        public int MaxQuantity = 1;
    }

    /// <summary>
    /// A ScriptableObject that represents a collection of possible item drops.
    /// It uses a weighted system to determine which item to spawn.
    /// </summary>
    [CreateAssetMenu(fileName = "LootTable", menuName = "GoodVillageGames/Items/Loot Table")]
    public class LootTable : ScriptableObject
    {
        [Header("Guaranteed Drops")]
        [Tooltip("A list of items that will ALWAYS drop. Quantity is randomized.")]
        public List<LootDrop> GuaranteedDrops;

        [Header("Random Drops")]
        [Tooltip("The list of possible random drops from this source.")]
        public List<LootDrop> RandomDrops;

        /// <summary>
        /// Generates a complete list of loot, including all guaranteed drops
        /// and a specified number of random rolls.
        /// </summary>
        /// <param name="numberOfRandomRolls">The number of times to roll on the random drop table.</param>
        /// <returns>A list of InventorySlots containing all the generated loot.</returns>
        public List<InventorySlot> GenerateLoot(int numberOfRandomRolls = 1)
        {
            var generatedLoot = new List<InventorySlot>();

            if (GuaranteedDrops != null)
            {
                foreach (var guaranteedDrop in GuaranteedDrops)
                {
                    int quantity = Random.Range(guaranteedDrop.MinQuantity, guaranteedDrop.MaxQuantity + 1);
                    generatedLoot.Add(new InventorySlot(guaranteedDrop.ItemData, quantity));
                }
            }

            for (int i = 0; i < numberOfRandomRolls; i++)
            {
                InventorySlot randomDrop = GetRandomDrop();
                if (randomDrop != null)
                {
                    generatedLoot.Add(randomDrop);
                }
            }

            return generatedLoot;
        }

        /// <summary>
        /// Calculates and returns a single random item drop based on the defined weights.
        /// </summary>
        private InventorySlot GetRandomDrop()
        {
            if (RandomDrops == null || RandomDrops.Count == 0) return null;

            int totalWeight = RandomDrops.Sum(drop => drop.Weight);
            int randomNumber = Random.Range(1, totalWeight + 1);

            foreach (var drop in RandomDrops)
            {
                if (randomNumber <= drop.Weight)
                {
                    int quantity = Random.Range(drop.MinQuantity, drop.MaxQuantity + 1);
                    return new InventorySlot(drop.ItemData, quantity);
                }
                randomNumber -= drop.Weight;
            }

            return null;
        }
    }
}