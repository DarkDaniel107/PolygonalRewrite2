using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class ChatWindow : MonoBehaviour
{
    static readonly ILogger logger = LogFactory.GetLogger(typeof(ChatWindow));
    public InputField chatMessage;
    public Text chatHistory;
    public Scrollbar scrollbar;
    public gamemanager gm;

    public SlashCommander sc;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) OnSend();
    }

    public void OnSend()
    {
        if (chatMessage.text.Trim() == "")
            return;
        string m = chatMessage.text.Trim();
        if (m.StartsWith("/")) {
            sc.ProcessCommand(m);
            return;
        }
        gm.sendChatMessage($"{gm.username}: {chatMessage.text}");
        chatMessage.text = "";
    }


    internal void AppendMessage(string message)
    {
        StartCoroutine(AppendAndScroll(message));
    }

    IEnumerator AppendAndScroll(string message)
    {
        chatHistory.text += message + "\n";

        // it takes 2 frames for the UI to update ?!?!
        yield return null;
        yield return null;

        // slam the scrollbar down
        scrollbar.value = 0;
        chatMessage.text = "";
    }
}
