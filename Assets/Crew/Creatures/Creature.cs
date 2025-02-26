using Crew.Components.Audio;
using Crew.Components.ColliderBased;
using Crew.Components.GoGameObjects;
using Crew.Utils;
using UnityEngine;

namespace Crew.Creatures
{
    public class Creature : MonoBehaviour
    {
        // Поля для параметров существа
        [Header("Params")]
        [SerializeField] private bool _invertScale; // Инвертировать ли масштаб
        [SerializeField] private float _speed; // Скорость перемещения
        [SerializeField] public float _jumpSpeed; // Скорость прыжка
        [SerializeField] private float _damageVelocity; // Скорость урона

        // Поля для проверок коллизий
        [Header("Checkers")]
        [SerializeField] private ColliderCheck _groundCheck; // Проверка соприкосновения с землей
        [SerializeField] private CheckCircleOverlap _attackRange; // Проверка области атаки
        [SerializeField] protected SpawnListComponent _particles; // Компонент для спауна частиц

        // Защищенные поля компонентов
        protected Rigidbody2D Rigidbody; // Rigidbody для физики
        protected Vector2 Direction; // Направление движения
        protected Animator Animator; // Компонент анимации
        protected PlaySoundsComponent Sounds; // Компонент для проигрывания звуков

        protected bool IsGrounded; // Находится ли существо на земле
        private bool _isJumping; // Происходит ли прыжок в данный момент

        // Хэши анимационных параметров
        private static readonly int IsGroundKey = Animator.StringToHash("is-ground");
        private static readonly int IsRunningKey = Animator.StringToHash("is-running");
        private static readonly int VerticalVelocityKey = Animator.StringToHash("vertical-velocity");
        private static readonly int Hit = Animator.StringToHash("hurt");
        private static readonly int AttackKey = Animator.StringToHash("attack");

        // Инициализация компонентов
        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
            Sounds = GetComponent<PlaySoundsComponent>();
        }

        // Установить направление движения
        public void SetDirection(Vector2 direction)
        {
            Direction = direction;
        }

        // Обновление состояния существа
        protected virtual void Update()
        {
            IsGrounded = _groundCheck.IsTouchingLayer;
        }

        // Фиксированное обновление физики
        private void FixedUpdate()
        {
            var xVelocity = Direction.x * _speed;
            var yVelocity = CalculateYVelocity();
            Rigidbody.velocity = new Vector2(xVelocity, yVelocity);

            Animator.SetBool(IsGroundKey, IsGrounded);
            Animator.SetBool(IsRunningKey, Direction.x != 0);
            Animator.SetFloat(VerticalVelocityKey, Rigidbody.velocity.y);

            UpdateSpriteDirection(Direction);
        }

        // Расчет вертикальной скорости (для прыжка)
        protected virtual float CalculateYVelocity()
        {
            var yVelocity = Rigidbody.velocity.y;
            var isJumpPressing = Direction.y > 0;

            if (IsGrounded)
            {
                _isJumping = false;
            }

            if (isJumpPressing)
            {
                _isJumping = true;

                var isFalling = Rigidbody.velocity.y <= 0.001f;
                yVelocity = isFalling ? CalculateJumpVelocity(yVelocity) : yVelocity;
            }
            else if (Rigidbody.velocity.y > 0 && _isJumping)
            {
                yVelocity *= 1f;
            }

            return yVelocity;
        }

        // Расчет скорости прыжка
        protected virtual float CalculateJumpVelocity(float yVelocity)
        {
            if (IsGrounded)
            {
                yVelocity = _jumpSpeed;
                DoJumpVpx();
            }

            return yVelocity;
        }

        // Выполнить прыжок
        protected void DoJumpVpx()
        {
            _particles.Spawn("Jump"); // Спаун эффекта прыжка
            Sounds.Play("Jump"); // Проиграть звук прыжка
        }

        // Обновить направление спрайта
        public void UpdateSpriteDirection(Vector2 direction)
        {
            var multiplier = _invertScale ? -1 : 1; // Множитель масштаба для инверсии
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(multiplier, 1, 1); // Повернуть вправо
            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1 * multiplier, 1, 1); // Повернуть влево
            }
        }

        // Получить урон
        public virtual void TakeDamage()
        {
            _isJumping = false;
            Animator.SetTrigger(Hit); // Запустить анимацию получения урона
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, _damageVelocity); // Применить отброс от удара
        }

        // Выполнить атаку
        public virtual void Attack()
        {
            Animator.SetTrigger(AttackKey); // Запустить анимацию атаки
            Sounds.Play("Melee"); // Проиграть звук атаки
        }

        // Вызывается при нажатии клавиши атаки
        public void OnAttackKey()
        {
            _attackRange.Check(); // Проверить на попадание в область атаки
        }

    }
}
