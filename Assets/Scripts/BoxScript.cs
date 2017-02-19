using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoxScript : MonoBehaviour {
    public GameObject box;
    public Rigidbody boxBody;
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
        box = this.gameObject;
        boxBody = box.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Freeze()
    {
        frozen = !frozen;

        if (frozen)
        {
            tempVel = boxBody.velocity;
            boxBody.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            boxBody.constraints = RigidbodyConstraints.None;
            boxBody.velocity = tempVel;
        }
    }
}
