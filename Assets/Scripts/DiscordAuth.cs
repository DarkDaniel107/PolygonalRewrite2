using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading.Tasks;

public class DiscordAuth : MonoBehaviour
{
    public bool Authing = false;
    public int CODE = 0;
    public gamemanager GM;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Authing) { 
            
        }
    }

    public int GenerateAuthCode(int id) {
        if (!GM.LocalhostServer)
        {
            int code = Random.Range(0, 9999);
            StreamWriter file = new StreamWriter("C:\\Users\\opc\\PycharmProjects\\Polygonal\\AuthCodes.txt", append: true);
            file.WriteLine(code.ToString() + ":" + id.ToString());
            Debug.Log(code);
            CODE = code;
            file.Close();
            return CODE;
        }
        else
        {
            int code = Random.Range(0, 9999);
            StreamWriter file = new StreamWriter("C:\\Users\\DarkDaniel107\\Desktop\\My stuff\\DiscordBots\\Polygonal\\AuthCodes.txt", append: true);
            file.WriteLine(code.ToString() + ":" + id.ToString());
            Debug.Log(code);
            CODE = code;
            file.Close();
            return CODE;
        }
    }

}
