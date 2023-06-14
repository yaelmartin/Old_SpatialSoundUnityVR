using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class AudioProjectManager : MonoBehaviour
{
    public string folderDirectory;
    [SerializeField] private GameObject audioOrbPrefab;
    [SerializeField] private AudioOrbsManager audioOrbsManager;

    private float maxAudioLengthSeconds; // Maximum duration of the retrieved audio files

    public void TryImportProjectOrAudioFiles()
    {
        ClearAudioOrbs();
        ImportAudioFiles();
        
        string jsonFilePath = Path.Combine(folderDirectory, "project.json");
        Debug.Log("Searching for =>"+jsonFilePath);

        if (File.Exists(jsonFilePath))
        {
            Debug.Log("Found existing project.json file, importing.");
            //LoadProjectJSON();
            StartCoroutine(ImportProject());
        }
        else
        {
            Debug.Log("No existing project.json file found. This project is new.");
        }
    }
    
    
    private System.Collections.IEnumerator ImportProject()
    {
        Debug.Log("ImportProjectCoroutine started");
        yield return new WaitForSeconds(2f);
        Debug.Log("ImportProjectCoroutine going to");
        LoadProjectJSON();
    }
    

    private void ImportAudioFiles()
    {
        string[] audioFiles = Directory.GetFiles(folderDirectory, "*.*", SearchOption.AllDirectories);
        int prefabIndex = 0;
        int gridWidth = 5; // Adjust the grid width as desired

        for (int i = 0; i < audioFiles.Length; i++)
        {
            string file = audioFiles[i];
            string extension = Path.GetExtension(file).ToLower();
            if (extension == ".ogg" || extension == ".mp3" || extension == ".wav" || extension == ".flac")
            {
                int row = prefabIndex / gridWidth;
                int column = prefabIndex % gridWidth;

                Vector3 position = audioOrbsManager.transform.position + new Vector3(column * 0.5f, 0f, row * 0.5f); // Adjust the spacing between prefabs as desired
                Quaternion rotation = Quaternion.identity;
                GameObject audioOrb = Instantiate(audioOrbPrefab, position, rotation);
                AudioSource audioSource = audioOrb.GetComponent<AudioSource>();
                StartCoroutine(LoadAudioClipAsync(file, audioSource));
                audioOrb.transform.SetParent(audioOrbsManager.transform,true);

                // Set the audio file name as the name of the AudioSource game object
                audioSource.gameObject.name = Path.GetFileName(file);

                // Update the maximum audio length
                if (audioSource.clip != null && audioSource.clip.length > maxAudioLengthSeconds)
                {
                    maxAudioLengthSeconds = audioSource.clip.length;
                }

                prefabIndex++;
            }
        }
        Debug.Log("All audio Orbs GO have been generated.");
    }


    private System.Collections.IEnumerator LoadAudioClipAsync(string filePath, AudioSource audioSource)
    {
        UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + filePath, GetAudioType(filePath));
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Failed to load audio clip from '{filePath}': {www.error}");
        }
        else
        {
            AudioClip audioClip = DownloadHandlerAudioClip.GetContent(www);
            audioSource.clip = audioClip;
        }
    }

    private AudioType GetAudioType(string filePath)
    {
        string extension = Path.GetExtension(filePath).ToLower();
        switch (extension)
        {
            case ".ogg":
                return AudioType.OGGVORBIS;
            case ".mp3":
                return AudioType.MPEG;
            case ".wav":
                return AudioType.WAV;
            case ".flac":
                return AudioType.OGGVORBIS; // Unity doesn't have native support for FLAC, but OGG Vorbis is a common format for FLAC files
            default:
                return AudioType.UNKNOWN;
        }
    }

    private void ClearAudioOrbs()
    {
        int childCount = audioOrbsManager.transform.childCount;

        for (int i = childCount - 1; i >= 0; i--)
        {
            GameObject child = audioOrbsManager.transform.GetChild(i).gameObject;
            Destroy(child);
        }
    }

    
    public void SaveProjectJSON()
    {
        List<AudioOrbData> audioOrbDataList = new List<AudioOrbData>();

        foreach (Transform child in audioOrbsManager.transform)
        {
            GameObject audioOrb = child.gameObject;
            AudioSource audioSource = audioOrb.GetComponent<AudioSource>();

            AudioOrbData audioOrbData = new AudioOrbData();
            audioOrbData.audioFileName = audioSource.gameObject.name;
            audioOrbData.position = audioOrb.transform.localPosition;
            audioOrbData.rotation = audioOrb.transform.localRotation;

            audioOrbDataList.Add(audioOrbData);
        }

        AudioProjectData projectData = new AudioProjectData();
        projectData.audioOrbDataList = audioOrbDataList;

        string json = JsonUtility.ToJson(projectData);
        string jsonFilePath = Path.Combine(folderDirectory, "project.json");
        File.WriteAllText(jsonFilePath, json);

        Debug.Log("Project JSON saved.");
    }
    
    private void LoadProjectJSON()
    {
        string json = File.ReadAllText(Path.Combine(folderDirectory, "project.json"));
        AudioProjectData projectData = JsonUtility.FromJson<AudioProjectData>(json);

        foreach (AudioOrbData audioOrbData in projectData.audioOrbDataList)
        {
            string audioFileName = audioOrbData.audioFileName;
            Vector3 position = audioOrbData.position;
            Quaternion rotation = audioOrbData.rotation;

            // Find the audio orb with the matching audio file name
            GameObject audioOrb = audioOrbsManager.transform.Find(audioFileName)?.gameObject;

            if (audioOrb != null)
            {
                // Apply the saved transform data
                audioOrb.transform.localRotation = rotation;
                audioOrb.transform.localPosition = position;
            }
        }

        Debug.Log("Project JSON loaded. Saved positions have been applied");
    }


}

[System.Serializable]
public class AudioProjectData
{
    public List<AudioOrbData> audioOrbDataList;
}

[System.Serializable]
public class AudioOrbData
{
    public string audioFileName;
    public Vector3 position;
    public Quaternion rotation;
}
