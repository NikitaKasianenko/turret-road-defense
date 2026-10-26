using DG.Tweening;
using Game.Audio;
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
        [SerializeField] private SoundEffect _shotSound;

        private TurretConfig _config;
        private IInputService _input;
        private TurretHeat _heat;
        private PowerUpService _powerUps;
        private Bullet.Pool _bulletPool;
        private IAudioService _audio;
        private float _cooldown;

        [Inject]
        public void Construct(TurretConfig config, IInputService input, TurretHeat heat, PowerUpService powerUps,
            Bullet.Pool bulletPool, IAudioService audio)
        {
            _config = config;
            _input = input;
            _heat = heat;
            _powerUps = powerUps;
            _bulletPool = bulletPool;
            _audio = audio;
        }

        private void Update()
        {
            _cooldown -= Time.deltaTime;

            bool ignoresHeat = _powerUps.IgnoresHeat;
            bool canFire = _input.IsPointerHeld && (ignoresHeat || !_heat.IsOverheated);

            if (!canFire || ignoresHeat)
                _heat.Cool(Time.deltaTime);

            if (!canFire || _cooldown > 0f)
                return;

            Fire();
            if (!ignoresHeat)
                _heat.AddShot();

            _cooldown = 1f / (_config.FireRate * _powerUps.FireRateMultiplier);
        }

        private void Fire()
        {
            _powerUps.ShotPattern.Fire(_muzzle.position, _muzzle.forward, _bulletPool);
            _muzzleFlash.Play();
            _audio.Play(_shotSound);

            _recoilTarget.DOKill(complete: true);
            _recoilTarget.DOPunchPosition(Vector3.back * _config.RecoilDistance, 1f / _config.FireRate, vibrato: 1, elasticity: 0f);
        }
    }
}
