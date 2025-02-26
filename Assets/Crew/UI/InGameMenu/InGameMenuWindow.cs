using Crew.Model;
using Crew.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Crew.UI.InGameMenu
{
    public class InGameMenuWindow : AnimatedWindow
    {
        private float _defaultTimeScale;
        private PlayerInput _playerInput;
        private GameSession _gameSession;

        protected override void Start()
        {
            base.Start();
            _defaultTimeScale = Time.timeScale;
            Time.timeScale = 0;

            _playerInput = FindObjectOfType<PlayerInput>();
            _playerInput.enabled = false;
        }

        public void OnShowSettings()
        {
            WindowUtils.CreateWindow("UI/SettingsWindow");
        }

        public void OnSaveGame()
        {
           
        }

        public void OnExit()
        {
            SceneManager.LoadScene("Main_Menu");

            var session = FindObjectOfType<GameSession>();
            Destroy(session.gameObject);
        }

        private void OnDestroy()
        {
            Time.timeScale = _defaultTimeScale;
            _playerInput.enabled = true;
        }
    }
}
