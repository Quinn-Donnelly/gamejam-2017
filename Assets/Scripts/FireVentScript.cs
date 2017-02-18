using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireVentScript : MonoBehaviour {
    public GameObject fire;
    public AudioSource sound;
    public bool isOn;
    public float flareRate = 2.0f;

    private bool active;
    private MeshRenderer fireMesh;

    // Use this for initialization
    void Start()
    {
        isOn = true;
        active = true;
        sound = GetComponent<AudioSource>();
        fireMesh = this.gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        StartCoroutine(SteamLoop());
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Pressed()
    {
        active = !active;
        isOn = false;
        sound.Stop();
        fireMesh.enabled = false;
    }

    IEnumerator SteamLoop()
    {
        while (enabled)
        {
            if (active)
            {
                if (isOn)
                {
                    isOn = false;
                    sound.Stop();
                    fireMesh.enabled = false;
                }
                else
                {
                    isOn = true;
                    sound.Play();
                    fireMesh.enabled = true;
                }
            }
            yield return new WaitForSeconds(flareRate);
        }
    }
}