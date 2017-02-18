using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScript : MonoBehaviour {
    public GameObject thing;
    public Transform currentLocation;
    public Transform targetLocation;
    public float speed = 0.5f;
    public bool isLoop;
    public bool isPatrol;
    public Transform[] targets;
    public int targetIterator = 0;

	// Use this for initialization
	void Start () {
        currentLocation = thing.transform;
        targetLocation = targets[0];
	}
	
	// Update is called once per frame
	void Update () {
        currentLocation = thing.transform;
        MoveTo(targetLocation);
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
}
