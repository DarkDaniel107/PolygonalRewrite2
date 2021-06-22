using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitmarkerManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void hitmark(Vector3 position, int damage) {
        foreach (GameObject marker in transform)
        {
            if (!marker.activeSelf)
            {
                marker.transform.position = transform.position;
                marker.SetActive(true);
                
            }
        }
    }
}
