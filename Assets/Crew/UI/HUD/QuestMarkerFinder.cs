using Crew.Model.Definitions.Localization;
using UnityEngine;
using UnityEngine.UI;

namespace Crew.UI.HUD
{
    public class QuestMarkerFinder : MonoBehaviour
    {
        [SerializeField] private string _questMarkerPrefabName = "QuestMarker"; // Имя префаба объекта QuestMarker
        [SerializeField] private string _questTextLocalizationKey = "quest_marker_text"; // Ключ локализации для текста квеста

        public void FindAndSetQuestText()
        {
            // Получаем локализованный текст квеста
            string localizedQuestText = LocalizationManager.I.Localize(_questTextLocalizationKey);

            // Ищем объект QuestMarker на сцене по имени префаба
            GameObject questMarkerObject = GameObject.Find(_questMarkerPrefabName);
            if (questMarkerObject != null)
            {
                // Получаем компонент текста на объекте QuestMarker
                Text questTextComponent = questMarkerObject.GetComponentInChildren<Text>();
                if (questTextComponent != null)
                {
                    // Устанавливаем локализованный текст квеста на компоненте текста
                    questTextComponent.text = localizedQuestText;
                }
                else
                {
                    Debug.LogWarning("Не удалось найти компонент текста на объекте QuestMarker.");
                }
            }
            else
            {
                Debug.LogWarning($"Не удалось найти объект QuestMarker с именем {_questMarkerPrefabName} на сцене.");
            }
        }
    }
}
