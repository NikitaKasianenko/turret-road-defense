using DG.Tweening;
using Game.Audio;
using Game.Effects;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace Game.Vehicle
{
    public class CarFeedback : MonoBehaviour
    {
        [SerializeField] private DamageFlash _damageFlash;
        [SerializeField] private CinemachineImpulseSource _impulseSource;
        [SerializeField] private Transform _body;

        [Header("Hit")]
        [SerializeField] private SoundEffect _hitSound;
        [SerializeField] private float _punchStrength = 0.06f;
        [SerializeField] private float _punchDuration = 0.2f;

        [Header("Destroy")]
        [SerializeField] private SoundEffect _explosionSound;
        [SerializeField] private CarWreck _wreck;
        [SerializeField] private ParticleSystem _explosion;
        [SerializeField] private ParticleSystem _smoke;
        [SerializeField] private float _destroyImpulseForce = 4f;
        [SerializeField] private float _destroyJumpPower = 1.2f;
        [SerializeField] private float _destroyDuration = 0.7f;
        [SerializeField] private Vector3 _destroyTilt = new(-8f, 0f, 18f);

        private IAudioService _audio;

        [Inject]
        public void Construct(IAudioService audio) =>
            _audio = audio;

        public void PlayHit()
        {
            _damageFlash.Play();
            _impulseSource.GenerateImpulse();
            _audio.Play(_hitSound);

            _body.DOKill(complete: true);
            _body.DOPunchScale(Vector3.one * _punchStrength, _punchDuration, vibrato: 6);
        }

        public void PlayDestroyed()
        {
            _damageFlash.Play();
            _impulseSource.GenerateImpulseWithForce(_destroyImpulseForce);
            _audio.Play(_explosionSound);
            _explosion.Play();
            _smoke.Play();
            _wreck.Scatter();

            _body.DOKill(complete: true);
            _body.DOLocalJump(Vector3.zero, _destroyJumpPower, numJumps: 1, _destroyDuration);
            _body.DOLocalRotate(_destroyTilt, _destroyDuration).SetEase(Ease.OutBack);
        }

        public void ResetVisual()
        {
            _body.DOKill();
            _body.localPosition = Vector3.zero;
            _body.localRotation = Quaternion.identity;
            _body.localScale = Vector3.one;
            _wreck.Restore();

            _explosion.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmittingAndClear);
            _smoke.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmittingAndClear);
            _damageFlash.ResetFlash();
        }
    }
}
