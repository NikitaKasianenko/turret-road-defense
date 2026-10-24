using DG.Tweening;
using UnityEngine;

namespace Game.UI
{
    public class PulseAnimation : MonoBehaviour
    {
        [SerializeField] private float _scale = 1.08f;
        [SerializeField, Min(0.01f)] private float _halfPeriod = 0.6f;

        private void OnEnable() =>
            transform.DOScale(_scale, _halfPeriod).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);

        private void OnDisable()
        {
            transform.DOKill();
            transform.localScale = Vector3.one;
        }
    }
}
