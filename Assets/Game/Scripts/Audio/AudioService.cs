using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Audio
{
    public class AudioService : IAudioService, IInitializable
    {
        private const int VoiceCount = 16;

        private readonly Dictionary<SoundEffect, float> _lastPlayTimes = new();

        private AudioSource[] _voices;
        private int _nextVoice;

        public void Initialize()
        {
            var host = new GameObject("Audio");
            _voices = new AudioSource[VoiceCount];

            for (int i = 0; i < VoiceCount; i++)
            {
                AudioSource voice = host.AddComponent<AudioSource>();
                voice.playOnAwake = false;
                voice.spatialBlend = 0f;
                _voices[i] = voice;
            }
        }

        public void Play(SoundEffect sound)
        {
            if (sound == null || sound.Clips.Length == 0 || IsThrottled(sound))
                return;

            AudioSource voice = _voices[_nextVoice];
            _nextVoice = (_nextVoice + 1) % _voices.Length;

            voice.clip = sound.Clips[Random.Range(0, sound.Clips.Length)];
            voice.volume = sound.Volume;
            voice.pitch = Random.Range(sound.PitchRange.x, sound.PitchRange.y);
            voice.Play();
        }

        private bool IsThrottled(SoundEffect sound)
        {
            float now = Time.unscaledTime;
            if (_lastPlayTimes.TryGetValue(sound, out float lastPlayTime) && now - lastPlayTime < sound.MinInterval)
                return true;

            _lastPlayTimes[sound] = now;
            return false;
        }
    }
}
