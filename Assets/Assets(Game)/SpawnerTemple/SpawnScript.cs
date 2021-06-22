using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnScript : MonoBehaviour
{
    public Transform ObjectPool;

    [HideInInspector]public gamemanager GM;

    public List<GameObject> mobs = new List<GameObject>();

    public bool KillAllMobs = false;

    bool StartedSummoning = false;

    private void Start()
    {
        foreach (Transform mob in ObjectPool) {
            mobs.Add(mob.gameObject);
        }
        GM = GameObject.Find("GameManager").GetComponent<gamemanager>();
    }

    private void Update()
    {
        if (!GM.Started) return;
        if (!StartedSummoning) {
            StartCoroutine(SummonMobs());
        }

        if (KillAllMobs) {
            foreach (GameObject mob in mobs)
            {
                mob.SetActive(false);
            }
            KillAllMobs = false;
        }
    }

    IEnumerator SummonMobs() {
        StartedSummoning = true;
        while (GM.Started) {
                foreach (GameObject mob in mobs) {
                    if (!mob.activeSelf) {
                        mob.transform.position = transform.position;
                        mob.SetActive(true);
                        mob.GetComponent<NavMeshAgent>().SetDestination(findGround(new Vector2(Random.Range(-381.21936f, 151.705933f), Random.Range(218.191376f, -75.4220505f)), new Vector3(0, 0, 0)));
                    }
                }
            yield return new WaitForSeconds(Random.Range(1, 5));
        }
    }

    public Vector3 findGround(Vector2 StartingPosition, Vector3 offset)
    {
        RaycastHit hit;
        Physics.Raycast(new Vector3(StartingPosition.x, 10000, StartingPosition.y), Vector3.down, out hit, maxDistance: 100000);
        return hit.point + offset;
    }
}
