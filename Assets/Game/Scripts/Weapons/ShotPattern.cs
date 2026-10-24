using UnityEngine;

namespace Game.Weapons
{
    public abstract class ShotPattern : ScriptableObject
    {
        public abstract void Fire(Vector3 origin, Vector3 direction, Bullet.Pool bulletPool);
    }
}
