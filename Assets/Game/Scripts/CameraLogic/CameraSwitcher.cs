using Unity.Cinemachine;
using UnityEngine;

namespace Game.CameraLogic
{
    public class CameraSwitcher : MonoBehaviour
    {
        private const int ActivePriority = 10;
        private const int InactivePriority = 0;

        [SerializeField] private CinemachineCamera _introCamera;
        [SerializeField] private CinemachineCamera _gameplayCamera;

        public void ShowIntro()
        {
            _introCamera.PreviousStateIsValid = false;
            _gameplayCamera.PreviousStateIsValid = false;
            Activate(_introCamera);
        }

        public void ShowGameplay() =>
            Activate(_gameplayCamera);

        private void Activate(CinemachineCamera activeCamera)
        {
            _introCamera.Priority = activeCamera == _introCamera ? ActivePriority : InactivePriority;
            _gameplayCamera.Priority = activeCamera == _gameplayCamera ? ActivePriority : InactivePriority;
        }
    }
}
