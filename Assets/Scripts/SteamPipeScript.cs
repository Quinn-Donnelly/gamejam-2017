using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SteamPipeScript : MonoBehaviour {
    public GameObject steam;
    public AudioSource sound;
    public bool isOn;
    public float toggleRate = 2.0f;
    public bool frozen;

    private bool active;
    private MeshRenderer steamMesh;
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
		isOn = true;
        active = true;
        sound = GetComponent<AudioSource>();
        steamMesh = this.gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        StartCoroutine(SteamLoop());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Pressed()
    {
        active = !active;
        isOn = false;
        sound.Stop();
        steamMesh.enabled = false;
    }

    IEnumerator SteamLoop()
    {
        while (enabled)
        {
            if(active)
            {
                if (!frozen)
                {
                    if (isOn)
                    {
                        isOn = false;
                        sound.Stop();
                        steamMesh.enabled = false;
                    }
                    else
                    {
                        isOn = true;
                        sound.Play();
                        steamMesh.enabled = true;
                    }
                }
            }
            yield return new WaitForSeconds(toggleRate);
        }
    }

    void Freeze()
    {
        frozen = !frozen;
        sound.Stop();
    }
}
