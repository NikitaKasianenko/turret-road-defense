using UnityEngine;

namespace Game.Vehicle
{
    public class CarWheels : MonoBehaviour
    {
        [SerializeField] private CarMovement _movement;
        [SerializeField] private Transform[] _wheels;
        [SerializeField, Min(0.01f)] private float _wheelRadius = 0.35f;

        private void Update()
        {
            float angle = _movement.Speed / _wheelRadius * Mathf.Rad2Deg * Time.deltaTime;

            foreach (Transform wheel in _wheels)
                wheel.Rotate(angle, 0f, 0f, Space.Self);
        }
    }
}
