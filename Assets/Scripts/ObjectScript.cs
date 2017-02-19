using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectScript : MonoBehaviour {
    public GameObject thing;
    public Transform currentLocation;
    public Transform targetLocation;
    public float speed = 0.5f;
    public bool isLoop;
    public bool isPatrol;
    public bool frozen;
    public Transform[] targets;
    public int targetIterator = 0;
    public float damageOnHit;

    private UnityAction listener;

    private void Awake()
    {
        listener = new UnityAction(Freeze);
    }

    void OnEnable()
    {
        EventManager.StartListening("Eyes Open", listener);
        EventManager.StartListening("Eyes Closed", listener);
    }

    void OnDisable()
    {
        EventManager.StopListening("Eyes Open", listener);
        EventManager.StopListening("Eyes Closed", listener);
    }

	// Use this for initialization
	void Start () {
        currentLocation = thing.transform;
        targetLocation = targets[0];
	}
	
	// Update is called once per frame
	void Update () {
        if (!frozen)
        {
            currentLocation = thing.transform;
            MoveTo(targetLocation);
        }
	}

    void MoveTo(Transform target)
    {
        float step = speed * Time.deltaTime;
        thing.transform.position = Vector3.MoveTowards(currentLocation.position, targetLocation.position, step);
        if (Vector3.Distance(thing.transform.position, target.position) < 1)
        {
            if (isLoop)
            {
                if (targetIterator >= targets.Length - 1)
                {
                    targetIterator = 0;
                }
                else
                {
                    targetIterator++;
                }
                targetLocation = targets[targetIterator];
            }
            else if (isPatrol)
            {

            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.SendMessage("ApplyDamage", damageOnHit);
        }
    }

    void Freeze()
    {
        frozen = !frozen;
    }

}
