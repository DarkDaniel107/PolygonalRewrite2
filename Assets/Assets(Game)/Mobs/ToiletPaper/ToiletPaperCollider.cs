using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ToiletPaperCollider : MonoBehaviour
{
    public MeshCollider collider;
    public int DeathTime = 15;
    public int WaitForNoCollider = 1;
    public tpHitboxScript hitbox;
    public ParticleSystem deathSystem;

    bool DestroySt = false;
    ToiletPaperBossAI t;

    private void OnEnable()
    {
        t = GameObject.Find("ToiletPaperBoss").GetComponent<ToiletPaperBossAI>();
        StartCoroutine(waittoDie());
    }

    private void FixedUpdate()
    {
        deathSystem.transform.eulerAngles = new Vector3(-90, 0, 0);
        if (hitbox.Exploding && !DestroySt) StartCoroutine(Destroy());
    }

    IEnumerator waittoDie() {
        yield return new WaitForSeconds(WaitForNoCollider);
        collider.isTrigger = false;
        yield return new WaitForSeconds(DeathTime);
        Die();
    }

    void Die() {
        Destroy(gameObject);
    }

    IEnumerator Destroy()
    {
        DestroySt = true;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        deathSystem.Play();
        yield return new WaitForSeconds(0.1f);
        hitbox.gameObject.SetActive(false);
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
