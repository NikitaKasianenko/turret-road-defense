using DG.Tweening;
using Game.Configs;
using Game.Input;
using Game.PowerUps;
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
        private IInputService _input;
        private TurretHeat _heat;
        private PowerUpService _powerUps;
        private Bullet.Pool _bulletPool;
        private float _cooldown;

        [Inject]
        public void Construct(TurretConfig config, IInputService input, TurretHeat heat, PowerUpService powerUps,
            Bullet.Pool bulletPool)
        {
            _config = config;
            _input = input;
            _heat = heat;
            _powerUps = powerUps;
            _bulletPool = bulletPool;
        }

        private void Update()
        {
            _cooldown -= Time.deltaTime;

            if (!_input.IsPointerHeld || _heat.IsOverheated)
            {
                _heat.Cool(Time.deltaTime);
                return;
            }

            if (_cooldown > 0f)
                return;

            Fire();
            _heat.AddShot();
            _cooldown = 1f / (_config.FireRate * _powerUps.FireRateMultiplier);
        }

        private void Fire()
        {
            _powerUps.ShotPattern.Fire(_muzzle.position, _muzzle.forward, _bulletPool);
            _muzzleFlash.Play();

            _recoilTarget.DOKill(complete: true);
            _recoilTarget.DOPunchPosition(Vector3.back * _config.RecoilDistance, 1f / _config.FireRate, vibrato: 1, elasticity: 0f);
        }
    }
}
