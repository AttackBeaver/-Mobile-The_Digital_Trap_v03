using System;
using Crew.Definitions;
using UnityEngine;
using UnityEngine.Events;

namespace Crew.Components.Health
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health; // Текущее здоровье объекта.
        [SerializeField] private UnityEvent _onDamage; // Событие при получении урона.
        [SerializeField] private UnityEvent _onHeal; // Событие при лечении.
        [SerializeField] public UnityEvent _onDie; // Событие при смерти.
        [SerializeField] public HealthChangeEvent _onChange; // Событие при изменении здоровья.

        public int Health => _health;

        /// <summary>
        /// Метод для изменения здоровья объекта.
        /// </summary>
        /// <param name="healthDelta">Изменение здоровья (может быть положительным или отрицательным).</param>
        public void ModifyHealth(int healthDelta)
        {
            _health += healthDelta; // Изменяем текущее здоровье
            _onChange?.Invoke(_health); // Вызываем событие изменения здоровья

            // Вызываем событие получения урона, если здоровье уменьшилось
            if (healthDelta < 0)
            {
                _onDamage?.Invoke();
            }

            // // Вызываем событие лечения, если здоровье увеличилось
            if (healthDelta > 0) // Добавляем проверку, чтобы избежать вызова события лечения, если здоровье уже полное
            {
                _onHeal?.Invoke();
            }

            // Вызываем событие смерти, если здоровье стало меньше или равно нулю
            if (_health <= 0)
            {
                _onDie?.Invoke();
            }
        }

        /// <summary>
        /// Метод для установки здоровья объекта.
        /// </summary>
        /// <param name="health">Значение здоровья.</param>
        public void SetHealth(int health)
        {
            _health = health;
        }

        /// <summary>
        /// Пользовательское событие, которое передает новое значение здоровья.
        /// </summary>
        [Serializable]
        public class HealthChangeEvent : UnityEvent<int>
        {
        }
    }
}
