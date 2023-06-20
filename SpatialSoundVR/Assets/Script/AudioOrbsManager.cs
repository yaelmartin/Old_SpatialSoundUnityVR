using System;
using UnityEngine;

namespace SpatialSoundVR
{
    public class AudioOrbsManager : MonoBehaviour
    {
        [SerializeField] private AudioSourceVisualizer[] audioVisualizers;

        public void SaveAudioVisualizersAndInitialize()
        {
            audioVisualizers = GetComponentsInChildren<AudioSourceVisualizer>();

            foreach (AudioSourceVisualizer visualizer in audioVisualizers)
            {
                visualizer.Initialize();
            }
        }

        public void DeleteAudioVisualizers()
        {
            if (!(audioVisualizers == null || audioVisualizers.Length == 0))
            {
                foreach (AudioSourceVisualizer visualizer in audioVisualizers)
                {
                    if (visualizer != null)
                    {
                        Destroy(visualizer.gameObject);
                    }
                }
            }

            audioVisualizers = Array.Empty<AudioSourceVisualizer>();
        }



        public void PlayAudioFromTime(float seconds)
        {
            if (audioVisualizers == null || audioVisualizers.Length == 0) { return; }
            foreach (AudioSourceVisualizer visualizer in audioVisualizers)
            {
                visualizer.PlayFrom(seconds);
            }
        }

        public void StopAudio()
        {
            if (audioVisualizers == null || audioVisualizers.Length == 0) { return; }
            foreach (AudioSourceVisualizer visualizer in audioVisualizers)
            {
                visualizer.StopAudio();
            }
        }
    }
}