using System;

namespace Game.Combat
{
    public interface IReadOnlyHealth
    {
        event Action Changed;
        event Action Died;

        float Current { get; }
        float Max { get; }
        float Normalized { get; }
        bool IsDead { get; }
    }
}
