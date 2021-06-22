using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RichPresence : MonoBehaviour
{
    Discord.Discord discord;
    Discord.ActivityManager activity;
    public bool LoopB = true;
    public string details = "The developer will change these";
    public string state = "l a t e r";
    public bool StartCallbacks = false;
    // Start is called before the first frame update
    void Start()
    {
        discord = new Discord.Discord(773267441227661372, (ulong)Discord.CreateFlags.Default);
        activity = discord.GetActivityManager();
        var presence = new Discord.Activity
        {
            State = state,
            Details = details,
            Timestamps = {
                    Start = DateTimeOffset.Now.ToUnixTimeSeconds()
                }
        };
        activity.UpdateActivity(presence, (result) => {
            if (result == Discord.Result.Ok)
            {
                Debug.Log("All is well");
            }
            else
            {
                Debug.LogError("All is not well");
            }
        });
        StartCoroutine(Loop());
        StartCallbacks = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!StartCallbacks) return;
        discord.RunCallbacks();
    }
    public IEnumerator Loop() {
        while (LoopB) {
            yield return new WaitForSeconds(1);
            var presence = new Discord.Activity {
                State = state,
                Details = details,
            };
            activity.UpdateActivity(presence, (result) => {
                if (result == Discord.Result.Ok)
                {
                    Debug.Log("All is well");
                }
                else {
                    Debug.LogError("All is not well");
                }
            });
            
        }
    }
}
