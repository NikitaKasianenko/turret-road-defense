using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Configs;
using Game.Enemies;
using Game.Level;
using UnityEngine;
using Zenject;

namespace Game.Effects
{
    public class TimeEffects : IInitializable, IDisposable, IResettable
    {
        private readonly EnemySpawner _enemySpawner;
        private readonly LevelSession _session;
        private readonly FeelConfig _config;

        private CancellationTokenSource _hitStopCancellation;
        private Tween _slowMotion;

        public TimeEffects(EnemySpawner enemySpawner, LevelSession session, FeelConfig config)
        {
            _enemySpawner = enemySpawner;
            _session = session;
            _config = config;
        }

        public void Initialize()
        {
            _enemySpawner.EnemyKilled += OnEnemyKilled;
            _session.Ended += OnSessionEnded;
        }

        public void Dispose()
        {
            _enemySpawner.EnemyKilled -= OnEnemyKilled;
            _session.Ended -= OnSessionEnded;
            ResetState();
        }

        public void ResetState()
        {
            CancelHitStop();
            _slowMotion?.Kill();
            Time.timeScale = 1f;
        }

        private void OnEnemyKilled(Enemy enemy)
        {
            if (!_session.IsRunning)
                return;

            CancelHitStop();
            _hitStopCancellation = new CancellationTokenSource();
            HitStopAsync(_hitStopCancellation.Token).Forget();
        }

        private async UniTaskVoid HitStopAsync(CancellationToken cancellationToken)
        {
            Time.timeScale = _config.HitStopTimeScale;

            bool isCanceled = await UniTask
                .Delay(TimeSpan.FromSeconds(_config.HitStopDuration), ignoreTimeScale: true, cancellationToken: cancellationToken)
                .SuppressCancellationThrow();

            if (!isCanceled)
                Time.timeScale = 1f;
        }

        private void OnSessionEnded()
        {
            CancelHitStop();
            _slowMotion?.Kill();

            Time.timeScale = _config.SlowMotionTimeScale;
            _slowMotion = DOTween.To(() => Time.timeScale, value => Time.timeScale = value, 1f, _config.SlowMotionDuration)
                .SetEase(Ease.InQuad)
                .SetUpdate(isIndependentUpdate: true);
        }

        private void CancelHitStop()
        {
            _hitStopCancellation?.Cancel();
            _hitStopCancellation?.Dispose();
            _hitStopCancellation = null;
        }
    }
}
