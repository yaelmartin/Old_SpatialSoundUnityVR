using SpatialSoundVR;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SpatialSoundVR
{
    public class UIVRSliderSnapTurn : MonoBehaviour, IPointerUpHandler
    {
        [SerializeField] private Slider slider;
        [SerializeField] private UIVRSettingsLocomotionInstance uiVrSettingsLocomotionInstance;

        public void OnPointerUp(PointerEventData data)
        {
            uiVrSettingsLocomotionInstance.UseValueSnapTurn();
        }
    }
}