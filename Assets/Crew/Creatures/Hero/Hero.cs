using Crew.Components.ColliderBased;
using Crew.Components.Health;
using Crew.Model;
using Crew.Model.Data;
using Crew.Utils;
using UnityEngine;

namespace Crew.Creatures.Hero
{
   public class Hero : Creature, ICanAddInInventory
   {
      // Поля для взаимодействия
      [SerializeField] private float _interactionRadius; // Радиус взаимодействия
      [SerializeField] private CheckCircleOverlap _interactionCheck; // Проверка области взаимодействия
      [SerializeField] private LayerMask _interactionLayer; // Слой для взаимодействия

      // Поля для альтернативной атаки
      [SerializeField] private Cooldown _altattackCooldown; // Кулдаун альтернативной атаки
      private static readonly int AltAttackKey = Animator.StringToHash("alt_attack"); // Хэш для анимации альтернативной атаки
      private static readonly int IsDeadKey = Animator.StringToHash("is-dead");

      private bool _allowDoubleJump; // Позволить ли двойной прыжок
      private GameSession _session; // Сессия игры

      protected override void Awake()
      {
         base.Awake(); // Вызов метода базового класса
      }

      private void Start()
      {
         _session = FindObjectOfType<GameSession>(); // Поиск объекта GameSession в сцене
         _session.Data.Inventory.OnChanged += OnInventoryChanged; // Подписка на событие изменения инвентаря
         _session.Data.Inventory.OnChanged += AnotherHandler; // Дополнительный обработчик события изменения инвентаря

         var health = GetComponent<HealthComponent>(); // Получение компонента здоровья
         health.SetHealth(_session.Data.Hp.Value); // Установка здоровья сущности
      }

      private void OnDestroy()
      {
         _session.Data.Inventory.OnChanged -= OnInventoryChanged; // Отписка от события изменения инвентаря
      }

      private void AnotherHandler(string id, int value)
      {
         Debug.Log($"Изменено: {id}: {value}"); // Вывод сообщения о добавлении предмета в инвентарь в консоль
      }

      private void OnInventoryChanged(string id, int value)
      {
         // Проверка состояний инвентаря
         // Здесь можно добавить логику для изменения поведения героя в зависимости от предметов в инвентаре
      }

      public void OnHealthChanged(int currentHealth)
      {
         _session.Data.Hp.Value = currentHealth; // Обновление текущего здоровья в сессии игры
      }

      protected override void Update()
      {
         base.Update(); // Вызов метода базового класса
      }

      protected override float CalculateYVelocity()
      {
         if (IsGrounded)
         {
            _allowDoubleJump = true; // Разрешить двойной прыжок, если на земле
         }

         return base.CalculateYVelocity();
      }

      protected override float CalculateJumpVelocity(float yVelocity)
      {
         if (!IsGrounded && _allowDoubleJump && _session.PerksModel.IsDoubleJumpSupported)
         {
            _allowDoubleJump = false; // Запретить двойной прыжок после использования
            DoJumpVpx(); // Выполнить прыжок
            return _jumpSpeed; // Вернуть скорость прыжка
         }

         return base.CalculateJumpVelocity(yVelocity); // Вызов метода базового класса
      }

      public void AddInInventory(string id, int value)
      {
         _session.Data.Inventory.Add(id, value); // Добавить предмет в инвентарь
      }

      public override void TakeDamage()
      {
         base.TakeDamage(); // Вызов метода базового класса
      }

      public void Interact()
      {
         _interactionCheck.Check(); // Проверка области взаимодействия
      }

      public float attackCooldown = 1.0f; // Время отката между атаками
      private float lastAttackTime = 0.0f; // Время последней атаки

      public override void Attack()
      {
         // Проверяем, прошло ли достаточно времени с момента последней атаки
         if (Time.time - lastAttackTime >= attackCooldown)
         {
            base.Attack(); // Вызов метода базового класса

            lastAttackTime = Time.time; // Обновляем время последней атаки
         }
      }

      public void OnDoAltattack()
      {
         Sounds.Play("Range"); // Воспроизведение звука для альтернативной атаки
         _particles.Spawn("AltAttack"); // Спаун эффекта для альтернативной атаки
      }

      public void AltAttack()
      {
         if (_altattackCooldown.IsReady && _session.PerksModel.IsFireballSupported) // Проверка готовности к альтернативной атаке
         {
            Animator.SetTrigger(AltAttackKey); // Запуск анимации альтернативной атаки
            _altattackCooldown.Reset(); // Сброс кулдауна
         }
      }

      public void OnDie()
      {
         Animator.SetBool(IsDeadKey, true);
         Sounds.Play("Die"); // Проиграть звук смерти
      }
   }
}
