using System;

namespace Game.Level
{
    public class LevelSession
    {
        public event Action Started;
        public event Action Ended;

        public bool IsRunning { get; private set; }

        public void Start()
        {
            IsRunning = true;
            Started?.Invoke();
        }

        public void End()
        {
            if (!IsRunning)
                return;

            IsRunning = false;
            Ended?.Invoke();
        }
    }
}
