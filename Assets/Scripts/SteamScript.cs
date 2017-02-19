using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamScript : MonoBehaviour {
    public float damageRate = 5.0f;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.SendMessage("ApplyPlayerDamage", damageRate);
        Debug.Log("take damage from steam");
    }
}
