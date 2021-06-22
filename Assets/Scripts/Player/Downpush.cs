using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Downpush : MonoBehaviour
{
    public bool Push = false;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        Push = false;
    }

    private void OnTriggerExit(Collider other)
    {
        Push = true;
    }
}
