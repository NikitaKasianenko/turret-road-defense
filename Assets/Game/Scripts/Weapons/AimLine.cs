using UnityEngine;

namespace Game.Weapons
{
    [RequireComponent(typeof(LineRenderer))]
    public class AimLine : MonoBehaviour
    {
        [SerializeField] private TurretAim _aim;
        [SerializeField] private LineRenderer _line;
        [SerializeField, Min(0f)] private float _startOffset = 2.5f;
        [SerializeField, Min(0f)] private float _length = 22f;
        [SerializeField] private float _height = 0.08f;

        private void LateUpdate()
        {
            Vector3 direction = _aim.TargetDirection;
            Vector3 origin = _aim.transform.position;
            origin.y = _height;

            _line.SetPosition(0, origin + direction * _startOffset);
            _line.SetPosition(1, origin + direction * (_startOffset + _length));
        }
    }
}
