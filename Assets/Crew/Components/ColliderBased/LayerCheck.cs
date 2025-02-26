using UnityEngine;

namespace Crew.Components.ColliderBased
{
    public class LayerCheck : MonoBehaviour
    {
        [SerializeField] protected LayerMask _layer; // Слой для проверки.
        [SerializeField] protected bool _isTouchingLayer; // Флаг, указывающий, находится ли объект на слое.

        /// <summary>
        /// Возвращает значение, указывающее, находится ли объект на слое.
        /// </summary>
        public bool IsTouchingLayer => _isTouchingLayer;
    }
}
