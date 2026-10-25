using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine.Jobs;

namespace Game.Enemies
{
    [BurstCompile]
    public struct EnemyMotionJob : IJobParallelForTransform
    {
        public NativeArray<EnemyMotion> Motions;
        public float3 TargetPosition;
        public float DeltaTime;
        public float Acceleration;
        public float MaxTurnAngle;

        public void Execute(int index, TransformAccess transform)
        {
            EnemyMotion motion = Motions[index];
            float3 position = transform.position;

            float3 toTarget = TargetPosition - position;
            toTarget.y = 0f;
            motion.DistanceToTarget = math.length(toTarget);

            motion.CurrentSpeed = MoveTowards(motion.CurrentSpeed, motion.TargetSpeed, Acceleration * DeltaTime);
            transform.position = position + motion.Direction * (motion.CurrentSpeed * DeltaTime);

            quaternion look = quaternion.LookRotationSafe(motion.Direction, math.up());
            transform.rotation = RotateTowards(transform.rotation, look, MaxTurnAngle);

            Motions[index] = motion;
        }

        private static float MoveTowards(float current, float target, float maxDelta) =>
            current + math.clamp(target - current, -maxDelta, maxDelta);

        private static quaternion RotateTowards(quaternion from, quaternion to, float maxAngle)
        {
            float angle = 2f * math.acos(math.min(math.abs(math.dot(from.value, to.value)), 1f));
            return angle <= maxAngle ? to : math.slerp(from, to, maxAngle / angle);
        }
    }
}
