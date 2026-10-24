using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu(fileName = "SpreadShot", menuName = "Game/Shot Patterns/Spread")]
    public class SpreadShotPattern : ShotPattern
    {
        [SerializeField, Min(2)] private int _bulletCount = 3;
        [SerializeField, Range(0f, 45f)] private float _angleStep = 12f;

        public override void Fire(Vector3 origin, Vector3 direction, Bullet.Pool bulletPool)
        {
            float firstAngle = -_angleStep * (_bulletCount - 1) * 0.5f;

            for (int i = 0; i < _bulletCount; i++)
            {
                Quaternion rotation = Quaternion.AngleAxis(firstAngle + _angleStep * i, Vector3.up);
                bulletPool.Spawn(origin, rotation * direction);
            }
        }
    }
}
