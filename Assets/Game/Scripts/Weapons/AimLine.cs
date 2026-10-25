using UnityEngine;

namespace Game.Weapons
{
    [RequireComponent(typeof(LineRenderer))]
    public class AimLine : MonoBehaviour
    {
        [SerializeField] private TurretAim _aim;
        [SerializeField] private Transform _muzzle;
        [SerializeField] private LineRenderer _line;
        [SerializeField, Min(0f)] private float _length = 22f;

        private float _barrelLength;

        private void Awake()
        {
            Vector3 barrel = _muzzle.position - _aim.transform.position;
            barrel.y = 0f;
            _barrelLength = barrel.magnitude;
        }

        private void LateUpdate()
        {
            Vector3 direction = _aim.TargetDirection;
            Vector3 origin = _aim.transform.position;
            origin.y = _muzzle.position.y;

            Vector3 start = origin + direction * _barrelLength;
            _line.SetPosition(0, start);
            _line.SetPosition(1, start + direction * _length);
        }
    }
}
