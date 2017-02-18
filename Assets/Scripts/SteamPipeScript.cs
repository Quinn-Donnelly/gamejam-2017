using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamPipeScript : MonoBehaviour {
    public GameObject steam;
    public AudioSource sound;
    public bool isOn;
    public float toggleRate = 2.0f;

	// Use this for initialization
	void Start () {
		isOn = true;
        sound = GetComponent<AudioSource>();
        StartCoroutine(SteamLoop());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator SteamLoop()
    {
        while (enabled)
        {
            if (isOn)
            {
                isOn = false;
                sound.Stop();
                GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                isOn = true;
                sound.Play();
                GetComponent<MeshRenderer>().enabled = true;
            }
            yield return new WaitForSeconds(toggleRate);
        }
    }
}
