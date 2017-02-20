using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConveyorScript : MonoBehaviour {
    public GameObject conveyorBelt;
    public float power = 0.15f;
    public bool frozen;

    private UnityAction listener;

    private void Awake()
    {
        listener = new UnityAction(Freeze);
    }

    void OnEnable()
    {
        EventManager.StartListening("Looking", listener);
    }

    void OnDisable()
    {
        EventManager.StopListening("Looking", listener);
    }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay(Collider other)
    {
        if (other.attachedRigidbody && !frozen)
        {
            other.attachedRigidbody.AddForce(conveyorBelt.transform.forward.normalized * power, ForceMode.Force);
        }
    }

    void Freeze()
    {
        frozen = !frozen;
    }

}
