using UnityEngine;
using System.Collections;

public class playerInteract : MonoBehaviour {

    // This will determine the max distance the interact can be triggered from
    public float maxInteractDist;
    // This is the string that will be used to get the key code for the interact action
    public string interactButton;

    // When true it will attempt to invoke interaction with object hit
    private bool trigger;


	// Use this for initialization
	void Start () {
        trigger = false;
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(interactButton))
        {
            trigger = true;
        }

        // Check for interact
        if(trigger)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                if (hit.distance <= maxInteractDist)
                {
                    // TODO: delete this line. used to test when interact can be triggered
                    Debug.DrawRay(transform.position, hit.transform.position, Color.red, 0.5f);

                    // Send a message to the game object we hit
                    hit.transform.gameObject.SendMessage("interact");

                    // Reset the trigger
                    trigger = false;
                }
            }
        }
    }
}
