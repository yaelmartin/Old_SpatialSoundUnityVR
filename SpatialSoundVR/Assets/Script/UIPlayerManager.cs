using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpatialSoundVR
{
    public class UIPlayerManager : MonoBehaviour
    {

        [SerializeField] private AudioProjectManager audioProjectManager;
        [SerializeField] private UIPlayerInstance[] uiPlayerInstances;
        [SerializeField] public float maxAudioLengthSeconds => audioProjectManager.maxAudioLengthSecondsAccessor;

        
        /*
        private BoolTimer _boolTimer;

        /// <summary>
        /// Sets a bool to true for a certain amount of time, then resets
        /// </summary>
        public struct BoolTimer
        {
            float resetTime;

            /// <summary>
            /// Set the value to true for time seconds
            /// </summary>
            public void Set(float time)
            {
                resetTime = Mathf.Max(resetTime, Time.time + time);
            }

            /// <summary>
            /// Set the value to true for time seconds
            /// </summary>
            public void Set(float time, bool overwrite)
            {
                if (overwrite)
                    resetTime = Time.time + time;
                else
                    resetTime = Mathf.Max(resetTime, Time.time + time);
            }

            /// <summary>
            /// Set the value to false and reset timer
            /// </summary>
            public void Reset()
            {
                resetTime = Time.time - 1;
            }

            public bool Value
            {
                get { return Time.time <= resetTime; }
            }

            public static implicit operator bool(BoolTimer bt)
            {
                return bt.Value;
            }
        }

        private bool _needToPlayMusicFromSliderValue;
        private float _lastPlayerTimeValueRaw;
        
        */

        private void Awake()
        {
            UIsNoProjectOpened();
        }
        
        /*
        private void Update()
        {
            // will play the music if needed
            if (_needToPlayMusicFromSliderValue && !_boolTimer)
            {
                _needToPlayMusicFromSliderValue = false;
                bool result = audioProjectManager.TryPlaySongFrom(_lastPlayerTimeValueRaw * maxAudioLengthSeconds);


                if (result)
                {
                    foreach (UIPlayerInstance playerInstance in uiPlayerInstances)
                    {
                        playerInstance.SetButtonsPlayPause(false);
                        playerInstance.SetSlider(_lastPlayerTimeValueRaw);
                    }
                }



            }
        }
        */


        public void Play()
        {
            if (!audioProjectManager.TryResume()) return;
            foreach (UIPlayerInstance playerInstance in uiPlayerInstances)
            {
                playerInstance.SetButtonsPlayPause(false);
            }
        }

        public void Pause()
        {
            if (!audioProjectManager.TryPause()) return;
            foreach (UIPlayerInstance playerInstance in uiPlayerInstances)
            {
                playerInstance.SetButtonsPlayPause(true);
            }
        }

        public void Stop()
        {
            if (!audioProjectManager.TryStop()) return;
            foreach (UIPlayerInstance playerInstance in uiPlayerInstances)
            {
                playerInstance.SetButtonsPlayPause(true);
            }
        }

        public void AskToOpenFileExplorer()
        {
            //audioProjectManager.TryOpenFileExplorer();
            audioProjectManager.TryOpenProjectUsingDefaultPath();
        }
        

        public void AskToSaveProject()
        {
            audioProjectManager.TrySaveProject();
        }


        public void PlayFromSliderTime(float range0To1)
        {
            if (!audioProjectManager.TryPlaySongNowFrom(range0To1 * maxAudioLengthSeconds)){ return; }
            foreach (UIPlayerInstance playerInstance in uiPlayerInstances) {
                playerInstance.SetButtonsPlayPause(false);
                playerInstance.SetSlider(range0To1);
            }
        }

        /*
        .___  ___.  _______ .___________. __    __    ______    _______       _______.
        |   \/   | |   ____||           ||  |  |  |  /  __  \  |       \     /       |
        |  \  /  | |  |__   `---|  |----`|  |__|  | |  |  |  | |  .--.  |   |   (----`
        |  |\/|  | |   __|      |  |     |   __   | |  |  |  | |  |  |  |    \   \    
        |  |  |  | |  |____     |  |     |  |  |  | |  `--'  | |  '--'  |.----)   |   
        |__|  |__| |_______|    |__|     |__|  |__|  \______/  |_______/ |_______/                                                                         
         */

        public void UIsSetValuesForNewlySongLoad()
        {
            string textTotalTime = ConvertFloatToTimeString(maxAudioLengthSeconds);
            string textCurrentTime = ConvertFloatToTimeString(0);
            foreach (UIPlayerInstance playerInstance in uiPlayerInstances)
            {
                playerInstance.SetValuesForNewlySongLoad(textCurrentTime, textTotalTime);
            }
        }
        public string GetTimeStringPreviewPlayerSlider(float range0To1)
        {
            return ConvertFloatToTimeString(range0To1 * maxAudioLengthSeconds);
        }
        


        public void UIsNoProjectOpened()
        {
            foreach (UIPlayerInstance playerInstance in uiPlayerInstances)
            {
                playerInstance.NoProjectOpened();
            }
            
            /*
            _boolTimer.Reset();
            _needToPlayMusicFromSliderValue = false;
            */
        }

        private string ConvertFloatToTimeString(float time) =>
            $"{Mathf.FloorToInt(time / 60)}:{Mathf.FloorToInt(time % 60).ToString("00")}";
    }

/*
 
         foreach (UIPlayerInstance playerInstance in uiPlayerInstances)
        {
            if (playerInstance != null)
            {
            
            }
        }
 */
}



