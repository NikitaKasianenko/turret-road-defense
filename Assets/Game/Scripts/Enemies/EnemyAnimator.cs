using UnityEngine;

namespace Game.Enemies
{
    public class EnemyAnimator : MonoBehaviour
    {
        private static readonly int SpeedHash = Animator.StringToHash("Speed");
        private static readonly int LocomotionScaleHash = Animator.StringToHash("LocomotionScale");
        private static readonly int AttackHash = Animator.StringToHash("Attack");
        private static readonly int IsDeadHash = Animator.StringToHash("IsDead");

        [SerializeField] private Animator _animator;
        [SerializeField, Min(0f)] private float _speedDampTime = 0.15f;
        [SerializeField, Min(0.01f), Tooltip("Ground speed (m/s) the Run clip is authored for; must match the Run threshold in AC_Stickman")]
        private float _runClipSpeed = 1.5f;
        [SerializeField, Min(1f), Tooltip("Faster movement plays the run cycle faster, up to this factor")]
        private float _maxLocomotionScale = 2.5f;

        public void SetSpeed(float metersPerSecond)
        {
            float locomotionScale = Mathf.Clamp(metersPerSecond / _runClipSpeed, 1f, _maxLocomotionScale);

            _animator.SetFloat(SpeedHash, metersPerSecond, _speedDampTime, Time.deltaTime);
            _animator.SetFloat(LocomotionScaleHash, locomotionScale, _speedDampTime, Time.deltaTime);
        }

        public void PlayAttack() =>
            _animator.SetTrigger(AttackHash);

        public void PlayDeath() =>
            _animator.SetBool(IsDeadHash, true);

        public void ResetState()
        {
            _animator.Rebind();
            _animator.Update(0f);

            int stateHash = _animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
            _animator.Play(stateHash, 0, Random.value);
        }
    }
}
