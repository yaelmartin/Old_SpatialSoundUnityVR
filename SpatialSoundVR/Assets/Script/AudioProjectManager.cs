using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace SpatialSoundVR
{
    public class AudioProjectManager : MonoBehaviour
    {
        [SerializeField] private GameObject audioOrbPrefab;
        [SerializeField] private AudioOrbsManager audioOrbsManager;
        [SerializeField] private UIPlayerManager uiPlayerManager;
        
        
        
        //Attributes
        [SerializeField] private string currentProjectFolderDirectory;
        [SerializeField] private bool preventNewLoad;
        [SerializeField] private bool readyForPlay;
        [SerializeField] private float currentAudioTimeSeconds;
        [SerializeField] private float maxAudioLengthSeconds; // Maximum duration of the retrieved audio files
        [SerializeField] private MusicStatus musicStatus;
        
        //Accessors
        [SerializeField] public float maxAudioLengthSecondsAccessor => maxAudioLengthSeconds;
        [SerializeField] public bool readyForPlayAccessor => readyForPlay;
        





        public void Start()
        {
            preventNewLoad = false;
            readyForPlay = false;
            musicStatus = MusicStatus.Undefined;
        }

        
        
        
        
        /*
             ___       ______ .___________. __    ______   .__   __.      _______.
            /   \     /      ||           ||  |  /  __  \  |  \ |  |     /       |
           /  ^  \   |  ,----'`---|  |----`|  | |  |  |  | |   \|  |    |   (----`
          /  /_\  \  |  |         |  |     |  | |  |  |  | |  . `  |     \   \    
         /  _____  \ |  `----.    |  |     |  | |  `--'  | |  |\   | .----)   |   
        /__/     \__\ \______|    |__|     |__|  \______/  |__| \__| |_______/                                                                       
         Called by buttons
         */
        
        
        //MAIN ACTIONS, THEY DON'T NEED ANYTHING TO BE READY/CALLED
        //They also don't return a bool
        /*
        public void TryOpenFileExplorer()
        {
            
        }
        */

        /**
         * Can always be called. Shouldn't be used in the final version
         */
        public void TryOpenProjectUsingDefaultPath()
        {
            if (!preventNewLoad)
            {
                preventNewLoad = true;
                StartCoroutine(ImportAudioFilesAndProjectCoroutine());
            }
        }
        
        
        public void TryReset()
        {
            if (!preventNewLoad)
            {
                ResetForFutureLoad();
            }
        }




        
        //THESE ACTIONS NEEDS THE SONG TO BE READY
        public bool TryResume()
        {
            if (readyForPlay && (musicStatus==MusicStatus.Pause || musicStatus==MusicStatus.NotStarted))
            {
                audioOrbsManager.PlayAudioFromTime(currentAudioTimeSeconds);
                musicStatus = MusicStatus.Playing;
                return true;
            }
            else
            {
                return false;
            } 
        }

        public bool TryPause()
        {
            if (readyForPlay && musicStatus==MusicStatus.Playing)
            {
                //TODO
                audioOrbsManager.StopAudio();
                musicStatus = MusicStatus.Pause;
                return true;
            }
            else
            {
                return false;
            } 
        }

        public bool TryStop()
        {
            if (TryPause())
            {
                currentAudioTimeSeconds = 0;
                musicStatus = MusicStatus.NotStarted;
                audioOrbsManager.StopAudio();
                return true;
            }
            else
            {
                return false;
            } 
        }


        public bool TryPlaySongNowFrom(float seconds)
        {
            if (readyForPlay)
            {
                musicStatus = MusicStatus.Playing;
                currentAudioTimeSeconds = seconds;
                audioOrbsManager.PlayAudioFromTime(seconds);
                return true;
            }
            else
            {
                return false;
            }
        }
        
        
        
        public bool TrySaveProject()
        {
            if (currentProjectFolderDirectory != null && readyForPlay)
            {
                SaveProjectJSON();
                return true;
            }
            else
            {
                return false;
            }
        }


        /*
         __  .___  ___. .______     ______   .______     .___________.
        |  | |   \/   | |   _  \   /  __  \  |   _  \    |           |
        |  | |  \  /  | |  |_)  | |  |  |  | |  |_)  |   `---|  |----`
        |  | |  |\/|  | |   ___/  |  |  |  | |      /        |  |     
        |  | |  |  |  | |  |      |  `--'  | |  |\  \----.   |  |     
        |__| |__|  |__| | _|       \______/  | _| `._____|   |__|     
         */


        //MAIN METHOD
        private IEnumerator ImportAudioFilesAndProjectCoroutine()
        {
            #if UNITY_EDITOR
                // keep folderDirectory
            #else
                folderDirectory = Path.Combine(Application.streamingAssetsPath, "AudioSource");
            #endif

            yield return StartCoroutine(ResetForFutureLoad());
            yield return new WaitForFixedUpdate();
            yield return StartCoroutine(ImportAudioFiles());
            yield return new WaitForFixedUpdate();
            
            
            string jsonFilePath = Path.Combine(currentProjectFolderDirectory, "project.json");
            Debug.Log("Searching for =>" + jsonFilePath);

            if (File.Exists(jsonFilePath))
            {
                yield return new WaitForFixedUpdate();
                Debug.Log("Found existing project.json file, importing.");
                yield return StartCoroutine(ImportProject());
            }
            else
            {
                Debug.Log("No existing project.json file found. This project is new.");
            }

            // make everything valid
            readyForPlay = true;
            preventNewLoad = false;
            musicStatus = MusicStatus.NotStarted;
            uiPlayerManager.UIsSetValuesForNewlySongLoad();
        }

        
        /// <summary>
        /// WILL import all audio files from the directory
        /// </summary>
        private IEnumerator ImportAudioFiles()
        {
            string[] audioFiles = Directory.GetFiles(currentProjectFolderDirectory, "*.*", SearchOption.AllDirectories);
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

                    Vector3 position = audioOrbsManager.transform.position +
                                       new Vector3(column * 0.5f, 0f,
                                           row * 0.5f); // Adjust the spacing between prefabs as desired
                    Quaternion rotation = Quaternion.Euler(0, 180, 0);
                    GameObject audioOrb = Instantiate(audioOrbPrefab, position, rotation);
                    AudioSource audioSource = audioOrb.GetComponent<AudioSource>();

                    // Call LoadAudioClipAsync coroutine using yield return
                    yield return StartCoroutine(LoadAudioClipAsync(file, audioSource));

                    audioOrb.transform.SetParent(audioOrbsManager.transform, true);

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
            audioOrbsManager.SaveAudioVisualizersAndInitialize();
  

            yield return null;
        }



        private IEnumerator LoadAudioClipAsync(string filePath, AudioSource audioSource)
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
                    return
                        AudioType
                            .OGGVORBIS; // Unity doesn't have native support for FLAC, but OGG Vorbis is a common format for FLAC files
                default:
                    return AudioType.UNKNOWN;
            }
        }
        
        /*
        .______       _______     _______. _______ .___________.
        |   _  \     |   ____|   /       ||   ____||           |
        |  |_)  |    |  |__     |   (----`|  |__   `---|  |----`
        |      /     |   __|     \   \    |   __|      |  |     
        |  |\  \----.|  |____.----)   |   |  |____     |  |     
        | _| `._____||_______|_______/    |_______|    |__|                                                         
         */
        private IEnumerator ResetForFutureLoad()
        {
            readyForPlay = false;
            musicStatus = MusicStatus.Undefined;
            uiPlayerManager.UIsNoProjectOpened();
            audioOrbsManager.DeleteAudioVisualizers();
            maxAudioLengthSeconds = 0f;
            currentAudioTimeSeconds = 0f;
            yield return null;
        }


        
        /*
               __       _______.  ______   .__   __. 
              |  |     /       | /  __  \  |  \ |  | 
              |  |    |   (----`|  |  |  | |   \|  | 
        .--.  |  |     \   \    |  |  |  | |  . `  | 
        |  `--'  | .----)   |   |  `--'  | |  |\   | 
         \______/  |_______/     \______/  |__| \__| 
         */
        private IEnumerator ImportProject()
        {
            LoadProjectJSON();
            yield return null;
        }
        private void SaveProjectJSON()
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
            string jsonFilePath = Path.Combine(currentProjectFolderDirectory, "project.json");
            File.WriteAllText(jsonFilePath, json);

            Debug.Log("Project JSON saved.");
        }

        private void LoadProjectJSON()
        {
            string json = File.ReadAllText(Path.Combine(currentProjectFolderDirectory, "project.json"));
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
    
    [System.Serializable] public class AudioProjectData
    {
        public List<AudioOrbData> audioOrbDataList;
    }
    [System.Serializable] public class AudioOrbData
    {
        public string audioFileName;
        public Vector3 position;
        public Quaternion rotation;
    }
    
    
    public enum MusicStatus
    {
        Undefined,
        NotStarted,
        Playing,
        Pause
    }
}
