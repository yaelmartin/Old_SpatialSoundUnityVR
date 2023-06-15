using System;
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
    
    public void DeleteAudioVisualizers()
    {
        int childCount = transform.childCount;

        for (int i = childCount - 1; i >= 0; i--)
        {
            GameObject child = transform.GetChild(i).gameObject;
            Destroy(child);
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