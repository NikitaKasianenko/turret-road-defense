using UnityEngine;

namespace Game.Effects
{
    public class RendererTint : MonoBehaviour
    {
        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

        [SerializeField] private Renderer[] _renderers;

        private MaterialPropertyBlock _propertyBlock;

        private void Awake() =>
            _propertyBlock = new MaterialPropertyBlock();

        public void Apply(Color color)
        {
            foreach (Renderer target in _renderers)
            {
                target.GetPropertyBlock(_propertyBlock);
                _propertyBlock.SetColor(BaseColorId, color);
                target.SetPropertyBlock(_propertyBlock);
            }
        }
    }
}
