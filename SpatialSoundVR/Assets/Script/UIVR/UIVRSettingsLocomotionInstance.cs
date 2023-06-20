using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpatialSoundVR
{
    public class UIVRSettingsLocomotionInstance : MonoBehaviour
    {
        [SerializeField] private UIVRSettingsManager uivrSettingsManager;

        [SerializeField] private Toggle toggleSmoothLocomotion;
        [SerializeField] private Slider sliderSmoothLocomotion;
        [SerializeField] private TMP_Text sliderSmoothLocomotionText;

        [SerializeField] private Toggle toggleSmoothTurn;
        [SerializeField] private Slider sliderSmoothTurn;
        [SerializeField] private TMP_Text sliderSmoothTurnText;
        [SerializeField] private Slider sliderSnapTurn;
        [SerializeField] private TMP_Text sliderSnapTurnText;

        [SerializeField] private Toggle toggleFlyMode;


        public void UseSmoothLocomotion(bool value)
        {
            uivrSettingsManager.SmoothLocomotion(value);
        }
        public void UseSmoothTurn(bool value)
        {
            uivrSettingsManager.SmoothTurn(value);
        }
        public void UseFlyMode(bool value)
        {
            uivrSettingsManager.FlyMode(value);
        }
        
        public void UseValueSmoothLocomotion()
        {
            uivrSettingsManager.SetValueSmoothLocomotion(sliderSmoothLocomotion.value);
        }
    
        public void UseValueSmoothTurn()
        {
            uivrSettingsManager.SetValueSmoothTurn(sliderSmoothTurn.value);
        }

        public void UseValueSnapTurn()
        {
            uivrSettingsManager.SetValueSnapTurn(sliderSnapTurn.value);
        }



        /*
        ____    ____  __       _______. __    __       ___       __      
        \   \  /   / |  |     /       ||  |  |  |     /   \     |  |     
         \   \/   /  |  |    |   (----`|  |  |  |    /  ^  \    |  |     
          \      /   |  |     \   \    |  |  |  |   /  /_\  \   |  |     
           \    /    |  | .----)   |   |  `--'  |  /  _____  \  |  `----.
            \__/     |__| |_______/     \______/  /__/     \__\ |_______|                                                   
         */
        //STATES
        public void UISetStateSmoothLocomotion(bool value)
        {
            toggleSmoothLocomotion.SetIsOnWithoutNotify(value);
            sliderSmoothLocomotion.interactable = value;
        }

        public void UISetSatesSlidersSmoothSnapTurn(bool value)
        {
            toggleSmoothTurn.SetIsOnWithoutNotify(value);
            sliderSnapTurn.gameObject.SetActive(!value);
            sliderSmoothTurn.gameObject.SetActive(value);
        }

        public void UISetStateFlyMode(bool value)
        {
            toggleFlyMode.SetIsOnWithoutNotify(value);
        }

        //VALUES
        public void UISetValueSmoothLocomotion(float speedRaw, string speedString)
        {
            sliderSmoothLocomotion.SetValueWithoutNotify(speedRaw);
            sliderSmoothLocomotionText.text = speedString;
        }

        public void UISetValueSliderSmoothTurn(float speed, string speedString)
        {
            sliderSmoothTurn.SetValueWithoutNotify(speed);
            sliderSmoothTurnText.text = speedString;
        }

        public void UISetValuesSliderSnapTurn(float amountRaw, string amountString)
        {
            sliderSnapTurn.SetValueWithoutNotify(amountRaw);
            sliderSnapTurnText.text = amountString;
        }


        //PREVIEW
        public void UpdatePreviewSliderSmoothLocomotion()
        {
            float speed = ((float) Math.Pow(sliderSmoothLocomotion.value,4f))/2500000;
            sliderSmoothLocomotionText.text = uivrSettingsManager.FormatSpeed(speed);
        }
        public void UpdatePreviewSliderSmoothTurn()
        {
            sliderSmoothTurnText.text = uivrSettingsManager.FormatAngularSpeed(sliderSmoothTurn.value);
        }
        public void UpdatePreviewSliderSnapTurn()
        {
            sliderSnapTurnText.text = uivrSettingsManager.FormatAngle(sliderSnapTurn.value*15);
        }






    }
}