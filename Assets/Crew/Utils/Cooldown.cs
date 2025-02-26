using System;
using UnityEngine;

namespace Crew.Utils
{
    [Serializable]
    public class Cooldown
    {
        [SerializeField] private float _value;

        private float _timesUP;

        public float Value
        {
            get => _value;
            set => _value = value;
        }

        public void Reset()
        {
            _timesUP = Time.time + _value;
        }
        public float RemainingTime => Mathf.Max(_timesUP - Time.time, 0);
        public bool IsReady => _timesUP <= Time.time;
    }
}
