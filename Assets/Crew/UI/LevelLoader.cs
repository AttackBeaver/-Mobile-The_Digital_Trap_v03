using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Crew.UI
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private Animator _animator; // Аниматор для управления анимацией экрана загрузки
        [SerializeField] private float _transitionTime; // Время анимации загрузки

        private static readonly int Enabled = Animator.StringToHash("Enabled"); // Хэш для управления параметром анимации

        /// <summary>
        /// Вызывается при завершении загрузки сцены.
        /// Инициализирует загрузчик уровней при загрузке сцены.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void OnAfterSceneLoad()
        {
            InitLoader();
        }

        /// <summary>
        /// Вызывается при старте объекта.
        /// Делает объект неуничтожаемым при переходе между сценами.
        /// </summary>
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Инициализирует загрузчик уровней путем загрузки сцены "Load_Level" при старте.
        /// </summary>
        private static void InitLoader()
        {
            SceneManager.LoadScene("Load_Level", LoadSceneMode.Additive);
        }

        /// <summary>
        /// Загружает указанный уровень с использованием анимации загрузки.
        /// </summary>
        /// <param name="sceneName">Имя загружаемой сцены.</param>
        public void LoadLevel(string sceneName)
        {
            StartCoroutine(StartAnimation(sceneName));
        }

        /// <summary>
        /// Начинает анимацию загрузки, а затем загружает указанный уровень и завершает анимацию.
        /// </summary>
        /// <param name="sceneName">Имя загружаемой сцены.</param>
        private IEnumerator StartAnimation(string sceneName)
        {
            _animator.SetBool(Enabled, true); // Включить анимацию загрузки
            yield return new WaitForSeconds(_transitionTime); // Подождать время анимации
            SceneManager.LoadScene(sceneName); // Загрузить указанный уровень
            _animator.SetBool(Enabled, false); // Выключить анимацию загрузки
        }
    }
}
