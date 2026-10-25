using DG.Tweening;
using UnityEngine;

namespace Game.Effects
{
    public class DamageFlash : MonoBehaviour
    {
        private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

        [SerializeField] private Renderer[] _renderers;
        [SerializeField] private Color _color = Color.white;
        [SerializeField, Min(0f)] private float _duration = 0.15f;

        private MaterialPropertyBlock _propertyBlock;
        private Tween _tween;

        private void Awake() =>
            _propertyBlock = new MaterialPropertyBlock();

        private void OnDestroy() =>
            _tween?.Kill();

        public void Play()
        {
            _tween?.Kill();
            _tween = DOVirtual.Float(1f, 0f, _duration, Apply);
        }

        public void ResetFlash()
        {
            _tween?.Kill();
            Apply(0f);
        }

        private void Apply(float intensity)
        {
            _propertyBlock.SetColor(EmissionColorId, _color * intensity);
            MaterialPropertyBlock block = intensity > 0f ? _propertyBlock : null;

            foreach (Renderer target in _renderers)
                target.SetPropertyBlock(block);
        }
    }
}
