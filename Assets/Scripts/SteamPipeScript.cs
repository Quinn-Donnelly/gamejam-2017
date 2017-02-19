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
    private bool isAlwaysOn = false;

    private bool active;
    private MeshRenderer steamMesh;
    public Collider steamCollider;
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
		isOn = true;
        active = true;
        steam = gameObject.transform.GetChild(0).gameObject;
        sound = GetComponent<AudioSource>();
        steamMesh = steam.GetComponent<MeshRenderer>();
        steamCollider = steam.GetComponent<Collider>();

        if (toggleRate <= 0)
        {
            isAlwaysOn = true;
        }
        StartCoroutine(SteamLoop());
        frozen = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Pressed()
    {
        Debug.Log("Pipe toggled");
        active = !active;
        isOn = false;
        steamCollider.enabled = !steamCollider.enabled;
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
                        if (!isAlwaysOn)
                        {
                            sound.Stop();
                            steamMesh.enabled = false;
                            steamCollider.enabled = false;
                        }
                    }
                    else
                    {
                        isOn = true;
                        if (!sound.isPlaying)
                        {
                            sound.Play();
                        }
                        steamMesh.enabled = true;
                        steamCollider.enabled = true;
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
