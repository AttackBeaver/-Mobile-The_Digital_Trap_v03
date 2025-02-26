using Crew.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Crew.Creatures.Hero
{
   public class HeroInputReader : MonoBehaviour
   {
      [SerializeField] private Hero _hero; // Ссылка на компонент героя.

      /// <summary>
      /// Метод, вызываемый при событии перемещения.
      /// </summary>
      /// <param name="context">Контекст события ввода.</param>
      public void OnMovement(InputAction.CallbackContext context)
      {
         var direction = context.ReadValue<Vector2>(); // Чтение направления движения из контекста события.
         _hero.SetDirection(direction); // Установка направления движения героя.
      }

      /// <summary>
      /// Метод, вызываемый при событии взаимодействия.
      /// </summary>
      /// <param name="context">Контекст события ввода.</param>
      public void OnInteract(InputAction.CallbackContext context)
      {
         if (context.performed)
         {
            _hero.Interact(); // Вызов метода взаимодействия героя.
         }
      }

      /// <summary>
      /// Метод, вызываемый при событии атаки.
      /// </summary>
      /// <param name="context">Контекст события ввода.</param>
      public void OnAttack(InputAction.CallbackContext context)
      {
         if (context.performed)
         {
            _hero.Attack(); // Вызов метода атаки героя.
         }
      }

      /// <summary>
      /// Метод, вызываемый при событии альтернативной атаки.
      /// </summary>
      /// <param name="context">Контекст события ввода.</param>
      public void OnAltAttack(InputAction.CallbackContext context)
      {
         if (context.performed)
         {
            _hero.AltAttack(); // Вызов метода альтернативной атаки героя.
         }
      }

      /// <summary>
      /// Метод, для вызова меню паузы
      /// </summary>
      /// <param name="context">Контекст события ввода</param>
      public void OnPause(InputAction.CallbackContext context)
      {
         if (context.performed)
         {
            WindowUtils.CreateWindow("UI/InGameMenuWindow");
         }
      }

      public void OnConsole(InputAction.CallbackContext context)
      {
         if (context.performed)
         {
            WindowUtils.CreateWindow("UI/Console");
         }
      }
   }
}
