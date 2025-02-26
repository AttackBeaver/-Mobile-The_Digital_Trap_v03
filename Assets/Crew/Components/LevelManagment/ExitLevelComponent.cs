using Crew.Model;
using Crew.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Crew.Components.LevelManagment
{
    public class ExitLevelComponent : MonoBehaviour
    {
        [SerializeField] private string _sceneName;
        
        public void Exit()
        {
            var session = FindObjectOfType<GameSession>();
            session.Save();
            var loader = FindObjectOfType<LevelLoader>();
            loader.LoadLevel(_sceneName);
        }
    }
}
