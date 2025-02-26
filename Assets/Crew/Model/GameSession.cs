using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Crew.Components.LevelManagment;
using Crew.Model.Data;
using Crew.Utils.Disposables;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Crew.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] public PlayerData _data;
        [SerializeField] public string _defaultCheckPoint;
        [SerializeField] public string _saveScene;
        [SerializeField] public string _lastCheck;

        public PlayerData Data => _data;
        private PlayerData _save;

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        public QuickInventoryModel QuickInventory { get; private set; }
        public PerksModel PerksModel { get; private set; }

        public List<string> _checkpoints = new List<string>();
        public string saveCheck;

        private bool _sessionStarted = false;

        // Метод для установки последнего сохраненного чекпоинта
        public void SetLastCheckpoint(string lastCheckpoint)
        {
            _defaultCheckPoint = lastCheckpoint;
            saveCheck = lastCheckpoint;
        }

        private void Awake()
        {
            var existsSession = GetExistsSession();
            if (existsSession != null)
            {
                existsSession.StartSession(_defaultCheckPoint);
                Destroy(gameObject);
            }
            else
            {
                Save();
                InitModels();
                DontDestroyOnLoad(this);
            }
        }

        private void Start()
        {
            if (!_sessionStarted)
            {
                if (!string.IsNullOrEmpty(_lastCheck))
                {
                    StartSession(_lastCheck); // Если есть загруженное сохранение, используем сохраненный чекпоинт
                }
                else if (!string.IsNullOrEmpty(_defaultCheckPoint))
                {
                    StartSession(_defaultCheckPoint); // Если нет сохранения, но есть дефолтный чекпоинт, используем его
                }
                else
                {
                    // Иначе, если нет ни сохранения, ни дефолтного чекпоинта, выводим предупреждение
                    UnityEngine.Debug.LogWarning("No default checkpoint set and no saved checkpoint found!");
                }
                _sessionStarted = true;
            }
        }

        public void StartSession(string defaultCheckPoint)
        {
            var scene = SceneManager.GetActiveScene();
            _saveScene = scene.name;

            LoadHud();

            // Если чекпоинт уже установлен, то проверяем наличие его в списке перед добавлением
            if (!IsChecked(defaultCheckPoint))
            {
                SetChecked(defaultCheckPoint);
                _lastCheck = defaultCheckPoint; // Обновляем последний чекпоинт
            }

            SpawnHero(); // Вызываем спавн персонажа после установки чекпоинта
        }


        private void SpawnHero()
        {
            var checkpoints = FindObjectsOfType<CheckPointComponent>();

            // Получаем id чекпоинта, на котором должен спавниться персонаж
            var spawnCheckpointId = _checkpoints.Last();

            // Находим чекпоинт с соответствующим id и спавним персонажа
            foreach (var checkPoint in checkpoints)
            {
                if (checkPoint.Id == spawnCheckpointId)
                {
                    checkPoint.SpawnHero();
                    break;
                }
            }
        }

        private void InitModels()
        {
            QuickInventory = new QuickInventoryModel(_data);
            _trash.Retain(QuickInventory);

            PerksModel = new PerksModel(_data);
            _trash.Retain(PerksModel);
        }

        private void LoadHud()
        {
            SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
            LoadOnScreenControls();
        }

        [Conditional("USE_ONSCREEN_CONTROLS")]
        private void LoadOnScreenControls()
        {
            SceneManager.LoadScene("Controls", LoadSceneMode.Additive);
        }

        private GameSession GetExistsSession()
        {
            var sessions = FindObjectsOfType<GameSession>();
            foreach (var gameSession in sessions)
            {
                if (gameSession != this)
                    return gameSession;
            }

            return null;
        }

        public void Save()
        {
            _save = _data.Clone();
        }

        public void LoadLastSave()
        {
            _data = _save.Clone();
            _trash.Dispose();
            InitModels();
        }

        public bool IsChecked(string id)
        {
            return _checkpoints.Contains(id);
        }

        public void SetChecked(string id)
        {
            if (!_checkpoints.Contains(id))
            {
                Save();
                _checkpoints.Add(id);
                _lastCheck = id; // Обновляем последний чекпоинт
            }
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }

        public List<string> _removedItems = new List<string>();

        public bool RestoreState(string id)
        {
            return _removedItems.Contains(id);
        }

        public void StoreState(string id)
        {
            if (!_removedItems.Contains(id))
                _removedItems.Add(id);
        }

        // Метод для применения сохраненного инвентаря
        public void ApplySavedInventory(List<InventoryItemSaveData> savedInventory)
        {
            QuickInventory.ApplySavedInventory(savedInventory);
            _trash.Retain(QuickInventory);
        }

        // Метод для применения сохраненных перков
        public void ApplySavedPerks(List<string> savedPerkIds)
        {
            PerksModel.ApplySavedPerks(savedPerkIds);
            _trash.Retain(PerksModel);
        }
    }
}
