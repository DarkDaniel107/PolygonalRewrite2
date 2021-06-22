using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletBossAnimator : MonoBehaviour
{
    public ToiletPaperBossAI tpba;
    public BeatCounter bc;
    public GameObject TP;

    int currentBeat = -100;
    int frame = 1;
    int lspinframes = 0;

    bool moved = false;
    bool move = false;

    float distence = -1;

    Vector3 StartPos;
    Vector3 EndPos;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!tpba.mainAgro.activeSelf && tpba.Active)
        {
            tpba.Active = false;
            GetComponent<Rigidbody>().isKinematic = false;
            StartCoroutine(Despawn());
        }
    }

    private void FixedUpdate()
    {
        if (!tpba.Active) return;
        if (tpba.state == "LazarSpin" && currentBeat != bc.currentbeat)
        {
            currentBeat = bc.currentbeat;
            lspinframes = 30;
        }
        if (tpba.state == "JitterSummon" && currentBeat != bc.currentbeat && !moved)
        {
            currentBeat = bc.currentbeat;
            move = true;
            distence = Vector3.Distance(transform.position, tpba.TargetPos);
            StartPos = transform.position;
            float radius = 50;
            float angle = Random.Range(0, 99) * Mathf.PI * 2f / 100;
            Vector3 newPos = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
            EndPos = newPos + tpba.TargetPos;
        }
        if (tpba.state == "JitterSummon" && currentBeat != bc.currentbeat && moved)
        {
            currentBeat = bc.currentbeat;
            tpba.TargetPos = tpba.mainAgro.transform.position;
        }

        if (frame <= 20)
        {
            frame++;
            if (tpba.state == "LazarSpin")
            {
                transform.Rotate(new Vector3(0, 18, 0));
            }
            else if (move)
            {

                transform.position = Vector3.MoveTowards(StartPos, EndPos, (distence / 20) * frame);
            }
        }
        else if (frame > 20 && currentBeat == bc.currentbeat)
        {
            Debug.LogError("Assertion failed, frame overload!!!!!");
            Application.Quit();
        }

        lspinframes--;
        if (lspinframes == 0)
        {
            tpba.Lazar.SetActive(false);
            transform.eulerAngles = new Vector3(0, 0, 90);
        }

        if (frame == 20)
        {
            frame = 1;
            if (move)
            {
                GameObject gm = Instantiate(TP, new Vector3(transform.position.x, transform.position.y + 5, transform.position.z), Quaternion.identity);
                gm.GetComponent<Rigidbody>().velocity = new Vector3(transform.forward.x, transform.forward.y * 3, transform.forward.z) * 5;
            }
            move = false;
        }
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(5);
        gameObject.SetActive(false);
    }
}
