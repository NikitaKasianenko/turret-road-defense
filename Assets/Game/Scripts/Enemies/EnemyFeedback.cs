using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Effects;
using UnityEngine;

namespace Game.Enemies
{
    public class EnemyFeedback : MonoBehaviour
    {
        [SerializeField] private Transform _model;
        [SerializeField] private DamageFlash _damageFlash;

        [Header("Hit")]
        [SerializeField] private float _hitPunchStrength = 0.15f;
        [SerializeField] private float _hitPunchDuration = 0.15f;

        [Header("Death")]
        [SerializeField] private float _knockbackDistance = 2.5f;
        [SerializeField] private float _knockbackHeight = 1f;
        [SerializeField] private float _knockbackDuration = 0.5f;
        [SerializeField] private float _shrinkDuration = 0.25f;

        [Header("Vanish")]
        [SerializeField] private ParticleSystem _vanishPuff;
        [SerializeField] private float _vanishSwellScale = 1.25f;
        [SerializeField] private float _vanishSwellDuration = 0.1f;
        [SerializeField] private float _vanishShrinkDuration = 0.15f;
        [SerializeField, Tooltip("Keeps the object alive until the puff particles fade out")] private float _vanishTail = 0.5f;

        public void PlayHit()
        {
            _damageFlash.Play();

            _model.DOKill(complete: true);
            _model.DOPunchScale(Vector3.one * _hitPunchStrength, _hitPunchDuration, vibrato: 6);
        }

        public UniTask PlayDeathAsync(Vector3 knockbackDirection, CancellationToken cancellationToken)
        {
            _damageFlash.Play();
            _model.DOKill(complete: true);

            Vector3 landingPoint = transform.position + knockbackDirection * _knockbackDistance;

            Sequence sequence = DOTween.Sequence()
                .Join(transform.DOJump(landingPoint, _knockbackHeight, numJumps: 1, _knockbackDuration))
                .Join(_model.DOLocalRotate(new Vector3(-90f, 0f, 0f), _knockbackDuration).SetEase(Ease.OutQuad))
                .Append(_model.DOScale(Vector3.zero, _shrinkDuration).SetEase(Ease.InBack));

            return sequence.ToUniTask(TweenCancelBehaviour.Kill, cancellationToken);
        }

        public UniTask PlayVanishAsync(CancellationToken cancellationToken)
        {
            _damageFlash.Play();
            _model.DOKill(complete: true);

            Sequence sequence = DOTween.Sequence()
                .Append(_model.DOScale(Vector3.one * _vanishSwellScale, _vanishSwellDuration).SetEase(Ease.OutQuad))
                .AppendCallback(_vanishPuff.Play)
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
    }
}
