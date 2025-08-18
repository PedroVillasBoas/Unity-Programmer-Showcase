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
