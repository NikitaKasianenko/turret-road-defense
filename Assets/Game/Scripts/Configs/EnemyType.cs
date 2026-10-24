using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(fileName = "EnemyType", menuName = "Game/Enemy Type")]
    public class EnemyType : ScriptableObject
    {
        [field: SerializeField, Min(1f)] public float MaxHealth { get; private set; } = 30f;
        [field: SerializeField, Min(0f), Tooltip("Keep below car speed: enemies the car has passed fall behind instead of attacking from the turret's blind spot")]
        public float RunSpeed { get; private set; } = 5.5f;
        [field: SerializeField, Min(0f)] public float AttackDamage { get; private set; } = 15f;
        [field: SerializeField, Min(0)] public int ScoreReward { get; private set; } = 10;

        [field: Header("Look")]
        [field: SerializeField, Min(0.1f)] public float Scale { get; private set; } = 1f;
        [field: SerializeField] public Color Tint { get; private set; } = Color.white;
    }
}
