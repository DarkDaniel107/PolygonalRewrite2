using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;

public class Follow : MonoBehaviour
{
    public GameObject ObjectToFollow = null;
    public NavMeshAgent Agent;
    public bool UseIdentityGameObject;
    gamemanager GM;
    bool StartedParthing = false;
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
    }

    IEnumerator follow() {
        StartedParthing = true;
        while (GM.Started) {
            Agent.SetDestination(ObjectToFollow.transform.position);
            yield return new WaitForSeconds(1);
        }
        yield return new WaitForSeconds(1);
    }
}
