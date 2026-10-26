using System;
using DG.Tweening;
using Game.Configs;
using Game.Level;
using UnityEngine;
using Zenject;

namespace Game.Effects
{
    public class TimeEffects : IInitializable, IDisposable, IResettable
    {
        private readonly LevelSession _session;
        private readonly FeelConfig _config;

        private Tween _slowMotion;

        public TimeEffects(LevelSession session, FeelConfig config)
        {
            _session = session;
            _config = config;
        }

        public void Initialize() =>
            _session.Ended += OnSessionEnded;

        public void Dispose()
        {
            _session.Ended -= OnSessionEnded;
            ResetState();
        }

        public void ResetState()
        {
            _slowMotion?.Kill();
            Time.timeScale = 1f;
        }

        private void OnSessionEnded()
        {
            _slowMotion?.Kill();

            Time.timeScale = _config.SlowMotionTimeScale;
            _slowMotion = DOTween.To(() => Time.timeScale, value => Time.timeScale = value, 1f, _config.SlowMotionDuration)
                .SetEase(Ease.InQuad)
                .SetUpdate(isIndependentUpdate: true);
        }
    }
}
