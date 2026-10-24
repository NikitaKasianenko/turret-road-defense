using UnityEngine;

namespace Game.Effects
{
    public class ImpactEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particles;
        [SerializeField, Min(1)] private int _particlesPerImpact = 8;

        public void Play(Vector3 position, Vector3 normal)
        {
            _particles.transform.SetPositionAndRotation(position, Quaternion.LookRotation(normal));
            _particles.Emit(_particlesPerImpact);
        }
    }
}
