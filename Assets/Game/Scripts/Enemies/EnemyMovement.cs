using Game.Configs;
using UnityEngine;
using Zenject;

namespace Game.Enemies
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private EnemyAnimator _animator;

        private EnemyConfig _config;
        private float _runSpeed;

        [Inject]
        public void Construct(EnemyConfig config) =>
            _config = config;

        public void Setup(float runSpeed) =>
            _runSpeed = runSpeed;

        public void Walk(Vector3 direction) =>
            Move(direction, _config.WalkSpeed);

        public void Run(Vector3 direction) =>
            Move(direction, _runSpeed);

        public void Stop() =>
            _animator.SetSpeed(0f);

        private void Move(Vector3 direction, float speed)
        {
            transform.position += direction * (speed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction),
                _config.TurnSpeed * Time.deltaTime);

            _animator.SetSpeed(speed / _runSpeed);
        }
    }
}
