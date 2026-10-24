using UnityEngine;
using Zenject;

namespace Game.UI
{
    public class Billboard : MonoBehaviour
    {
        private Camera _camera;

        [Inject]
        public void Construct(Camera camera) =>
            _camera = camera;

        private void LateUpdate() =>
            transform.rotation = _camera.transform.rotation;
    }
}
