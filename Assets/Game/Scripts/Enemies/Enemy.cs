using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Combat;
using Game.Configs;
using Game.Vehicle;
using UnityEngine;
using Zenject;

namespace Game.Enemies
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        private enum State
        {
            Idle,
            Chase,
            Dead
        }

        [SerializeField] private EnemyAnimator _animator;
        [SerializeField] private EnemyFeedback _feedback;
        [SerializeField] private Collider _collider;

        private EnemyConfig _config;
        private Car _car;
        private Health _health;

        private State _state;
        private float _attackCooldown;

        public event Action<Enemy> Died;

        [Inject]
        public void Construct(EnemyConfig config, Car car)
        {
            _config = config;
            _car = car;
            _health = new Health(config.MaxHealth);
        }

        private void Update()
        {
            switch (_state)
            {
                case State.Idle:
                    UpdateIdle();
                    break;
                case State.Chase:
                    UpdateChase();
                    break;
            }
        }

        public void TakeDamage(float damage)
        {
            if (_state == State.Dead)
                return;

            _health.TakeDamage(damage);

            if (_health.IsDead)
                Die();
            else
                _feedback.PlayHit();
        }

        public UniTask PlayDeathAsync(CancellationToken cancellationToken) =>
            _feedback.PlayDeathAsync(FlatDirectionFrom(_car.Position), cancellationToken);

        private void Respawn(Vector3 position, Quaternion rotation)
        {
            transform.SetPositionAndRotation(position, rotation);

            _health.Restore();
            _state = State.Idle;
            _collider.enabled = true;

            _feedback.ResetVisual();
            _animator.ResetState();
        }

        private void UpdateIdle()
        {
            _animator.SetSpeed(0f);

            if (_car.IsTargetable && FlatDistance(_car.Position) <= _config.AggroDistance)
                StartChase();
        }

        private void StartChase()
        {
            _state = State.Chase;
            _attackCooldown = _config.AttackInterval * 0.5f;
        }

        private void UpdateChase()
        {
            if (!_car.IsTargetable)
            {
                _state = State.Idle;
                return;
            }

            Vector3 targetPoint = _car.ClosestPoint(transform.position);
            Vector3 toTarget = targetPoint - transform.position;
            toTarget.y = 0f;
            float distance = toTarget.magnitude;

            float maxStep = _config.RunSpeed * Time.deltaTime;
            float moveDistance = Mathf.Min(maxStep, Mathf.Max(distance - _config.AttackRange, 0f));
            Vector3 direction = distance > Mathf.Epsilon ? toTarget / distance : transform.forward;

            transform.position += direction * moveDistance;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction),
                _config.TurnSpeed * Time.deltaTime);

            _animator.SetSpeed(maxStep > 0f ? moveDistance / maxStep : 0f);

            if (distance <= _config.AttackRange)
                TickAttack();
        }

        private void TickAttack()
        {
            _attackCooldown -= Time.deltaTime;
            if (_attackCooldown > 0f)
                return;

            _attackCooldown = _config.AttackInterval;
            _animator.PlayAttack();
            _car.TakeDamage(_config.AttackDamage);
        }

        private void Die()
        {
            _state = State.Dead;
            _collider.enabled = false;
            _animator.PlayDeath();
            Died?.Invoke(this);
        }

        private float FlatDistance(Vector3 point)
        {
            Vector3 offset = point - transform.position;
            offset.y = 0f;
            return offset.magnitude;
        }

        private Vector3 FlatDirectionFrom(Vector3 point)
        {
            Vector3 direction = transform.position - point;
            direction.y = 0f;
            return direction.normalized;
        }

        public class Pool : MonoMemoryPool<Vector3, Quaternion, Enemy>
        {
            protected override void Reinitialize(Vector3 position, Quaternion rotation, Enemy enemy) =>
                enemy.Respawn(position, rotation);
        }
    }
}
