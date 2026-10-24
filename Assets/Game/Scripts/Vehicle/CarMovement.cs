using Game.Configs;
using UnityEngine;
using Zenject;

namespace Game.Vehicle
{
    public class CarMovement : MonoBehaviour
    {
        private CarConfig _config;
        private float _targetSpeed;

        public float Speed { get; private set; }

        [Inject]
        public void Construct(CarConfig config) =>
            _config = config;

        private void Update()
        {
            float rate = _targetSpeed > Speed ? _config.Acceleration : _config.BrakeDeceleration;
            Speed = Mathf.MoveTowards(Speed, _targetSpeed, rate * Time.deltaTime);
            transform.position += transform.forward * (Speed * Time.deltaTime);
        }

        public void Accelerate() =>
            _targetSpeed = _config.MaxSpeed;

        public void Brake() =>
            _targetSpeed = 0f;

        public void Warp(Vector3 position)
        {
            transform.position = position;
            Speed = 0f;
            _targetSpeed = 0f;
        }
    }
}
