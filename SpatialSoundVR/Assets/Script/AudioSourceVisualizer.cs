using UnityEngine;
using TMPro;

namespace SpatialSoundVR
{
    public class AudioSourceVisualizer : MonoBehaviour
    {
        public AudioSource audioSource;
        public Gradient loudnessColorGradient;

        private Renderer cubeRenderer;
        private TMP_Text textMesh;

        private bool isPlayingFromTime = false;
        private float playFromTime = 0f;

        private MaterialPropertyBlock materialPropertyBlock;
        private int colorPropertyID;

        private void Awake()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            if (audioSource != null)
            {
                //Resources.UnloadAsset(audioSource.clip);
                AudioClip.DestroyImmediate(audioSource.clip,false);
                //audioSource.clip.UnloadAudioData();
            }
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

        public void Initialize()
        {
            cubeRenderer = GetComponent<Renderer>();
            audioSource = GetComponent<AudioSource>();
            textMesh = GetComponentInChildren<TMP_Text>();
            textMesh.text = name;

            materialPropertyBlock = new MaterialPropertyBlock();
            colorPropertyID = Shader.PropertyToID("_BaseColor");
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
}