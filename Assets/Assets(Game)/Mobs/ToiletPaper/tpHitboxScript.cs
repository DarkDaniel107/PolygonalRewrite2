using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tpHitboxScript : MonoBehaviour
{
    public bool touchingPlayer = false;
    public bool ChainReaction = false;
    public bool Exploding = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("GAMEPLAYER_1Ad34A56AR1")) {
            touchingPlayer = true;
            Exploding = true;
        }
        if (collision.gameObject.name.Contains("ToiletPaperHitbox") && collision.gameObject.GetComponent<tpHitboxScript>().Exploding)
        {
            ChainReaction = true;
            Exploding = true;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name.Contains("GAMEPLAYER_1Ad34A56AR1"))
        {
            touchingPlayer = true;
            Exploding = true;
        }
        if (collision.gameObject.name.Contains("ToiletPaperHitbox") && collision.gameObject.GetComponent<tpHitboxScript>().Exploding)
        {
            ChainReaction = true;
            Exploding = true;
        }
    }
}
