using System;
using Crew.Model;
using Crew.Utils;
using UnityEngine;

namespace Crew.UI.MainMenu
{
    public class MainMenuWindow : AnimatedWindow
    {
        private Action _closeAction;
        public void OnShowSettings()
        {
            WindowUtils.CreateWindow("UI/SettingsWindow");
        }

        public void OnStartGame()
        {
            _closeAction = () =>
            {
                var loader = FindObjectOfType<LevelLoader>();
                loader.LoadLevel("Level_1");
            };
            Close();
        }

        public void OnLoadSaveGame()
        {
            Close();
        }

        public void OnExit()
        {
            _closeAction = () =>
            {
                Application.Quit();

#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            };
            Close();
        }

        public override void OnCloseAnimatonComplete()
        {
            base.OnCloseAnimatonComplete();
            _closeAction?.Invoke();
        }
    }
}
