using DG.Tweening;
using Game.PowerUps;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.UI
{
    public class PowerUpView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _group;
        [SerializeField] private TMP_Text _label;
        [SerializeField] private Image _timer;
        [SerializeField, Min(0f)] private float _fadeDuration = 0.2f;

        private PowerUpService _powerUps;

        [Inject]
        public void Construct(PowerUpService powerUps) =>
            _powerUps = powerUps;

        private void Start()
        {
            _group.alpha = 0f;
            _powerUps.Changed += Refresh;
        }

        private void OnDestroy() =>
            _powerUps.Changed -= Refresh;

        private void LateUpdate() =>
            _timer.fillAmount = _powerUps.NormalizedTimeLeft;

        private void Refresh()
        {
            PowerUp active = _powerUps.Active;
            _group.DOKill();
            _group.DOFade(active != null ? 1f : 0f, _fadeDuration);

            if (active == null)
                return;

            _label.text = active.DisplayName.ToUpperInvariant();
            _label.color = active.Color;
            _timer.color = active.Color;
            _label.transform.DOKill(complete: true);
            _label.transform.DOPunchScale(Vector3.one * 0.3f, 0.25f, vibrato: 4);
        }
    }
}
