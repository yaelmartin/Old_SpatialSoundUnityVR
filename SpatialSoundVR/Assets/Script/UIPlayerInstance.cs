using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpatialSoundVR
{
    public class UIPlayerInstance : MonoBehaviour
    {
        [SerializeField] private UIPlayerManager uiPlayerManager;

        [SerializeField] private Button buttonSave;
        [SerializeField] private GameObject playerBar;
        [SerializeField] private Slider playerSlider;
        [SerializeField] private TMP_Text playerTextCurrentTime;
        [SerializeField] private TMP_Text playerTextTotalTime;
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private GameObject buttonPlayGO;
        [SerializeField] private GameObject buttonPauseGO;

        private void Play()
        {
            uiPlayerManager.Play();
        }

        private void Pause()
        {
            uiPlayerManager.Pause();
        }

        private void Stop()
        {
            uiPlayerManager.Stop();
        }

        private void AskToOpenFileExplorer()
        {
            uiPlayerManager.AskToOpenFileExplorer();
        }

        private void AskToSaveProject()
        {
            uiPlayerManager.AskToSaveProject();
        }


        public void PlayFromSliderTime()
        {
            uiPlayerManager.PlayFromSliderTime(playerSlider.value);
        }

        /// <summary>
        /// Is called at each Player slider OnValueChanged
        /// </summary>
        public void UpdatePreviewPlayerSliderTime()
        {
            playerTextCurrentTime.text = uiPlayerManager.GetTimeStringPreviewPlayerSlider(playerSlider.value);
        }

        /*
        .___  ___.  _______ .___________. __    __    ______    _______       _______.
        |   \/   | |   ____||           ||  |  |  |  /  __  \  |       \     /       |
        |  \  /  | |  |__   `---|  |----`|  |__|  | |  |  |  | |  .--.  |   |   (----`
        |  |\/|  | |   __|      |  |     |   __   | |  |  |  | |  |  |  |    \   \    
        |  |  |  | |  |____     |  |     |  |  |  | |  `--'  | |  '--'  |.----)   |   
        |__|  |__| |_______|    |__|     |__|  |__|  \______/  |_______/ |_______/                                                                         
         */
        public void SetButtonsPlayPause(bool value)
        {
            buttonPlayGO.SetActive(value);
            buttonPauseGO.SetActive(!value);
        }

        public void SetValuesForNewlySongLoad(string textCurrentTime, string textTotalTime)
        {
            playerSlider.SetValueWithoutNotify(0);
            playerTextTotalTime.text = textTotalTime;
            playerTextCurrentTime.text = textCurrentTime;

            buttonSave.interactable = true;

            SetButtonsPlayPause(true);

            playerBar.SetActive(true);
        }

        public void NoProjectOpened()
        {
            buttonSave.interactable = false;
            playerBar.SetActive(false);
        }

        public void SetSlider(float range0To1)
        {
            playerSlider.SetValueWithoutNotify(range0To1);
        }
    }
}
