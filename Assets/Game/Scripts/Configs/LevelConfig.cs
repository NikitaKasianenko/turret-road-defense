using System;
using UnityEngine;

namespace Game.Configs
{
    [Serializable]
    public class LevelConfig
    {
        [field: SerializeField, Min(10f)] public float Length { get; private set; } = 300f;

        [field: Header("Ground")]
        [field: SerializeField, Min(1f)] public float GroundTileLength { get; private set; } = 75f;
        [field: SerializeField, Min(0f), Tooltip("Extra ground before start and after finish")] public float GroundPadding { get; private set; } = 75f;

        [field: Header("Enemies")]
        [field: SerializeField, Min(0)] public int EnemyCount { get; private set; } = 30;
        [field: SerializeField, Min(0f)] public float SpawnStartDistance { get; private set; } = 35f;
        [field: SerializeField, Min(0f)] public float SpawnEndOffset { get; private set; } = 15f;
        [field: SerializeField, Min(0f)] public float SpawnHalfWidth { get; private set; } = 8f;
    }
}
