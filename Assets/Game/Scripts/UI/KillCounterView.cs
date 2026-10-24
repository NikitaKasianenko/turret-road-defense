using DG.Tweening;
using Game.Enemies;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.UI
{
    public class KillCounterView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private float _punchStrength = 0.25f;
        [SerializeField] private float _punchDuration = 0.2f;

        private EnemySpawner _enemySpawner;

        [Inject]
        public void Construct(EnemySpawner enemySpawner) =>
            _enemySpawner = enemySpawner;

        private void Start()
        {
            _enemySpawner.KilledCountChanged += Refresh;
            Refresh();
        }

        private void OnDestroy() =>
            _enemySpawner.KilledCountChanged -= Refresh;

        private void Refresh()
        {
            _text.text = _enemySpawner.KilledCount.ToString();

            if (_enemySpawner.KilledCount == 0)
                return;

            _text.transform.DOKill(complete: true);
            _text.transform.DOPunchScale(Vector3.one * _punchStrength, _punchDuration, vibrato: 4);
        }
    }
}
