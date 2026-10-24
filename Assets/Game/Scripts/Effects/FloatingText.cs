using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace Game.Effects
{
    public class FloatingText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private float _riseHeight = 1.5f;
        [SerializeField] private float _duration = 0.8f;
        [SerializeField] private float _popScale = 1.3f;

        private Pool _pool;

        private void Show(Vector3 position, string text, Color color)
        {
            transform.DOKill();
            _text.DOKill();

            transform.position = position;
            transform.localScale = Vector3.zero;
            _text.text = text;
            _text.color = color;

            DOTween.Sequence()
                .Join(transform.DOScale(_popScale, _duration * 0.2f).SetEase(Ease.OutBack))
                .Append(transform.DOScale(1f, _duration * 0.1f))
                .Insert(0f, transform.DOMoveY(position.y + _riseHeight, _duration).SetEase(Ease.OutCubic))
                .Insert(_duration * 0.5f, _text.DOFade(0f, _duration * 0.5f))
                .SetTarget(transform)
                .OnComplete(() => _pool.Despawn(this));
        }

        public class Pool : MonoMemoryPool<Vector3, string, Color, FloatingText>
        {
            protected override void OnCreated(FloatingText text)
            {
                base.OnCreated(text);
                text._pool = this;
            }

            protected override void Reinitialize(Vector3 position, string message, Color color, FloatingText text) =>
                text.Show(position, message, color);
        }
    }
}
