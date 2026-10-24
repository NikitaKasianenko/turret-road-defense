using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Input
{
    public class PointerInputService : IInputService
    {
        public bool IsPointerHeld =>
            Pointer.current != null && Pointer.current.press.isPressed;

        public Vector2 PointerPosition =>
            Pointer.current?.position.ReadValue() ?? Vector2.zero;

        public UniTask WaitForTapAsync(CancellationToken cancellationToken) =>
            UniTask.WaitUntil(IsTappedThisFrame, cancellationToken: cancellationToken);

        private static bool IsTappedThisFrame() =>
            Pointer.current != null && Pointer.current.press.wasPressedThisFrame;
    }
}
