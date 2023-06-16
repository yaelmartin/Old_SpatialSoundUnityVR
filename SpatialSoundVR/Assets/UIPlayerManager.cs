using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIPlayerManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private AudioProjectManager audioProjectManager;
    
    
    [SerializeField] private Button buttonSave;
    
    

    [SerializeField] private GameObject playerBar;
    [SerializeField] private Slider playerSlider;
    [SerializeField] private TMP_Text playerTextCurrentTime;
    [SerializeField] private TMP_Text playerTextTotalTime;
    
    
    [SerializeField] private Slider volumeSlider;
    
    [SerializeField] private GameObject buttonPlayGO;
    [SerializeField] private GameObject buttonPauseGO;
    
    
    
    
    void Start()
    {
        buttonSave.interactable = false;
        playerBar.SetActive(false);
    }


    public void SetValuesForNewlySongLoad(float songLenght)
    {
        playerSlider.maxValue = songLenght;
        playerSlider.SetValueWithoutNotify(0);
        playerTextTotalTime.text = ConvertFloatToTimeString(songLenght);
        playerTextCurrentTime.text = ConvertFloatToTimeString(songLenght);
        
        SetButtonsPlayPause(true);
        
        playerBar.SetActive(true);
    }
    public void NoProjectOpened()
    {
        buttonSave.interactable = false;
        playerBar.SetActive(false);
    }


    public void Play()
    {
        audioProjectManager.TryResume();
        SetButtonsPlayPause(false);
    }
    public void Pause()
    {
        audioProjectManager.TryPause();
        SetButtonsPlayPause(true);
    }
    public void Stop()
    {
        audioProjectManager.TryStop();
        SetButtonsPlayPause(true);
    }

    public void AskToOpenFileExplorer() { audioProjectManager.TryOpenFileExplorer(); }
    public void AskToInitialize() { //TODO but shouldn't be needed
    }
    public void AskToSaveProject() { audioProjectManager.TrySaveProject(); }
    

    public void PlayFromSliderTime()
    {
        //TODO
        audioProjectManager.TryPlaySongFrom(playerSlider.value);
    }

    /*
    .___  ___.  _______ .___________. __    __    ______    _______       _______.
    |   \/   | |   ____||           ||  |  |  |  /  __  \  |       \     /       |
    |  \  /  | |  |__   `---|  |----`|  |__|  | |  |  |  | |  .--.  |   |   (----`
    |  |\/|  | |   __|      |  |     |   __   | |  |  |  | |  |  |  |    \   \    
    |  |  |  | |  |____     |  |     |  |  |  | |  `--'  | |  '--'  |.----)   |   
    |__|  |__| |_______|    |__|     |__|  |__|  \______/  |_______/ |_______/                                                                         
     */
    private void SetButtonsPlayPause(bool value)
    {
        buttonPlayGO.SetActive(value);
        buttonPauseGO.SetActive(!value);
    }
    public string ConvertFloatToTimeString(float time) => $"{Mathf.FloorToInt(time / 60)}:{Mathf.FloorToInt(time % 60).ToString("00")}";



}
