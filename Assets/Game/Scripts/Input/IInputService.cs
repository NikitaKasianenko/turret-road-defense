using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Input
{
    public interface IInputService
    {
        bool IsPointerHeld { get; }
        Vector2 PointerPosition { get; }

        UniTask WaitForTapAsync(CancellationToken cancellationToken);
    }
}
