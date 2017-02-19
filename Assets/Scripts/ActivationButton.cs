using UnityEngine;
using System.Collections;

public class ActivationButton : MonoBehaviour {

    // This will be the hazard that links to the button
    public GameObject[] hazards;

    void Interact()
    {
        GetComponent<AudioSource>().Play();
        Debug.Log("pressed button");
        for(int i = 0; i < hazards.Length; ++i)
        {
            hazards[i].SendMessage("Pressed");
        }
    }
}
