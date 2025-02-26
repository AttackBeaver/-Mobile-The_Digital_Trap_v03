using UnityEngine;

namespace Crew
{
    public class ParallaxEffect : MonoBehaviour
    {
        // Начальная позиция объекта и длина его спрайта.
        private float _startingPos, _lengthOfSprite;
        
        // Уровень параллакса и ссылка на главную камеру.
        public float AmountOfParallax;
        public Camera MainCamera;

        // Вызывается при запуске сцены.
        private void Start()
        {
            // Установка начальной позиции и длины спрайта объекта.
            _startingPos = transform.position.x;
            _lengthOfSprite = GetComponent<SpriteRenderer>().bounds.size.x;
        }

        // Вызывается на каждый кадр.
        private void FixedUpdate()
        {
            // Получение позиции камеры и вычисление временной позиции объекта.
            Vector3 Position = MainCamera.transform.position;
            float Temp = Position.x * (1 - AmountOfParallax);
            float Distance = Position.x * AmountOfParallax;

            // Новая позиция объекта с учетом параллакс-эффекта.
            Vector3 NewPosition = new Vector3(_startingPos + Distance, transform.position.y, transform.position.z);

            // Установка новой позиции объекта.
            transform.position = NewPosition;

            // Перемещение объекта при достижении границы спрайта.
            if (Temp > _startingPos + (_lengthOfSprite / 2))
            {
                _startingPos += _lengthOfSprite;
            }
            else if (Temp < _startingPos - (_lengthOfSprite / 2))
            {
                _startingPos -= _lengthOfSprite;
            }
        }
    }
}
