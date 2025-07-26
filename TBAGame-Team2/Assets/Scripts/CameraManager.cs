using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera mainCamera;
    public CinemachineVirtualCamera zoomCamera;

    public bool isZoomed = false;

    public void TriggerZoom()
    {
        zoomCamera.Priority = 20;
        isZoomed = true;
    }

    public void ResetZoom()
    {
        zoomCamera.Priority = 0;
        isZoomed = false;
    }
    

    // Update is called once per frame
    void Update()
    {
         if (Input.GetKeyDown(KeyCode.Z))
            TriggerZoom();

        if (Input.GetKeyDown(KeyCode.X))
            ResetZoom();
    }
}
