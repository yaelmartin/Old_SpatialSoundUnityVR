using UnityEngine;
using UnityEngine.Rendering;
using TMPro;

public class AudioSourceVisualizer : MonoBehaviour
{
    public AudioSource audioSource;
    public Gradient loudnessColorGradient;

    private Renderer cubeRenderer;
    public TMP_Text _textMesh;

    private bool isPlayingFromTime = false;
    private float playFromTime = 0f;

    private MaterialPropertyBlock materialPropertyBlock;
    private int colorPropertyID;

    public void Initialize()
    {
        cubeRenderer = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();
        _textMesh = GetComponentInChildren<TMP_Text>();
        _textMesh.text = name;

        materialPropertyBlock = new MaterialPropertyBlock();
        colorPropertyID = Shader.PropertyToID("_BaseColor");
    }

    private void Update()
    {
        if (isPlayingFromTime && !audioSource.isPlaying)
        {
            isPlayingFromTime = false;
            cubeRenderer.GetPropertyBlock(materialPropertyBlock);
            materialPropertyBlock.SetColor(colorPropertyID, Color.black);
            cubeRenderer.SetPropertyBlock(materialPropertyBlock);
        }

        if (isPlayingFromTime)
        {
            float loudness = GetAudioLoudness();
            float normalizedLoudness = Mathf.InverseLerp(0f, 1f, loudness);
            Color color = loudnessColorGradient.Evaluate(normalizedLoudness);

            cubeRenderer.GetPropertyBlock(materialPropertyBlock);
            materialPropertyBlock.SetColor(colorPropertyID, color);
            cubeRenderer.SetPropertyBlock(materialPropertyBlock);
        }
    }

    private float GetAudioLoudness()
    {
        float[] samples = new float[512];
        audioSource.GetOutputData(samples, 0);
        float sum = 0f;
        for (int i = 0; i < samples.Length; i++)
        {
            sum += Mathf.Abs(samples[i]);
        }
        return sum / samples.Length;
    }

    public void PlayFrom(float seconds)
    {
        isPlayingFromTime = true;
        playFromTime = seconds;

        audioSource.time = playFromTime;
        audioSource.Play();
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }
}
