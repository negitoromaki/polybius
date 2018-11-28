using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITrackHandler : MonoBehaviour
{
    // this script is used to make sure a vuforia target is found before
    // showing up the UI

    public CustomTrackableHandler trackHandler;
    public GameObject UIObject;

    private void Update()
    {
        if (trackHandler.isTracked)
        {
            if (!UIObject.activeSelf)
            {
                UIObject.SetActive(true);
            }
        }
        else
        {
            if (UIObject.activeSelf)
            {
                UIObject.SetActive(false);
            }
        }
    }
}
