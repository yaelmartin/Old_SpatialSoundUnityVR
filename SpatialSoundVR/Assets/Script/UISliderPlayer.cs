using SpatialSoundVR;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SpatialSoundVR
{
    public class UISliderPlayer : MonoBehaviour, IPointerUpHandler
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private UIPlayerInstance _uiPlayerInstance;

        public void OnPointerUp(PointerEventData data)
        {
            _uiPlayerInstance.PlayFromSliderTime();
        }
    }
}