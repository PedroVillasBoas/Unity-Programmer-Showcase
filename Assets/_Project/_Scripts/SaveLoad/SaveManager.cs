using System.IO;
using UnityEngine;

namespace GoodVillageGames.Core.Itemization.Equipment
{
    /// <summary>
    /// Manages saving and loading the game state to and from a file.
    /// </summary>
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }

        [SerializeField] private float _autoSaveInterval = 60f;

        private float _autoSaveTimer;
        private string _saveFilePath;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                _saveFilePath = Path.Combine(Application.persistentDataPath, "PedroVilasBoas-GameShowcase-savedata.json");
                _autoSaveTimer = _autoSaveInterval;
            }
        }

        private void Update()
        {
            _autoSaveTimer -= Time.unscaledDeltaTime;

            AutoSave();
        }

        private void AutoSave()
        {
            if (_autoSaveTimer <= 0f)
            {
                Debug.Log("Autosaving...");
                SaveGame();
                _autoSaveTimer = _autoSaveInterval;
            }
        }

        private void OnApplicationQuit()
        {
            Debug.Log("Application quitting, performing final save...");
            SaveGame();
        }

        public void SaveGame()
        {
            Debug.Log("Saving game...");

            SaveData saveData = new()
            {
                // Ask the managers for their current state
                inventoryData = InventoryManager.Instance.GetSaveData(),
                equipmentData = EquipmentManager.Instance.GetSaveData()
            };

            string json = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(_saveFilePath, json);

            Debug.Log("Game saved to: " + _saveFilePath);
        }

        public void LoadGame()
        {
            if (File.Exists(_saveFilePath))
            {
                Debug.Log("Loading game...");

                string json = File.ReadAllText(_saveFilePath);
                SaveData saveData = JsonUtility.FromJson<SaveData>(json);

                InventoryManager.Instance.LoadSaveData(saveData.inventoryData);
                EquipmentManager.Instance.LoadSaveData(saveData.equipmentData);

                Debug.Log("Game loaded.");
            }
            else
            {
                Debug.Log("No save file found.");
            }
        }
    }
}