using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BeatCounter : MonoBehaviour
{
    public int bpm = 120;
    public int currentbeat = 2;
    public int maxbeat = 0;
    public long secondsWaited;
    public bool Started = false;
    bool internalStarted = false;
    long mtime = 0;
    private void Start()
    {
        secondsWaited = 60000 / bpm;
        Debug.Log(secondsWaited);
    }
    // Update is called once per frame
    void Update()
    {
        
        if (Started && !internalStarted) {
            mtime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            internalStarted = true;
        }

        if (internalStarted && (DateTimeOffset.Now.ToUnixTimeMilliseconds() >= (mtime + secondsWaited))) {
            currentbeat++;
            mtime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
    }
}
