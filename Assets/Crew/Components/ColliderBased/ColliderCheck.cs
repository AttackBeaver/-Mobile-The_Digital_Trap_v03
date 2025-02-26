using UnityEngine;

namespace Crew.Components.ColliderBased
{
    public class ColliderCheck : LayerCheck
    {
        private Collider2D _collider; // Ссылка на коллайдер объекта.

        private void Awake()
        {
            _collider = GetComponent<Collider2D>(); // Получаем ссылку на коллайдер при инициализации.
        }

        /// <summary>
        /// Вызывается, когда другой коллайдер находится в зоне действия этого коллайдера.
        /// </summary>
        /// <param name="other">Другой коллайдер, находящийся в зоне действия.</param>
        private void OnTriggerStay2D(Collider2D other)
        {
            _isTouchingLayer = _collider.IsTouchingLayers(_layer); // Проверяем, находится ли коллайдер на слое.
        }

        /// <summary>
        /// Вызывается, когда другой коллайдер покидает зону действия этого коллайдера.
        /// </summary>
        /// <param name="other">Другой коллайдер, покидающий зону действия.</param>
        private void OnTriggerExit2D(Collider2D other)
        {
            _isTouchingLayer = _collider.IsTouchingLayers(_layer); // Проверяем, находится ли коллайдер на слое.
        }
    }
}
