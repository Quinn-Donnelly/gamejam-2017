using UnityEngine;
using System.Collections;

public class WallMover : MonoBehaviour {

    // Controls how quickly the wall closes on itself
    public float closingRate;
    // Controls how long between each sucsessive close of the wall
    public float pause;
    
    // The two walls that are closing on one another
    private GameObject wall1;
    private GameObject wall2;

    private int moveDirection;

    // Point in between the two walls
    private Vector3 target;

	// Use this for initialization
	void Start () {
        moveDirection = 1;

        // Grab the two child walls
        wall1 = gameObject.transform.GetChild(0).gameObject;
        wall2 = gameObject.transform.GetChild(1).gameObject;

        target = (wall1.transform.position + wall2.transform.position) / 2f;
	}

    void Update()
    {
        float step = moveDirection * closingRate * Time.deltaTime;
        wall1.transform.position = Vector3.MoveTowards(wall1.transform.position, target, step);
        wall2.transform.position = Vector3.MoveTowards(wall2.transform.position, target, step);
    }

    void Collied()
    {
        moveDirection *= -1;
    }
}
