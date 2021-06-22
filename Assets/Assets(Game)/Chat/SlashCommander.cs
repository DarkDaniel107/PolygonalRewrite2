using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SlashCommander : MonoBehaviour
{
    public ChatWindow cw;
    public GameObject toiletpaperboss;
    public void ProcessCommand(string message) {
        string m = message.Replace("/", "");
        string[] inputers = m.Split(' ');
        Debug.Log($"'{inputers[0]}'");
        try
        {
            if (inputers[0] == "summon")
            {
                if (inputers[1] == "toilet_paper")
                {
                    toiletpaperboss.SetActive(true);
                }
                else
                {
                    cw.AppendMessage("Unknown input!");
                }
            }

            else
            {
                cw.AppendMessage("Unknown command!");
            }
        }
        catch (IndexOutOfRangeException i) {
            cw.AppendMessage("Missing argument!");
        }
    }
}
