using System;
using Game.Weapons;
using UnityEngine;

namespace Game.Configs
{
    [Serializable]
    public class TurretConfig
    {
        [field: Header("Aim")]
        [field: SerializeField, Range(0f, 180f)] public float MaxYawAngle { get; private set; } = 75f;
        [field: SerializeField, Min(0f)] public float AimSmoothTime { get; private set; } = 0.06f;

        [field: Header("Fire")]
        [field: SerializeField] public ShotPattern DefaultShotPattern { get; private set; }
        [field: SerializeField, Min(0.1f), Tooltip("Shots per second")] public float FireRate { get; private set; } = 7f;
        [field: SerializeField, Min(0f)] public float RecoilDistance { get; private set; } = 0.12f;

        [field: Header("Heat")]
        [field: SerializeField, Range(0f, 1f)] public float HeatPerShot { get; private set; } = 0.022f;
        [field: SerializeField, Min(0f), Tooltip("Heat lost per second while not firing")] public float CoolingRate { get; private set; } = 0.45f;
        [field: SerializeField, Range(0f, 1f), Tooltip("After overheating, the gun fires again once cooled down to this level")]
        public float OverheatRecoverLevel { get; private set; } = 0.35f;

        [field: Header("Bullet")]
        [field: SerializeField, Min(0f)] public float BulletDamage { get; private set; } = 10f;
        [field: SerializeField, Min(0.1f)] public float BulletSpeed { get; private set; } = 45f;
        [field: SerializeField, Min(0.1f)] public float BulletLifetime { get; private set; } = 1.2f;
        [field: SerializeField, Min(0f)] public float BulletRadius { get; private set; } = 0.25f;
    }
}
