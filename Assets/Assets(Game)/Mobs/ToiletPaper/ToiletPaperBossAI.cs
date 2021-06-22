using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class ToiletPaperBossAI : MonoBehaviour
{
    /*
        0 - 30: Base
    30, 31, 32: lazar
        35: spin lazar (Done)
        38: circular summon
        40 - 48: jitter chill
        53: spin lazar (Done)
        55: circular summon
    61, 62, 63, 64: Square corner summon hear thingy
        67: spin lazar (Done)
        69: circular summon
        70 - 84: jitter chill
        85: spin lazar (Done)
        87: circular summon
        88 - 96: jitter chill
        96 - 160: jitter chill + summon every beat

    160 - 224: unknown (Chill melody)

    (Rising action)
        239: circular summon
        240: Circular summon

        247: Ciruclar summon
        248: Circular summon

        255: Ciruclar summon
        256: Circular summon

        265: Circular summon
        267: Circular summon
        269: Circular summon

        283: spin lazar (Done)
        285: circular summon
        286 - 296: jitter chill
        301: spin lazar (Done)
        303: circular summon
    310 - 312: Square corner summon heart thingy
        333: spin lazar (Done)
        335: circular summon

    44 - 51 (bar) - Chill melody
    52 - 55 - Rising action melody

        443: spin lazar (Done)
        445: circular summon
        446 - 460: jitter chill
        461: spin lazar (Done)
        463: circular summon
        464 - 469: jitter chill
    469, 470, 471, 472: Square corner summon heart thingy
        483: spin lazar (Done)
        485: circular summon
        493: spin lazar (Done)
        495: circular summon

    Rest of song: agro insta kill


     */
    public GameObject mainAgro;

    public GameObject Lazar;
    public GameObject TP;
    public BeatCounter bc;

    public GameObject Circularsummon;

    public AudioSource toilet_paper;

    public string state = "NotRunning";
    public Vector3 TargetPos;

    int donebeat = -100;

    public bool Active = false;

    private void OnEnable()
    {
        BossFight();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (bc.currentbeat != donebeat) EditState();
    }

    public void BossFight()
    {
        if (mainAgro.name == "GameManager") mainAgro = NetworkClient.connection.identity.gameObject;
        Active = true;
        toilet_paper.gameObject.SetActive(true);
        bc.Started = true;
        bc.currentbeat = 1;
        toilet_paper.Play(0);
    }

    void EditState()
    {
        donebeat = bc.currentbeat;
        if (bc.currentbeat == 35 ||
            bc.currentbeat == 53 ||
            bc.currentbeat == 67 ||
            bc.currentbeat == 85 ||
            bc.currentbeat == 283 ||
            bc.currentbeat == 301 ||
            bc.currentbeat == 333 ||
            bc.currentbeat == 443 ||
            bc.currentbeat == 461 ||
            bc.currentbeat == 483 ||
            bc.currentbeat == 493) LazarSpin();
        else if (bc.currentbeat == 38 ||
            bc.currentbeat == 53 ||
            bc.currentbeat == 55 ||
            bc.currentbeat == 69 ||
            bc.currentbeat == 87 ||
            bc.currentbeat == 239 ||
            bc.currentbeat == 240 ||
            bc.currentbeat == 247 ||
            bc.currentbeat == 248 ||
            bc.currentbeat == 255 ||
            bc.currentbeat == 256 ||
            bc.currentbeat == 265 ||
            bc.currentbeat == 267 ||
            bc.currentbeat == 269 ||
            bc.currentbeat == 285 ||
            bc.currentbeat == 303 ||
            bc.currentbeat == 335 ||
            bc.currentbeat == 445 ||
            bc.currentbeat == 463 ||
            bc.currentbeat == 485 ||
            bc.currentbeat == 495) CircularSummon();
        else if (bc.currentbeat == 469 || bc.currentbeat == 470 || bc.currentbeat == 471 || bc.currentbeat == 472 ||
            bc.currentbeat == 310 || bc.currentbeat == 311 || bc.currentbeat == 312 ||
            bc.currentbeat == 61 || bc.currentbeat == 62 || bc.currentbeat == 63 || bc.currentbeat == 64) state = "CornerSummon";
        else if (bc.currentbeat == 1 || bc.currentbeat == 2) TargetPos = mainAgro.transform.position;
        else jittersummon();
    }

    void CircularSummon()
    {
        state = "CircularSummon";
        Debug.Log("CircularSummon");
        Instantiate(Circularsummon, new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z), Quaternion.Euler(0, 0, 0));
    }

    void LazarSpin()
    {
        Lazar.SetActive(true);
        state = "LazarSpin";
        Debug.Log("LazarSpin");
    }

    void jittersummon() {
        state = "JitterSummon";
        Debug.Log("jitterSummon");
        TargetPos = mainAgro.transform.position;
    }
}
