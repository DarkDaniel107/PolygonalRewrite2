using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitmarkerScript : MonoBehaviour
{
    public TextMesh text;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    public IEnumerator summoned() {
        yield return new WaitForSeconds(5);
        gameObject.SetActive(false);
    }
}
