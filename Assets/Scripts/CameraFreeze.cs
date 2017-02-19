using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraFreeze : MonoBehaviour {

    private UnityAction eyesOpenListener;
    private UnityAction eyesClosedListener;


    private Component[] cameras;

    void Start()
    {
        cameras = GetComponentsInChildren(typeof(Camera));
    }

    private void Awake()
    {
        eyesOpenListener = new UnityAction(eyesOpen);
        eyesClosedListener = new UnityAction(eyesClosed);
    }

    void OnEnable()
    {
        EventManager.StartListening("Eyes Open", eyesOpenListener);
        EventManager.StartListening("Eyes Closed", eyesClosedListener);
    }

    void OnDisable()
    {
        EventManager.StopListening("Eyes Open", eyesOpenListener);
        EventManager.StopListening("Eyes Closed", eyesClosedListener);
    }

    void eyesOpen()
    {
        Debug.Log("Camera GO");
        for (int i = 0; i < cameras.Length; ++i)
        {
            cameras[i].gameObject.SetActive(true);
        }
    }

    void eyesClosed()
    {
        Debug.Log("Camera Freeze");
        for(int i = 0; i < cameras.Length; ++i)
        {
            cameras[i].gameObject.SetActive(false);
        }
    }

}
