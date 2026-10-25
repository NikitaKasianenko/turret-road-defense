using UnityEngine;

namespace Game.Effects
{
    public class RendererTint : MonoBehaviour
    {
        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

        [SerializeField, Tooltip("Each renderer gets its own material instance, which keeps it SRP Batcher compatible")]
        private Renderer[] _renderers;

        private Material[] _materials;

        private void Awake()
        {
            _materials = new Material[_renderers.Length];
            for (int i = 0; i < _renderers.Length; i++)
                _materials[i] = _renderers[i].material;
        }

        private void OnDestroy()
        {
            foreach (Material material in _materials)
                Destroy(material);
        }

        public void Apply(Color color)
        {
            foreach (Material material in _materials)
                material.SetColor(BaseColorId, color);
        }
    }
}
