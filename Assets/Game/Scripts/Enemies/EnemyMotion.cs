using Unity.Mathematics;

namespace Game.Enemies
{
    public struct EnemyMotion
    {
        public float3 Direction;
        public float TargetSpeed;
        public float CurrentSpeed;
        public float DistanceToTarget;
    }
}
