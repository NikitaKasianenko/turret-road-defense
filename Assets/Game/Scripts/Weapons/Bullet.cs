using Game.Combat;
using Game.Configs;
using Game.Effects;
using UnityEngine;
using Zenject;

namespace Game.Weapons
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private TrailRenderer _trail;
        [SerializeField] private LayerMask _hitMask;

        private TurretConfig _config;
        private ImpactEffect _impactEffect;
        private Pool _pool;

        private Vector3 _direction;
        private float _lifetimeLeft;

        [Inject]
        public void Construct(TurretConfig config, ImpactEffect impactEffect)
        {
            _config = config;
            _impactEffect = impactEffect;
        }

        private void Update()
        {
            float distance = _config.BulletSpeed * Time.deltaTime;

            if (Physics.SphereCast(transform.position, _config.BulletRadius, _direction, out RaycastHit hit, distance,
                    _hitMask, QueryTriggerInteraction.Ignore))
            {
                OnHit(hit);
                return;
            }

            transform.position += _direction * distance;

            _lifetimeLeft -= Time.deltaTime;
            if (_lifetimeLeft <= 0f)
                _pool.Despawn(this);
        }

        private void Launch(Vector3 position, Vector3 direction)
        {
            _direction = direction;
            _lifetimeLeft = _config.BulletLifetime;

            transform.SetPositionAndRotation(position, Quaternion.LookRotation(direction));
            _trail.Clear();
        }

        private void OnHit(RaycastHit hit)
        {
            if (hit.collider.TryGetComponent(out IDamageable target))
                target.TakeDamage(_config.BulletDamage);

            _impactEffect.Play(hit.point, hit.normal);
            _pool.Despawn(this);
        }

        public class Pool : MonoMemoryPool<Vector3, Vector3, Bullet>
        {
            protected override void OnCreated(Bullet bullet)
            {
                base.OnCreated(bullet);
                bullet._pool = this;
            }

            protected override void Reinitialize(Vector3 position, Vector3 direction, Bullet bullet) =>
                bullet.Launch(position, direction);
        }
    }
}
