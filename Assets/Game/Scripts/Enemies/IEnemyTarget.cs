using Game.Combat;
using UnityEngine;

namespace Game.Enemies
{
    public interface IEnemyTarget : IDamageable
    {
        Vector3 Position { get; }
        bool IsTargetable { get; }

        Vector3 ClosestPoint(Vector3 point);
    }
}
