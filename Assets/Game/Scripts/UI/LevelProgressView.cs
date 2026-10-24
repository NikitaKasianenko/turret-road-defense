using Game.Configs;
using Game.Vehicle;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.UI
{
    public class LevelProgressView : MonoBehaviour
    {
        [SerializeField] private Image _fill;
        [SerializeField] private RectTransform _marker;

        private Car _car;
        private LevelConfig _config;

        [Inject]
        public void Construct(Car car, LevelConfig config)
        {
            _car = car;
            _config = config;
        }

        private void LateUpdate()
        {
            float progress = Mathf.Clamp01(_car.Position.z / _config.Length);
            _fill.fillAmount = progress;
            _marker.anchorMin = new Vector2(progress, _marker.anchorMin.y);
            _marker.anchorMax = new Vector2(progress, _marker.anchorMax.y);
        }
    }
}
