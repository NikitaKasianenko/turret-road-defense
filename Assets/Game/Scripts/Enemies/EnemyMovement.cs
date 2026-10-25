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
        private float _currentSpeed;
        private Vector3 _direction;

        [Inject]
        public void Construct(EnemyConfig config) =>
            _config = config;

        public void Setup(float runSpeed)
        {
            _runSpeed = runSpeed;
            _currentSpeed = 0f;
            _direction = transform.forward;
        }

        public void Walk(Vector3 direction) =>
            Move(direction, _config.WalkSpeed);

        public void Run(Vector3 direction) =>
            Move(direction, _runSpeed);

        public void Stop() =>
            Move(_direction, 0f);

        private void Move(Vector3 direction, float targetSpeed)
        {
            _direction = direction;
            _currentSpeed = Mathf.MoveTowards(_currentSpeed, targetSpeed, _config.Acceleration * Time.deltaTime);

            transform.position += direction * (_currentSpeed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction),
                _config.TurnSpeed * Time.deltaTime);

            _animator.SetSpeed(_currentSpeed);
        }
    }
}
