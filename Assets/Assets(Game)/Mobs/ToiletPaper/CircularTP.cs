using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularTP : MonoBehaviour
{
    public List<GameObject> mobs = new List<GameObject>();
    // Start is called before the first frame update
    void OnEnable()
    {
        foreach (Transform mob in transform)
        {
            mobs.Add(mob.gameObject);
        }

        float radius = 5;
        for (int i = 0; i < mobs.Count; i++)
        {
            float angle = i * Mathf.PI * 2f / mobs.Count;
            Vector3 newPos = new Vector3(Mathf.Cos(angle) * radius, transform.position.y + 2, Mathf.Sin(angle) * radius);
            mobs[i].GetComponent<Rigidbody>().velocity = newPos;
            mobs[i].GetComponent<ToiletPaperCollider>().DeathTime = 240;
            mobs[i].GetComponent<ToiletPaperCollider>().WaitForNoCollider = 10;
        }
    }

    IEnumerable WaitSecond() {
        yield return new WaitForSeconds(0.4f);
        float radius = 5;
        for (int i = 0; i < mobs.Count; i++)
        {
            float angle = i * Mathf.PI * 2f / mobs.Count;
            Vector3 newPos = new Vector3(Mathf.Cos(angle) * radius, transform.position.y + 2, Mathf.Sin(angle) * radius);
            mobs[i].GetComponent<Rigidbody>().velocity = newPos;
        }
    }

}
