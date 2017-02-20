using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectWatcherTrigger : MonoBehaviour {

    public string objectTag;

    public GameObject[] responders;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == objectTag) {
            foreach (GameObject o in responders) {
                o.SendMessage("Pressed");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == objectTag)
        {
            foreach (GameObject o in responders)
            {
                o.SendMessage("Pressed");
            }
        }
    }
}
