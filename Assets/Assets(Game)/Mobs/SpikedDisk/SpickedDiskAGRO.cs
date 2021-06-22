using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpickedDiskAGRO : MonoBehaviour
{
    public bool TouchingPlayer = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("GAMEPLAYER_1Ad34A56AR1")) {
            TouchingPlayer = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("GAMEPLAYER_1Ad34A56AR1"))
        {
            TouchingPlayer = true;
        }
    }
}
