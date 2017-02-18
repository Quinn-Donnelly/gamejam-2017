using UnityEngine;
using System.Collections;

/*  This class will notify the array of GameObjects upon enter stay and leave of a trigger
 * the names of the events are set in the unity editior. The message will pass the collider to each.
 */

public class TriggerZone : MonoBehaviour {
    
    // These will be the names of the messages that will be issued upon each case
    public string enteringMessageName;
    public string exitMessageName;
    public string stayMessageName;

    // Array of GameObjects that will be notified when each of the three collision cases occur
    public GameObject[] listeners;

    void OnTriggerEnter(Collider other)
    {
        for(int i = 0; i < listeners.Length; ++i)
        {
            listeners[i].SendMessage(enteringMessageName, other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < listeners.Length; ++i)
        {
            listeners[i].SendMessage(exitMessageName, other);
        }
    }

    void OnTriggerStay(Collider other)
    {
        for (int i = 0; i < listeners.Length; ++i)
        {
            listeners[i].SendMessage(stayMessageName, other);
        }
    }
}
