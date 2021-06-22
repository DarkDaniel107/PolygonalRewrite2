using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;

public class SpikedDiskAI : MonoBehaviour
{
    public GameObject ObjectToFollow = null;
    public SpickedDiskAGRO agro;
    public NavMeshAgent Agent;
    public float HesitationLeanthAgro = 0.1f;
    public bool UseIdentityGameObject;
    gamemanager GM;
    bool StartedParthing = false;
    bool Agro = false;

    Vector3 curPos = new Vector3(0, 0, 0);
    Vector3 lastPos = new Vector3(123123, 41243, 124817249);

    int WaitFrames = 0;
    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<gamemanager>();
    }

    
    // Update is called once per frame
    void Update()
    {
        if (!GM.Gettable) { return; }
        if (UseIdentityGameObject && ObjectToFollow == null) { ObjectToFollow = NetworkClient.connection.identity.gameObject; }
        if (!GM.Started) { return; }
        if (!StartedParthing)
        {
            StartCoroutine(follow());
        }

        if (agro.TouchingPlayer) {
            Agro = true;
        }
    }

    void FixedUpdate() {
        transform.Rotate(new Vector3(0, 1, 0));
    }

    IEnumerator follow()
    {
        StartedParthing = true;
        yield return new WaitForSeconds(3);
        while (GM.Started)
        {
            if (Agro)
            {
                Agent.speed = 5;
                Agent.SetDestination(ObjectToFollow.transform.position);
                yield return new WaitForSeconds(HesitationLeanthAgro);
            }
            else {
                curPos = transform.position;
                if (curPos == lastPos)
                {
                    Agent.speed = 1;
                    Agent.SetDestination(RandomNavSphere(transform.position, Random.Range(1, 10), -1));
                    for (int m = 0; m < 1 * 100; m++)
                    {
                        yield return new WaitForSeconds(0.01f);
                        if (Agro) break;
                    }
                }
                lastPos = curPos;
            }
            for (int m = 0; m < 1 * 100; m++) {
                yield return new WaitForSeconds(0.01f);
                if (Agro) break;
            }
        }
        yield return new WaitForSeconds(1);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("GAMEPLAYER_1Ad34A56AR1"))
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die() {
        yield return new WaitForSeconds(0.1f);
        GM.totalEntities -= 1;
        gameObject.SetActive(false);
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }
}
