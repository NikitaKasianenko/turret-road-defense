using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Audio;
using Game.Effects;
using UnityEngine;
using Zenject;

namespace Game.Enemies
{
    public class EnemyFeedback : MonoBehaviour
    {
        [SerializeField] private Transform _model;
        [SerializeField] private DamageFlash _damageFlash;

        [Header("Hit")]
        [SerializeField] private SoundEffect _hitSound;
        [SerializeField] private float _hitPunchStrength = 0.15f;
        [SerializeField] private float _hitPunchDuration = 0.15f;

        [Header("Death")]
        [SerializeField] private SoundEffect _deathSound;
        [SerializeField] private float _knockbackDistance = 2.5f;
        [SerializeField] private float _knockbackHeight = 1f;
        [SerializeField] private float _knockbackDuration = 0.5f;
        [SerializeField, Tooltip("Lets the death clip play out before the body shrinks")] private float _lyingDuration = 1f;
        [SerializeField] private float _shrinkDuration = 0.25f;

        [Header("Vanish")]
        [SerializeField, Tooltip("Lets the attack clip land before the enemy disappears")] private float _vanishDelay = 0.35f;
        [SerializeField] private SoundEffect _vanishSound;
        [SerializeField] private ParticleSystem _vanishPuff;
        [SerializeField] private float _vanishSwellScale = 1.25f;
        [SerializeField] private float _vanishSwellDuration = 0.1f;
        [SerializeField] private float _vanishShrinkDuration = 0.15f;
        [SerializeField, Tooltip("Keeps the object alive until the puff particles fade out")] private float _vanishTail = 0.5f;

        private IAudioService _audio;

        [Inject]
        public void Construct(IAudioService audio) =>
            _audio = audio;

        public void PlayHit()
        {
            _damageFlash.Play();
            _audio.Play(_hitSound);

            _model.DOKill(complete: true);
            _model.DOPunchScale(Vector3.one * _hitPunchStrength, _hitPunchDuration, vibrato: 6);
        }

        public UniTask PlayDeathAsync(Vector3 knockbackDirection, CancellationToken cancellationToken)
        {
            _damageFlash.Play();
            _audio.Play(_deathSound);
            _model.DOKill(complete: true);

            Vector3 landingPoint = transform.position + knockbackDirection * _knockbackDistance;

            Sequence sequence = DOTween.Sequence()
                .Append(transform.DOJump(landingPoint, _knockbackHeight, numJumps: 1, _knockbackDuration))
                .AppendInterval(_lyingDuration)
                .Append(_model.DOScale(Vector3.zero, _shrinkDuration).SetEase(Ease.InBack));

            return sequence.ToUniTask(TweenCancelBehaviour.Kill, cancellationToken);
        }

        public UniTask PlayVanishAsync(CancellationToken cancellationToken)
        {
            _damageFlash.Play();
            _model.DOKill(complete: true);

            Sequence sequence = DOTween.Sequence()
                .AppendInterval(_vanishDelay)
                .Append(_model.DOScale(Vector3.one * _vanishSwellScale, _vanishSwellDuration).SetEase(Ease.OutQuad))
                .AppendCallback(PlayVanishPuff)
                .Append(_model.DOScale(Vector3.zero, _vanishShrinkDuration).SetEase(Ease.InBack))
                .AppendInterval(_vanishTail);

            return sequence.ToUniTask(TweenCancelBehaviour.Kill, cancellationToken);
        }

        public void ResetVisual()
        {
            transform.DOKill();
            _model.DOKill();

            _model.localPosition = Vector3.zero;
            _model.localRotation = Quaternion.identity;
            _model.localScale = Vector3.one;
            _damageFlash.ResetFlash();
            _vanishPuff.Stop(withChildren: true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        private void PlayVanishPuff()
        {
            _vanishPuff.Play();
            _audio.Play(_vanishSound);
        }
    }
}
