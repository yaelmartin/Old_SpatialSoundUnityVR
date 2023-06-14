using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private GameObject RigVR;
    [SerializeField] private GameObject Rig2D;

    private bool _isVR;  
    
    // Start is called before the first frame update
    void Start()
    {
        _isVR = true;
        SwitchToVR();
    }

    private void SwitchTo2D()
    {
        RigVR.SetActive(false);
        Rig2D.SetActive(true);
    }
    
    private void SwitchToVR()
    {
        Rig2D.SetActive(false);
        RigVR.SetActive(true);
    }

    public void SwitchRigMode()
    {
        _isVR = !_isVR;

        if (_isVR)
        {
            SwitchToVR();
        }
        else
        {
            SwitchTo2D();
        }
    }

    

}
