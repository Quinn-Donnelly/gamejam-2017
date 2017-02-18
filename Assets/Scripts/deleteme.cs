using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class deleteme : MonoBehaviour {

    private UnityAction listener;

    private void Awake()
    {
        listener = new UnityAction(someFunc);
    }

    void OnEnable()
    {
        EventManager.StartListening("Looking", listener);
    }

    void OnDisable()
    {
        EventManager.StopListening("Looking", listener);
    }

    void Interact()
    {
        Debug.Log("DONT TOUCH ME!");
    }

    void someFunc()
    {
        Debug.Log("Hello Nick");
    }
}
