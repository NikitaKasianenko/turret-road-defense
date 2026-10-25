using DG.Tweening;
using Game.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _group;
        [SerializeField] private Image _fill;
        [SerializeField] private Image _trail;
        [SerializeField, Tooltip("Stays hidden until the first damage, hides again at zero; a hidden bar is deactivated so its canvas costs nothing")]
        private bool _hideUntilDamaged;
        [SerializeField, Min(0f)] private float _fadeDuration = 0.15f;
        [SerializeField, Min(0f)] private float _fillDuration = 0.15f;
        [SerializeField, Min(0f)] private float _trailDelay = 0.3f;
        [SerializeField, Min(0f)] private float _trailDuration = 0.4f;

        private IReadOnlyHealth _health;

        public void Bind(IReadOnlyHealth health)
        {
            Unbind();
            _health = health;
            _health.Changed += OnHealthChanged;

            SetInstant(_health.Normalized);
            _group.DOKill();
            _group.alpha = _hideUntilDamaged ? 0f : 1f;
            gameObject.SetActive(!_hideUntilDamaged);
        }

        private void OnDestroy() =>
            Unbind();

        private void Unbind()
        {
            if (_health != null)
                _health.Changed -= OnHealthChanged;
        }

        private void OnHealthChanged()
        {
            float value = _health.Normalized;

            if (_hideUntilDamaged)
                Fade(isVisible: value > 0f && value < 1f);

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

        private void Fade(bool isVisible)
        {
            float alpha = isVisible ? 1f : 0f;
            if (Mathf.Approximately(_group.alpha, alpha))
                return;

            _group.DOKill();
            gameObject.SetActive(true);
            Tween fade = _group.DOFade(alpha, _fadeDuration);

            if (!isVisible)
                fade.OnComplete(() => gameObject.SetActive(false));
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
