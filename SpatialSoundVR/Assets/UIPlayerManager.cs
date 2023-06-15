using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Slider volumeSlider;
    
    [SerializeField] private GameObject buttonPlayGO;
    [SerializeField] private GameObject buttonPauseGO;
    
    
    void Start()
    {
        buttonSave.interactable = false;
        playerBar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetValuesForNewlySongLoad(float songLenght)
    {

        playerSlider.maxValue = songLenght;
        playerSlider.SetValueWithoutNotify(0);
        
        ButtonsPlayPause(true);
        
        playerBar.SetActive(true);
    }

    public void HideEverything()
    {
        buttonSave.interactable = false;
    }


    public void Play()
    {
        //call the audioproject
        ButtonsPlayPause(false);
    }
    public void Pause()
    {
        //call the audioproject
        ButtonsPlayPause(true);
    }


    public void PlayFromSliderTime()
    {
        audioProjectManager.TryPlaySongFrom(playerSlider.value);
    }

    private void ButtonsPlayPause(bool value)
    {
        buttonPlayGO.SetActive(value);
        buttonPauseGO.SetActive(!value);
    }
}
