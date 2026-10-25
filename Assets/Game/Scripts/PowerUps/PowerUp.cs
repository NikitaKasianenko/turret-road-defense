using Game.Weapons;
using UnityEngine;

namespace Game.PowerUps
{
    [CreateAssetMenu(fileName = "PowerUp", menuName = "Game/Power Up")]
    public class PowerUp : ScriptableObject
    {
        [field: SerializeField] public string DisplayName { get; private set; } = "Power Up";
        [field: SerializeField] public Color Color { get; private set; } = Color.cyan;
        [field: SerializeField] public ShotPattern ShotPattern { get; private set; }
        [field: SerializeField, Min(0.1f)] public float FireRateMultiplier { get; private set; } = 1f;
        [field: SerializeField, Min(0.1f)] public float Duration { get; private set; } = 6f;
        [field: SerializeField, Tooltip("The gun never overheats while this power-up is active")]
        public bool IgnoresHeat { get; private set; }
    }
}
