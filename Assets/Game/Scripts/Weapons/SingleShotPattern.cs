using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu(fileName = "SingleShot", menuName = "Game/Shot Patterns/Single")]
    public class SingleShotPattern : ShotPattern
    {
        public override void Fire(Vector3 origin, Vector3 direction, Bullet.Pool bulletPool) =>
            bulletPool.Spawn(origin, direction);
    }
}
