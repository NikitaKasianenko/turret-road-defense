using System;
using Game.Vehicle;
using UnityEngine;

namespace Game.Level
{
    [RequireComponent(typeof(Collider))]
    public class FinishLine : MonoBehaviour
    {
        public event Action Reached;

        private void OnTriggerEnter(Collider other)
        {
            if (other.attachedRigidbody != null && other.attachedRigidbody.TryGetComponent(out Car _))
                Reached?.Invoke();
        }
    }
}
