using Game.Configs;
using Game.Input;
using UnityEngine;
using Zenject;

namespace Game.Weapons
{
    public class TurretAim : MonoBehaviour
    {
        private const float FarAimDistance = 1000f;
        private static readonly Plane GroundPlane = new(Vector3.up, Vector3.zero);

        private TurretConfig _config;
        private IInputService _input;
        private Camera _camera;

        private float _currentYaw;
        private float _yawVelocity;

        public float TargetYaw { get; private set; }

        public Vector3 TargetDirection =>
            transform.parent.rotation * Quaternion.Euler(0f, TargetYaw, 0f) * Vector3.forward;

        [Inject]
        public void Construct(TurretConfig config, IInputService input, Camera camera)
        {
            _config = config;
            _input = input;
            _camera = camera;
        }

        private void Update()
        {
            if (_input.IsPointerHeld)
                TargetYaw = CalculateYaw(_input.PointerPosition);

            _currentYaw = Mathf.SmoothDamp(_currentYaw, TargetYaw, ref _yawVelocity, _config.AimSmoothTime);
            ApplyRotation();
        }

        public void ResetAim()
        {
            TargetYaw = 0f;
            _currentYaw = 0f;
            _yawVelocity = 0f;
            ApplyRotation();
        }

        private float CalculateYaw(Vector2 screenPosition)
        {
            Ray ray = _camera.ScreenPointToRay(screenPosition);
            Vector3 aimPoint = GroundPlane.Raycast(ray, out float distance)
                ? ray.GetPoint(distance)
                : ray.GetPoint(FarAimDistance);

            Vector3 localDirection = transform.parent.InverseTransformDirection(aimPoint - transform.position);
            float yaw = Mathf.Atan2(localDirection.x, localDirection.z) * Mathf.Rad2Deg;
            return Mathf.Clamp(yaw, -_config.MaxYawAngle, _config.MaxYawAngle);
        }

        private void ApplyRotation() =>
            transform.localRotation = Quaternion.Euler(0f, _currentYaw, 0f);
    }
}
