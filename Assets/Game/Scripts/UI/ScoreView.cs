using DG.Tweening;
using Game.Scoring;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.UI
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _comboText;
        [SerializeField] private float _punchStrength = 0.25f;
        [SerializeField] private float _punchDuration = 0.2f;

        private ScoreService _score;
        private int _shownScore;
        private int _shownMultiplier = 1;

        [Inject]
        public void Construct(ScoreService score) =>
            _score = score;

        private void Start()
        {
            _score.Changed += Refresh;
            Refresh();
        }

        private void OnDestroy() =>
            _score.Changed -= Refresh;

        private void Refresh()
        {
            if (_score.Score != _shownScore && _score.Score > 0)
                Punch(_scoreText.transform);

            if (_score.Multiplier != _shownMultiplier && _score.Multiplier > 1)
                Punch(_comboText.transform);

            _shownScore = _score.Score;
            _shownMultiplier = _score.Multiplier;

            _scoreText.text = _score.Score.ToString();
            _comboText.text = $"x{_score.Multiplier} COMBO";
            _comboText.enabled = _score.Multiplier > 1;
        }

        private void Punch(Transform target)
        {
            target.DOKill(complete: true);
            target.DOPunchScale(Vector3.one * _punchStrength, _punchDuration, vibrato: 4);
        }
    }
}
