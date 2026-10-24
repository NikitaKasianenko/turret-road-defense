using Game.Weapons;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.UI
{
    public class HeatBar : MonoBehaviour
    {
        [SerializeField] private Image _fill;
        [SerializeField] private Gradient _heatColor;
        [SerializeField] private Color _overheatColor = new(1f, 0.15f, 0.1f);
        [SerializeField, Min(0.1f)] private float _overheatBlinkRate = 8f;

        private TurretHeat _heat;

        [Inject]
        public void Construct(TurretHeat heat) =>
            _heat = heat;

        private void LateUpdate()
        {
            _fill.fillAmount = _heat.Value;

            if (_heat.IsOverheated)
            {
                bool isBright = Mathf.PingPong(Time.unscaledTime * _overheatBlinkRate, 1f) > 0.5f;
                _fill.color = isBright ? _overheatColor : Color.white;
            }
            else
            {
                _fill.color = _heatColor.Evaluate(_heat.Value);
            }
        }
    }
}
