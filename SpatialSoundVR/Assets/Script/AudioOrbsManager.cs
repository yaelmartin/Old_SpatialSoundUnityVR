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

            StartCoroutine(LogAudioVisualizerCount());
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

        private System.Collections.IEnumerator LogAudioVisualizerCount()
        {
            yield return null; // Wait for one frame

            Debug.Log("Number of audio visualizer children: " + transform.childCount);
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