using Crew.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Crew.Components.LevelManagment
{
    public class ReloadLevelComponent : MonoBehaviour
    {
        /// <summary>
        /// Метод для перезагрузки уровня.
        /// </summary>
        public void Reload()
        {
            var session = FindObjectOfType<GameSession>(); // Поиск объекта GameSession в сцене.
            session.LoadLastSave(); // Загрузка последнего сохранения из сессии.

            var scene = SceneManager.GetActiveScene(); // Получение активной сцены.
            SceneManager.LoadScene(scene.name); // Перезагрузка текущей сцены.
        }
    }
}
