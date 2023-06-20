using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

namespace SpatialSoundVR
{
    public class UIVRSettingsManager : MonoBehaviour
    {

        [SerializeField] private UIVRSettingsLocomotionInstance[] uiVrSettingsLocomotionInstances;

        [SerializeField] private ActionBasedSnapTurnProvider actionBasedSnapTurnProvider;
        [SerializeField] private ActionBasedContinuousTurnProvider actionBasedContinuousTurnProvider;
        [SerializeField] private ActionBasedControllerManager leftActionBasedControllerManager;
        [SerializeField] private ActionBasedControllerManager rightActionBasedControllerManager;
        [SerializeField] private DynamicMoveProvider dynamicMoveProvider;

        void Awake()
        {
            SmoothLocomotion(false);
            SmoothTurn(false);
            FlyMode(false);
            SetValueSmoothTurn(160);
            float defaultSpeed = 1;
            SetValueSmoothLocomotion((float)(Math.Pow((2500000*defaultSpeed),1f/4f)));
            SetValueSnapTurn(45/15);
        }
        

        //ENABLING AND DISABLING
        public void SmoothLocomotion(bool value)
        {
            //dynamicMoveProvider.enabled = value;
            leftActionBasedControllerManager.smoothMotionEnabled = value;
            foreach (UIVRSettingsLocomotionInstance uiVrSettingsLocomotionInstance in uiVrSettingsLocomotionInstances)
            {
                uiVrSettingsLocomotionInstance.UISetStateSmoothLocomotion(value);
            }
        }

        public void SmoothTurn(bool value)
        {
            rightActionBasedControllerManager.smoothTurnEnabled = value;
            foreach (UIVRSettingsLocomotionInstance uiVrSettingsLocomotionInstance in uiVrSettingsLocomotionInstances)
            {
                uiVrSettingsLocomotionInstance.UISetSatesSlidersSmoothSnapTurn(value);
            }
        }
        
        public void FlyMode(bool value)
        {
            dynamicMoveProvider.enableFly = value;
            foreach (UIVRSettingsLocomotionInstance uiVrSettingsLocomotionInstance in uiVrSettingsLocomotionInstances)
            {
                uiVrSettingsLocomotionInstance.UISetStateFlyMode(value);
            }
        }

        
        
        
        //VALUES
        public void SetValueSmoothLocomotion(float speedRaw)
        {
            float speed = ((float) Math.Pow(speedRaw,4f))/2500000;
            dynamicMoveProvider.moveSpeed = speed;
            foreach (UIVRSettingsLocomotionInstance uiVrSettingsLocomotionInstance in uiVrSettingsLocomotionInstances)
            {
                uiVrSettingsLocomotionInstance.UISetValueSmoothLocomotion(speedRaw,FormatSpeed(speed));
            }
        }

        public void SetValueSnapTurn(float amountRaw)
        {
            float amount = amountRaw * 15;
            actionBasedSnapTurnProvider.turnAmount = amount;
            foreach (UIVRSettingsLocomotionInstance uiVrSettingsLocomotionInstance in uiVrSettingsLocomotionInstances)
            {
                uiVrSettingsLocomotionInstance.UISetValuesSliderSnapTurn(amountRaw,FormatAngle(amount));
            }
        }
        
        public void SetValueSmoothTurn(float speed)
        {
            actionBasedContinuousTurnProvider.turnSpeed = speed;
            foreach (UIVRSettingsLocomotionInstance uiVrSettingsLocomotionInstance in uiVrSettingsLocomotionInstances)
            {
                uiVrSettingsLocomotionInstance.UISetValueSliderSmoothTurn(speed,FormatAngularSpeed(speed));
            }
        }
        
        
        
        

    
        //METHODS    
        public string FormatSpeed(float speed)
        {
            string formattedSpeed = speed.ToString("F2") + "m/s";
            return formattedSpeed;
        }
        public string FormatAngle(float angle)
        {
            string formattedAngle = angle.ToString("F0") + "°";
            return formattedAngle;
        }
        public string FormatAngularSpeed(float speed)
        {
            string formattedSpeed = speed.ToString("F0") + "°/s";
            return formattedSpeed;
        }
    }
}