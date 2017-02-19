using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DropletScript : MonoBehaviour {
    public GameObject droplet;
    public Rigidbody dropBody;
    public Vector3 tempVel;
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
        droplet = this.gameObject;
        dropBody = droplet.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        Destroy(droplet);
    }

    void Freeze()
    {
        frozen = !frozen;

        if (frozen)
        {
            tempVel = dropBody.velocity;
            dropBody.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            dropBody.constraints = RigidbodyConstraints.None;
            dropBody.velocity = tempVel;
        }
    }
}
