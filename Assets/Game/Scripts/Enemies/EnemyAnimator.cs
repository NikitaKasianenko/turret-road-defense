using UnityEngine;

namespace Game.Enemies
{
    public class EnemyAnimator : MonoBehaviour
    {
        private static readonly int SpeedHash = Animator.StringToHash("Speed");
        private static readonly int AttackHash = Animator.StringToHash("Attack");
        private static readonly int IsDeadHash = Animator.StringToHash("IsDead");

        [SerializeField] private Animator _animator;
        [SerializeField, Min(0f)] private float _speedDampTime = 0.1f;

        public void SetSpeed(float normalizedSpeed) =>
            _animator.SetFloat(SpeedHash, normalizedSpeed, _speedDampTime, Time.deltaTime);

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
