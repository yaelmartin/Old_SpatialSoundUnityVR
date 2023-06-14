using UnityEngine;

public class AudioOrbsManager : MonoBehaviour
{
    private AudioSourceVisualizer[] audioVisualizers;
    
    public void SaveAudioVisualizersAndInitialize()
    {
        audioVisualizers = GetComponentsInChildren<AudioSourceVisualizer>();
        foreach (AudioSourceVisualizer visualizer in audioVisualizers)
        {
            visualizer.Initialize();
        }
    }

    public void PlayAudioFromTime(float seconds)
    {
        foreach (AudioSourceVisualizer visualizer in audioVisualizers)
        {
            visualizer.PlayFrom(seconds);
        }
    }

    public void StopAudio()
    {
        foreach (AudioSourceVisualizer visualizer in audioVisualizers)
        {
            visualizer.StopAudio();
        }
    }

    public void PlayAudioFromStart()
    {
        foreach (AudioSourceVisualizer visualizer in audioVisualizers)
        {
            visualizer.PlayFrom(0f);
        }
    }
    
    
}