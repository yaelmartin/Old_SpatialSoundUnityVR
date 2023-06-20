using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace SpatialSoundVR
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] private GameObject RigVR;
        [SerializeField] private GameObject Rig2D;

        private bool _isVR;

        // Start is called before the first frame update
        void Start()
        {
            UseVR(true);
        }

        public void SwitchRigMode()
        {
            _isVR = !_isVR;

            UseVR(_isVR);
        }
        
        public void UseVR(bool value)
        {
            if (value)
            {
                _isVR = true;
                Rig2D.SetActive(false);
                RigVR.SetActive(true);
            }
            else
            {
                _isVR = false;
                RigVR.SetActive(false);
                Rig2D.SetActive(true);
            }
        }

    }

}
