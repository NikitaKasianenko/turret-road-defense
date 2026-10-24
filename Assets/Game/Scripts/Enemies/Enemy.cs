using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Combat;
using Game.Configs;
using Game.Effects;
using Game.Level;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Game.Enemies
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        private enum State
        {
            Idle,
            Wander,
            Chase,
            Finished
        }

        [SerializeField] private EnemyMovement _movement;
        [SerializeField] private EnemyAnimator _animator;
        [SerializeField] private EnemyFeedback _feedback;
        [SerializeField] private RendererTint _tint;
        [SerializeField] private Collider _collider;

        private EnemyConfig _config;
        private IEnemyTarget _target;
        private LevelSession _session;
        private Health _health;

        private State _state;
        private float _stateTimeLeft;
        private Vector3 _spawnPosition;
        private Vector3 _wanderDirection;

        public event Action<Enemy> Killed;
        public event Action<Enemy> Struck;

        public EnemyType Type { get; private set; }

        [Inject]
        public void Construct(EnemyConfig config, IEnemyTarget target, LevelSession session)
        {
            _config = config;
            _target = target;
            _session = session;
        }

        private void Update()
        {
            switch (_state)
            {
                case State.Idle:
                    UpdateIdle();
                    break;
                case State.Wander:
                    UpdateWander();
                    break;
                case State.Chase:
                    UpdateChase();
                    break;
            }
        }

        public void TakeDamage(float damage)
        {
            if (_state == State.Finished || !_session.IsRunning)
                return;

            _health.TakeDamage(damage);

            if (_health.IsDead)
                Die();
            else
                _feedback.PlayHit();
        }

        public UniTask PlayDeathAsync(CancellationToken cancellationToken) =>
            _feedback.PlayDeathAsync(FlatDirectionFrom(_target.Position), cancellationToken);

        public UniTask PlayVanishAsync(CancellationToken cancellationToken) =>
            _feedback.PlayVanishAsync(cancellationToken);

        private void Respawn(Vector3 position, Quaternion rotation, EnemyType type)
        {
            Type = type;
            _health = new Health(type.MaxHealth);
            _movement.Setup(type.RunSpeed);

            transform.SetPositionAndRotation(position, rotation);
            transform.localScale = Vector3.one * type.Scale;
            _spawnPosition = position;
            _collider.enabled = true;
            EnterIdle();

            _feedback.ResetVisual();
            _tint.Apply(type.Tint);
            _animator.ResetState();
        }

        private void EnterIdle()
        {
            _state = State.Idle;
            _stateTimeLeft = RandomInRange(_config.IdleDurationRange);
        }

        private void EnterWander()
        {
            _state = State.Wander;
            _stateTimeLeft = RandomInRange(_config.WanderDurationRange);

            Vector3 toSpawn = _spawnPosition - transform.position;
            toSpawn.y = 0f;
            Vector2 random = Random.insideUnitCircle.normalized;

            _wanderDirection = toSpawn.magnitude > _config.WanderRadius
                ? toSpawn.normalized
                : new Vector3(random.x, 0f, random.y);
        }

        private void UpdateIdle()
        {
            _movement.Stop();

            if (TryStartChase() || !IsStateTimeOver())
                return;

            if (Random.value < _config.WanderChance)
                EnterWander();
            else
                EnterIdle();
        }

        private void UpdateWander()
        {
            if (TryStartChase())
                return;

            _movement.Walk(_wanderDirection);

            if (IsStateTimeOver())
                EnterIdle();
        }

        private bool TryStartChase()
        {
            if (!_target.IsTargetable || FlatDistance(_target.Position) > _config.AggroDistance)
                return false;

            _state = State.Chase;
            return true;
        }

        private void UpdateChase()
        {
            if (!_target.IsTargetable)
            {
                EnterIdle();
                return;
            }

            Vector3 toTarget = _target.ClosestPoint(transform.position) - transform.position;
            toTarget.y = 0f;
            float distance = toTarget.magnitude;

            if (distance <= _config.AttackRange)
                Strike();
            else
                _movement.Run(toTarget / distance);
        }

        private void Strike()
        {
            Finish();
            _animator.PlayAttack();
            _target.TakeDamage(Type.AttackDamage);
            Struck?.Invoke(this);
        }

        private void Die()
        {
            Finish();
            _animator.PlayDeath();
            Killed?.Invoke(this);
        }

        private void Finish()
        {
            _state = State.Finished;
            _collider.enabled = false;
        }

        private bool IsStateTimeOver()
        {
            _stateTimeLeft -= Time.deltaTime;
            return _stateTimeLeft <= 0f;
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

        private static float RandomInRange(Vector2 range) =>
            Random.Range(range.x, range.y);

        public class Pool : MonoMemoryPool<Vector3, Quaternion, EnemyType, Enemy>
        {
            protected override void Reinitialize(Vector3 position, Quaternion rotation, EnemyType type, Enemy enemy) =>
                enemy.Respawn(position, rotation, type);
        }
    }
}
