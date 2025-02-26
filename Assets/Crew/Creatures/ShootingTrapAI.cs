using System;
using Crew.Components.Audio;
using Crew.Components.ColliderBased;
using Crew.Components.GoGameObjects;
using Crew.Utils;
using UnityEngine;

namespace Crew.Creatures
{
    public class ShootingTrapAI : MonoBehaviour
    {
        [SerializeField] private ColliderCheck _vision;

        [Header("Range")]
        [SerializeField] private Cooldown _rangeCooldown;
        [SerializeField] private SpawnComponent _rangeAttack;

        protected PlaySoundsComponent Sounds; // Компонент для проигрывания звуков
        private static readonly int Range = Animator.StringToHash("attack"); //range

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            Sounds = GetComponent<PlaySoundsComponent>();
        }

        private void Update()
        {
            if (_vision.IsTouchingLayer)
            {
                if (_rangeCooldown.IsReady)
                {
                    RangeAttack();
                }
            }
        }

        private void RangeAttack()
        {
            _rangeCooldown.Reset();
            _animator.SetTrigger(Range);
        }

        public void OnRangeAttack()
        {
            _rangeAttack.Spawn();
            Sounds.Play("Range"); // Проиграть звук атаки
        }
    }
}
