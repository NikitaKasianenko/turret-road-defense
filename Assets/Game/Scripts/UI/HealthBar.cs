using DG.Tweening;
using Game.Combat;
using Game.Vehicle;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image _fill;
        [SerializeField] private Image _trail;
        [SerializeField, Min(0f)] private float _fillDuration = 0.15f;
        [SerializeField, Min(0f)] private float _trailDelay = 0.3f;
        [SerializeField, Min(0f)] private float _trailDuration = 0.4f;

        private IReadOnlyHealth _health;

        [Inject]
        public void Construct(Car car) =>
            _health = car.Health;

        private void Start()
        {
            _health.Changed += OnHealthChanged;
            SetInstant(_health.Normalized);
        }

        private void OnDestroy() =>
            _health.Changed -= OnHealthChanged;

        private void OnHealthChanged()
        {
            float value = _health.Normalized;

            if (value >= _fill.fillAmount)
            {
                SetInstant(value);
                return;
            }

            _fill.DOKill();
            _fill.DOFillAmount(value, _fillDuration);

            _trail.DOKill();
            _trail.DOFillAmount(value, _trailDuration).SetDelay(_trailDelay);
        }

        private void SetInstant(float value)
        {
            _fill.DOKill();
            _trail.DOKill();
            _fill.fillAmount = value;
            _trail.fillAmount = value;
        }
    }
}
