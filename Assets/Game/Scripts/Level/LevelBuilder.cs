using Game.Configs;
using UnityEngine;
using Zenject;

namespace Game.Level
{
    public class LevelBuilder : MonoBehaviour
    {
        [SerializeField] private GameObject _groundTilePrefab;
        [SerializeField] private Transform _groundRoot;
        [SerializeField] private FinishLine _finishLine;

        private LevelConfig _config;

        [Inject]
        public void Construct(LevelConfig config) =>
            _config = config;

        public void Build()
        {
            BuildGround();
            _finishLine.transform.position = Vector3.forward * _config.Length;
        }

        private void BuildGround()
        {
            float groundLength = _config.Length + _config.GroundPadding * 2f;
            int tilesCount = Mathf.CeilToInt(groundLength / _config.GroundTileLength);
            float firstTileCenter = -_config.GroundPadding + _config.GroundTileLength * 0.5f;

            for (int i = 0; i < tilesCount; i++)
            {
                Vector3 position = Vector3.forward * (firstTileCenter + _config.GroundTileLength * i);
                GameObject tile = Instantiate(_groundTilePrefab, position, Quaternion.identity, _groundRoot);
                tile.name = $"{_groundTilePrefab.name}_{i:00}";
            }
        }
    }
}
