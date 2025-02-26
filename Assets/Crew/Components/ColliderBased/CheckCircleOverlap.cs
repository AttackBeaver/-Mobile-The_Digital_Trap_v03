using UnityEngine;
using UnityEditor;
using Crew.Utils;
using System;
using UnityEngine.Events;
using System.Linq;

namespace Crew.Components.ColliderBased
{
    public class CheckCircleOverlap : MonoBehaviour
    {
        [SerializeField] private float _radius = 1f; // Радиус круглой области
        [SerializeField] private LayerMask _mask; // Маска слоев для проверки перекрытия
        [SerializeField] private string[] _tags; // Массив тегов для проверки перекрытия
        [SerializeField] private OnOverlapEvent _onOverlap; // Событие, вызываемое при перекрытии

        private Collider2D[] _interactionResult = new Collider2D[10]; // Результаты перекрытия

        // Рисует гизмо для обозначения круглой области при выборе объекта в редакторе
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Handles.color = HandlessUtils.TransparentRed; // Устанавливает цвет гизмо
            Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius); // Рисует круглую область
        }
#endif
        // Метод для проверки перекрытия
        public void Check()
        {
            // Получаем количество перекрытий
            var size = Physics2D.OverlapCircleNonAlloc(
                transform.position,
                _radius,
                _interactionResult,
                _mask);

            // Перебираем результаты перекрытия
            for (int i = 0; i < size; i++)
            {
                var overlapResult = _interactionResult[i]; // Получаем текущий результат перекрытия
                var isInTags = _tags.Any(tag => overlapResult.CompareTag(tag)); // Проверяем, содержится ли тег объекта в списке тегов

                if (isInTags)
                {
                    _onOverlap?.Invoke(overlapResult.gameObject); // Вызываем событие при перекрытии
                }
            }
        }

        // Событие, вызываемое при перекрытии
        [Serializable]
        public class OnOverlapEvent : UnityEvent<GameObject>
        {

        }

    }
}
