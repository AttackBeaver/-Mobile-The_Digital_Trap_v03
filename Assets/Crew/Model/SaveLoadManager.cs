using System.Collections.Generic;
using System.IO;
using Crew.Model;
using Crew.UI;
using Crew.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSaver : MonoBehaviour
{
    // Метод для сохранения данных
    public void SaveData()
    {
        // Найти объект GameSession на сцене
        GameSession gameSession = FindObjectOfType<GameSession>();

        // Проверить, найден ли объект GameSession
        if (gameSession != null)
        {
            // Создать объект SaveData и заполнить его данными из GameSession
            SaveData saveData = new SaveData
            {
                sceneName = SceneManager.GetActiveScene().name,
                lastCheckpoint = gameSession._lastCheck,
                perkIds = gameSession.PerksModel.GetPerkIds(),
                inventoryItems = gameSession.QuickInventory.GetInventorySaveData()
            };

            // Сериализовать объект SaveData в JSON
            string json = JsonUtility.ToJson(saveData);

            // Путь к файлу сохранения
            string filePath = Application.persistentDataPath + "/save.json";

            // Записать JSON в файл
            File.WriteAllText(filePath, json);

            WindowUtils.CreateWindow("UI/SaveMessage");
            Debug.Log("Save data written to: " + filePath);
        }
        else
        {
            Debug.LogWarning("GameSession object not found in the scene!");
        }
    }

    // Метод для загрузки данных
    public void LoadData()
    {
        // Путь к файлу сохранения
        string filePath = Application.persistentDataPath + "/save.json";

        // Проверить, существует ли файл сохранения
        if (File.Exists(filePath))
        {
            // Прочитать JSON из файла сохранения
            string json = File.ReadAllText(filePath);
            Debug.Log("Loaded data: " + json);

            // Десериализовать JSON в объект SaveData
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            // Загрузить сцену
            var loader = FindObjectOfType<LevelLoader>();
            loader.LoadLevel(saveData.sceneName);
            // SceneManager.LoadScene(saveData.sceneName);

            // Найти объект GameSession на загруженной сцене
            SceneManager.sceneLoaded += OnSceneLoaded;

            void OnSceneLoaded(Scene scene, LoadSceneMode mode)
            {
                GameSession gameSession = FindObjectOfType<GameSession>();
                if (gameSession != null)
                {
                    // Применить сохраненные данные инвентаря и перков
                    gameSession.ApplySavedInventory(saveData.inventoryItems);
                    gameSession.ApplySavedPerks(saveData.perkIds);
                    gameSession.SetLastCheckpoint(saveData.lastCheckpoint);

                    SceneManager.sceneLoaded -= OnSceneLoaded;
                }
            }
        }
        else
        {
            Debug.LogWarning("Save file not found!");
        }
    }
}

// Класс для сохранения данных
[System.Serializable]
public class SaveData
{
    public string sceneName;
    public string lastCheckpoint;
    public List<string> perkIds;
    public List<InventoryItemSaveData> inventoryItems;
}

// Структура для сохранения данных о предметах инвентаря
[System.Serializable]
public struct InventoryItemSaveData
{
    public string itemId;
    public int quantity;
}

