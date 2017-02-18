using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorScript : MonoBehaviour {
    public GameObject conveyorBelt;
    public float power = 0.15f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay(Collider other)
    {
        Debug.Log("test");
        if (other.attachedRigidbody)
        {
            other.attachedRigidbody.AddForce(conveyorBelt.transform.forward * power, ForceMode.Force);
        }
    }

}
