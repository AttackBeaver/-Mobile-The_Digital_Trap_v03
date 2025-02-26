using System.Collections.Generic;
using Crew.Model.Definitions.Localization;
using Crew.UI.Widgets;
using UnityEngine;

namespace Crew.UI.Localization
{
    public class LocalizationWindow : AnimatedWindow
    {
        [SerializeField] private Transform _container;
        [SerializeField] private LocaleItemWidget _prefab;

        private DataGroup<LocaleInfo, LocaleItemWidget> _dataGroup;

        private readonly string[] _supportedLocales = { "Rus", "Eng", "Bel", "Chi" };

        private readonly Dictionary<string, string> _localeNames = new Dictionary<string, string>
        {
            { "Rus", "Русский" },
            { "Eng", "English" },
            { "Bel", "Беларуская мова"},
            { "Chi", "中文"}
            // Добавьте сюда другие пары названий файлов и их отображаемых названий
        };

        protected override void Start()
        {
            base.Start();
            _dataGroup = new DataGroup<LocaleInfo, LocaleItemWidget>(_prefab, _container);
            _dataGroup.SetData(ComposeData());
        }

        private List<LocaleInfo> ComposeData()
        {
            var data = new List<LocaleInfo>();
            foreach (var locale in _supportedLocales)
            {
                // data.Add(new LocaleInfo { LocaleId = locale });
                // Создаем объект LocaleInfo, указывая отображаемое название из словаря
                data.Add(new LocaleInfo { LocaleId = locale, DisplayName = _localeNames[locale] });
            }

            return data;
        }

        public void OnSelected(string selectedLocale)
        {
            LocalizationManager.I.SetLocale(selectedLocale);
        }
    }
}
