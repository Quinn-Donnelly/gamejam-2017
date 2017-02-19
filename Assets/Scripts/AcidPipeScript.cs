using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AcidPipeScript : MonoBehaviour {
    public GameObject pipe;
    public GameObject droplet;
    public bool frozen;
    public AudioSource sound;
    public Vector3 spawnPoint;
    public float dripRate = 2.0f;

    private bool active;
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
    void Start()
    {
        active = true;
        sound = GetComponent<AudioSource>();
        StartCoroutine(DripLoop());
        spawnPoint = new Vector3(pipe.transform.position.x, pipe.transform.position.y - 1.25f, pipe.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Pressed()
    {
        active = !active;
        sound.Stop();
    }

    IEnumerator DripLoop()
    {
        while (enabled)
        {
            if (active && !frozen)
            {
                Instantiate(droplet, spawnPoint, pipe.transform.rotation);
            }
            yield return new WaitForSeconds(dripRate);
        }
    }

    void Freeze()
    {
        frozen = !frozen;
    }
}