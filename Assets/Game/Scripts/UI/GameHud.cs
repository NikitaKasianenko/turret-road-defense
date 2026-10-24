using DG.Tweening;
using Game.Level;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.UI
{
    public class GameHud : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _startScreen;
        [SerializeField] private CanvasGroup _resultScreen;
        [SerializeField] private TMP_Text _resultTitle;
        [SerializeField, FormerlySerializedAs("_resultKills")] private TMP_Text _resultStats;

        [Header("Result")]
        [SerializeField] private string _winText = "You win";
        [SerializeField] private string _loseText = "You lose";
        [SerializeField] private Color _winColor = new(0.45f, 0.9f, 0.35f);
        [SerializeField] private Color _loseColor = new(0.95f, 0.3f, 0.25f);

        [Header("Animation")]
        [SerializeField, Min(0f)] private float _fadeDuration = 0.25f;
        [SerializeField, Min(0f)] private float _titlePopDuration = 0.4f;

        private void Awake()
        {
            _startScreen.alpha = 0f;
            _resultScreen.alpha = 0f;
        }

        public void ShowStartScreen() =>
            Fade(_startScreen, isVisible: true);

        public void HideStartScreen() =>
            Fade(_startScreen, isVisible: false);

        public void ShowResult(LevelResult result, int kills, int score)
        {
            bool isWin = result == LevelResult.Win;
            _resultTitle.text = isWin ? _winText : _loseText;
            _resultTitle.color = isWin ? _winColor : _loseColor;
            _resultStats.text = $"Kills: {kills}\nScore: {score}";

            Fade(_resultScreen, isVisible: true);

            _resultTitle.transform.DOKill();
            _resultTitle.transform.localScale = Vector3.one * 0.6f;
            _resultTitle.transform.DOScale(1f, _titlePopDuration).SetEase(Ease.OutBack);
        }

        public void HideResult() =>
            Fade(_resultScreen, isVisible: false);

        private void Fade(CanvasGroup group, bool isVisible)
        {
            group.DOKill();
            group.DOFade(isVisible ? 1f : 0f, _fadeDuration);
        }
    }
}
