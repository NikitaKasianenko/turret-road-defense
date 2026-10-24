using DG.Tweening;
using Game.Configs;
using UnityEngine;
using Zenject;

namespace Game.Weapons
{
    public class TurretGun : MonoBehaviour
    {
        [SerializeField] private Transform _muzzle;
        [SerializeField] private ParticleSystem _muzzleFlash;
        [SerializeField] private Transform _recoilTarget;

        private TurretConfig _config;
        private Bullet.Pool _bulletPool;
        private float _cooldown;

        [Inject]
        public void Construct(TurretConfig config, Bullet.Pool bulletPool)
        {
            _config = config;
            _bulletPool = bulletPool;
        }

        private void Update()
        {
            _cooldown -= Time.deltaTime;
            if (_cooldown > 0f)
                return;

            Fire();
            _cooldown = 1f / _config.FireRate;
        }

        private void Fire()
        {
            _bulletPool.Spawn(_muzzle.position, _muzzle.forward);
            _muzzleFlash.Play();

            _recoilTarget.DOKill(complete: true);
            _recoilTarget.DOPunchPosition(Vector3.back * _config.RecoilDistance, 1f / _config.FireRate, vibrato: 1, elasticity: 0f);
        }
    }
}
