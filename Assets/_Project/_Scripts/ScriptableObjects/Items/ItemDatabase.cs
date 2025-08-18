using UnityEngine;
using System.Linq;
using TriInspector;
using System.Collections.Generic;

namespace GoodVillageGames.Core.Itemization
{
    [CreateAssetMenu(fileName = "ItemDatabase", menuName = "GoodVillageGames/Items/Item Database")]
    public class ItemDatabase : ScriptableObject
    {
        public List<ItemPrefabMapping> itemMappings;

        // Dict for speedy O(1) lookups when the game is in runtime
        private Dictionary<string, ItemData> _itemDataCache;

        private void OnEnable()
        {
            // Building the cache for speedy lookup
            _itemDataCache = new Dictionary<string, ItemData>();
            foreach (var mapping in itemMappings)
            {
                if (mapping.itemData != null && !_itemDataCache.ContainsKey(mapping.itemData.name))
                {
                    _itemDataCache.Add(mapping.itemData.name, mapping.itemData);
                }
            }
        }

        /// <summary>
        /// Finds the ItemData asset corresponding to the given item name.
        /// Uses a speedy dictionary lookup.
        /// </summary>
        /// <remarks>
        /// I could give the item an ID, but the project will have at most 20 items.
        /// So... I didn't bothered to do it.
        /// </remarks>
        public ItemData FindItemByName(string name)
        {
            _itemDataCache.TryGetValue(name, out ItemData itemData);
            return itemData;
        }

        public GameObject GetPrefab(ItemData data)
        {
            var mapping = itemMappings.FirstOrDefault(m => m.itemData == data);
            return mapping?.prefab;
        }
    }

    [System.Serializable]
    public class ItemPrefabMapping
    {
        [Title("Item Entry")]
        public ItemData itemData;
        public GameObject prefab;
    }
}
