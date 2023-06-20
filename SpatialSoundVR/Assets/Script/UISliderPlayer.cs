using SpatialSoundVR;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SpatialSoundVR
{
    public class UISliderPlayer : MonoBehaviour, IPointerUpHandler
    {
        [SerializeField] private Slider slider;
        [SerializeField] private UIPlayerInstance uiPlayerInstance;

        public void OnPointerUp(PointerEventData data)
        {
            uiPlayerInstance.PlayFromSliderTime();
        }
    }
}