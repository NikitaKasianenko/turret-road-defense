using UnityEngine;

namespace Game.Audio
{
    [CreateAssetMenu(fileName = "SoundEffect", menuName = "Game/Sound Effect")]
    public class SoundEffect : ScriptableObject
    {
        [field: SerializeField, Tooltip("A random clip is picked on each play, so repeated sounds don't feel identical")]
        public AudioClip[] Clips { get; private set; } = new AudioClip[0];
        [field: SerializeField, Range(0f, 1f)] public float Volume { get; private set; } = 1f;
        [field: SerializeField, Tooltip("x = min, y = max")] public Vector2 PitchRange { get; private set; } = new(0.95f, 1.05f);
        [field: SerializeField, Min(0f), Tooltip("Real-time seconds before this sound can play again; keeps rapid fire and crowds from piling up")]
        public float MinInterval { get; private set; }
    }
}
