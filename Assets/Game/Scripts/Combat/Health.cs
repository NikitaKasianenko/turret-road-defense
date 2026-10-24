using System;
using UnityEngine;

namespace Game.Combat
{
    public class Health : IReadOnlyHealth
    {
        public event Action Changed;
        public event Action Died;

        public float Current { get; private set; }
        public float Max { get; }
        public float Normalized => Current / Max;
        public bool IsDead => Current <= 0f;

        public Health(float max)
        {
            Max = max;
            Current = max;
        }

        public void TakeDamage(float damage)
        {
            if (IsDead || damage <= 0f)
                return;

            Current = Mathf.Max(Current - damage, 0f);
            Changed?.Invoke();

            if (IsDead)
                Died?.Invoke();
        }

        public void Restore()
        {
            Current = Max;
            Changed?.Invoke();
        }
    }
}
