using Game.Effects;
using Game.Vehicle;
using UnityEngine;
using Zenject;

namespace Game.PowerUps
{
    public class PowerUpOrb : MonoBehaviour
    {
        [SerializeField] private RendererTint _tint;
        [SerializeField] private TrailRenderer _trail;
        [SerializeField, Min(0.1f)] private float _flightDuration = 0.7f;
        [SerializeField] private float _arcHeight = 3f;
        [SerializeField] private float _targetHeight = 1.5f;

        private Car _car;
        private PowerUpService _powerUps;
        private Pool _pool;

        private PowerUp _powerUp;
        private Vector3 _startPosition;
        private float _elapsed;

        [Inject]
        public void Construct(Car car, PowerUpService powerUps)
        {
            _car = car;
            _powerUps = powerUps;
        }

        private void Update()
        {
            _elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(_elapsed / _flightDuration);

            Vector3 end = _car.Position + Vector3.up * _targetHeight;
            Vector3 position = Vector3.Lerp(_startPosition, end, progress * progress);
            position.y += Mathf.Sin(progress * Mathf.PI) * _arcHeight;
            transform.position = position;

            if (progress < 1f)
                return;

            _powerUps.Activate(_powerUp);
            _pool.Despawn(this);
        }

        private void Launch(Vector3 position, PowerUp powerUp)
        {
            _powerUp = powerUp;
            _startPosition = position;
            _elapsed = 0f;

            transform.position = position;
            _tint.Apply(powerUp.Color);
            _trail.startColor = powerUp.Color;
            _trail.Clear();
        }

        public class Pool : MonoMemoryPool<Vector3, PowerUp, PowerUpOrb>
        {
            protected override void OnCreated(PowerUpOrb orb)
            {
                base.OnCreated(orb);
                orb._pool = this;
            }

            protected override void Reinitialize(Vector3 position, PowerUp powerUp, PowerUpOrb orb) =>
                orb.Launch(position, powerUp);
        }
    }
}
