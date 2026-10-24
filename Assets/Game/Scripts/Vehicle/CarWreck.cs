using DG.Tweening;
using UnityEngine;

namespace Game.Vehicle
{
    public class CarWreck : MonoBehaviour
    {
        [SerializeField] private Transform[] _parts;
        [SerializeField, Min(0f)] private float _scatterDistance = 4f;
        [SerializeField, Min(0f)] private float _jumpPower = 3f;
        [SerializeField, Min(0.01f)] private float _duration = 1f;
        [SerializeField] private float _landingHeight = 0.35f;
        [SerializeField] private float _maxSpin = 540f;

        private Transform[] _parents;
        private Vector3[] _localPositions;
        private Quaternion[] _localRotations;
        private Vector3[] _localScales;

        private void Awake()
        {
            _parents = new Transform[_parts.Length];
            _localPositions = new Vector3[_parts.Length];
            _localRotations = new Quaternion[_parts.Length];
            _localScales = new Vector3[_parts.Length];

            for (int i = 0; i < _parts.Length; i++)
            {
                _parents[i] = _parts[i].parent;
                _localPositions[i] = _parts[i].localPosition;
                _localRotations[i] = _parts[i].localRotation;
                _localScales[i] = _parts[i].localScale;
            }
        }

        public void Scatter()
        {
            foreach (Transform part in _parts)
            {
                Vector3 landingPoint = part.position + ScatterDirection(part) * (_scatterDistance * Random.Range(0.7f, 1.3f));
                landingPoint.y = _landingHeight;

                part.SetParent(null, worldPositionStays: true);
                part.DOJump(landingPoint, _jumpPower * Random.Range(0.7f, 1.2f), numJumps: 1, _duration).SetEase(Ease.Linear);
                part.DOBlendableRotateBy(Random.insideUnitSphere * _maxSpin, _duration, RotateMode.FastBeyond360);
            }
        }

        public void Restore()
        {
            for (int i = 0; i < _parts.Length; i++)
            {
                Transform part = _parts[i];
                part.DOKill();
                part.SetParent(_parents[i], worldPositionStays: false);
                part.SetLocalPositionAndRotation(_localPositions[i], _localRotations[i]);
                part.localScale = _localScales[i];
            }
        }

        private Vector3 ScatterDirection(Transform part)
        {
            Vector3 outward = part.position - transform.position;
            outward.y = 0f;

            Vector2 jitter = Random.insideUnitCircle;
            Vector3 direction = outward.normalized + new Vector3(jitter.x, 0f, jitter.y) * 0.6f;
            return direction.sqrMagnitude > 0.01f ? direction.normalized : Vector3.right;
        }
    }
}
