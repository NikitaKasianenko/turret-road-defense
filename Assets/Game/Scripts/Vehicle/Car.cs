using Game.Combat;
using Game.Configs;
using Game.Enemies;
using Game.Level;
using UnityEngine;
using Zenject;

namespace Game.Vehicle
{
    public class Car : MonoBehaviour, IEnemyTarget, IResettable
    {
        [SerializeField] private CarMovement _movement;
        [SerializeField] private CarFeedback _feedback;
        [SerializeField] private Collider _body;

        private Health _health;
        private Vector3 _startPosition;
        private bool _isActive;

        public IReadOnlyHealth Health => _health;
        public Vector3 Position => transform.position;
        public bool IsTargetable => _isActive && !_health.IsDead;

        [Inject]
        public void Construct(CarConfig config) =>
            _health = new Health(config.MaxHealth);

        private void Awake() =>
            _startPosition = transform.position;

        public void ResetState()
        {
            _isActive = false;
            _health.Restore();
            _movement.Warp(_startPosition);
            _feedback.ResetVisual();
        }

        public void StartDriving()
        {
            _isActive = true;
            _movement.Accelerate();
        }

        public void Stop()
        {
            _isActive = false;
            _movement.Brake();
        }

        public void TakeDamage(float damage)
        {
            if (!IsTargetable)
                return;

            _health.TakeDamage(damage);

            if (_health.IsDead)
                _feedback.PlayDestroyed();
            else
                _feedback.PlayHit();
        }

        public Vector3 ClosestPoint(Vector3 point) =>
            _body.ClosestPoint(point);
    }
}
