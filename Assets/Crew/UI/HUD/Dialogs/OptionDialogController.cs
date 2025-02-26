using System;
using Crew.Model.Definitions.Localization;
using Crew.UI.Hud.Dialogs;
using Crew.UI.Widgets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Crew.UI.HUD.Dialogs
{
    public class OptionDialogController : MonoBehaviour
    {
        [SerializeField] private GameObject _content;
        [SerializeField] private Text _contentText;
        [SerializeField] private Transform _optionsContainer;
        [SerializeField] private OptionItemWidget _prefab;

        private DataGroup<OptionData, OptionItemWidget> _dataGroup;

        private void Start()
        {
            _dataGroup = new DataGroup<OptionData, OptionItemWidget>(_prefab, _optionsContainer);
        }

        public void OnOptionsSelected(OptionData selectedOption)
        {
            selectedOption.OnSelect.Invoke();
            _content.SetActive(false);
        }

        public void Show(OptionDialogData data)
        {
            _content.SetActive(true);
            _contentText.text = LocalizationManager.I.Localize(data.DialogText);

            _dataGroup.SetData(data.Options);
        }
    }

    [Serializable]
    public class OptionDialogData
    {
        public string DialogText;
        public OptionData[] Options;
    }

    // [Serializable]
    // public class OptionData
    // {
    //     public string Text;
    //     public UnityEvent OnSelect;
    // }

    [Serializable]
    public class OptionData
    {
        [SerializeField] private string _textKey;
        public UnityEvent OnSelect;

        public string Text => LocalizationManager.I.Localize(_textKey);

        // Конструктор для установки ключа локализации текста
        public OptionData(string textKey)
        {
            _textKey = textKey;
        }
    }
}
