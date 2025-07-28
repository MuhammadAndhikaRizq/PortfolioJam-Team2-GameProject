using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Fungus;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera mainCamera;
    public CinemachineVirtualCamera zoomCamera;

    public Flowchart flowchart;
    public GameObject UiHelp;
    public GameObject menuUI;

    public bool isZoomed = false;

    public void TriggerZoom()
    {
        Debug.Log("Ke Panggil");
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
        if (isZoomed)
        {
            UiHelp.SetActive(false);
            menuUI.SetActive(false);
        }
        else
        {
            menuUI.SetActive(true);
        }
    }

    void AutoZoom()
    {
        int status = flowchart.GetIntegerVariable("StoryState");
    }
}
