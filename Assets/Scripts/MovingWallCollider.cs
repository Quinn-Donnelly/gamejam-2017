using UnityEngine;
using System.Collections;

public class MovingWallCollider : MonoBehaviour {

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Moving Wall")
        {
            gameObject.SendMessageUpwards("Collied");
        }
    }

}
