using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System.Net;
using kcp2k;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class gamemanager : Mirror.NetworkManager
{
    /*
    0 = ball
    1 = cube
     */
    public int race = 0;

    public AudioSource PolySpawn;
    public AudioSource PolyTitle;
    public AudioSource ToiletPaperboss;

    public float IncreaseVolumeAmount = 0.01f;

    public bool I_Polyspawn = false;
    bool P_Polyspawn = false;

    bool I_Polytitle = true;
    bool P_Polytitle = true;
    bool I_ToiletPaper = true;
    bool P_ToiletPaper = true;

    [HideInInspector] public bool Started = false;
    [HideInInspector] public bool Loading = false;
    [HideInInspector] public bool Gettable = false;

    [HideInInspector] public Movement Player = null;
    public GameObject BeforeGame;
    public GameObject Game;
    public GameObject MainMenu;
    public GameObject LoadingScreen;
    public GameObject Audio;
    public GameObject GameCanvas;
    public GameObject Game2Canvas;
    public GameObject DeathScreen;

    public GameObject toiletPaper;

    public GameObject SpikedDiskGameObject;

    public Transform MobGroup;

    public float health = 100;
    public float maxhealth = 0;
    bool death = false;

    public Text LoadingReason;
    public Text HealthText;

    public float SpikedDiskMobDamage = 5;

    public int MaxEntities = 50;

    public int totalEntities = 0;

    string ip;
    public KcpTransport KCPTRANSPORT;
    protected bool isAdmin = false;

    public DiscordAuth DA;

    public GameObject AuthObejct;
    public Text Code;
    public Text HelpfulCode;

    bool tryConnect = true;
    public string forceArea = "";
    int maxID = 0;
    bool OverrideThing = true;
    public bool LocalhostServer = false;
    public GameObject NoConnectBackround;
    bool SendAuthr = false;
    bool AutoConnect = false;
    public GameObject Chat;
    public string username;
    public ChatWindow chatwindow;
    int currentClientID = -1;
    private void Start()
    {
        NetworkClient.RegisterHandler<SendAuth>(HasAuth);
        NetworkClient.RegisterHandler<AuthSucsessMessage>(AuthSucsess);
        NetworkClient.RegisterHandler<ChatMessageToClients>(ClientGetChatMessageMethod);

        NetworkServer.RegisterHandler<GetAuth>(SendAuthmethod);
        NetworkServer.RegisterHandler<ChatMessagetoServer>(ServerGetChatMessageMethod);
        if (!OverrideThing)
        {
            try
            {
                // This function does not affect the thingy
                try
                {
                    StreamReader adminFile = new StreamReader("C:\\Users\\opc\\Desktop\\POLYGONAL_SERVER_SIDE");
                    adminFile.Close();
                    isAdmin = true;
                    Debug.Log("[Admin] Detected admin file. isAdmin = true");
                    Debug.Log("What port do you want the server to be on?");
                    string portGet = Console.ReadLine();
                    if (portGet == " ") {
                        portGet = "42069";
                    }
                    Debug.Log($"Setting port to {portGet}...");
                    KCPTRANSPORT.Port = Convert.ToUInt16(portGet);
                    singleton.StartHost();
                }
                catch (FileNotFoundException)
                {
                    if (!isAdmin)
                    {
                        isAdmin = false;
                        LocalhostServer = true;
                        Debug.Log("[Admin] Did not detect admin file. isAdmin = false");
                    }
                }
            }
            catch (DirectoryNotFoundException)
            {
                try
                {
                    StreamReader adminFile = new StreamReader("C:\\Users\\DarkDaniel107\\Desktop\\PolygonalAdmin.txt");
                    adminFile.Close();
                    string portGet = Console.ReadLine();
                    if (portGet == " ")
                    {
                        portGet = "42069";
                    }
                    isAdmin = true;
                    LocalhostServer = true;
                    Debug.Log("[Admin] Detected admin file. isAdmin = true");
                    Debug.Log("What port do you want the server to be on?");
                    portGet = Console.ReadLine();
                    Debug.Log($"Setting port to {portGet}...");
                    Debug.Log(LocalhostServer);
                    KCPTRANSPORT.Port = Convert.ToUInt16(portGet);
                    singleton.StartHost();
                }
                catch (FileNotFoundException)
                {
                    isAdmin = false;
                    Debug.Log("[Admin] Did not detect admin file. isAdmin = false (UpperScan)");
                }
                if (!isAdmin)
                {
                    isAdmin = false;
                    Debug.Log("[Admin] Did not detect admin file. isAdmin = false");
                }
            }
            try
            {
                StreamReader read = new StreamReader("C:\\Users\\opc\\PycharmProjects\\Polygonal\\StringDataBack.txt");
            }
            catch (DirectoryNotFoundException)
            {
                LocalhostServer = true;
            }
            
        }
        // End function

        if (!isAdmin)
        {
            LoadingReason.text = "Connecting to Vanguard server...";
            maxhealth = health;
            if (!isAdmin && tryConnect)
            {
                singleton.StartClient();
                StartCoroutine(Connect());
            }

        }
        else {
            SinglePlayerButtonPressed();
        }
        
        
    }
    
    void Update()
    {
        Debug.Log(LocalhostServer);
        if (!Gettable) return;
        if (Player == null) {
            Player = NetworkClient.connection.identity.gameObject.GetComponent<Movement>();
        }
        if (!Started) return;
        if (toiletPaper.activeSelf)
        {
            forceArea = "boss.TP";
        }
        else {
            forceArea = "";
        }
        if (forceArea == "")
        {
            I_ToiletPaper = false;
            if (!I_Polyspawn && Player.Area == "spawn")
            {
                PolySpawn.gameObject.SetActive(true);
                I_Polyspawn = true;
            }

            if (I_Polyspawn && !(Player.Area == "spawn"))
            {
                I_Polyspawn = false;
            }

            if (!I_Polytitle && Player.Area == "other")
            {
                I_Polytitle = true;
            }

            if (I_Polytitle && !(Player.Area == "other"))
            {
                I_Polytitle = false;
            }
        }
        else {
            I_Polytitle = false;
            I_Polyspawn = false;
            if (!I_ToiletPaper && forceArea == "boss.TP")
            {
                I_ToiletPaper = true;
            }

            if (I_ToiletPaper && !(forceArea == "boss.TP"))
            {
                I_ToiletPaper = false;
            }
        }

        health -= Player.SPIKEDDISKMOBHITS * SpikedDiskMobDamage;
        Player.SPIKEDDISKMOBHITS = 0;
        HealthText.text = $"Health: {health}/{maxhealth}";
        if (health <= 0) {
            death = true;
            DeathScreen.SetActive(true);
            Player.gameObject.SetActive(false);
        }
        if (isAdmin) {
            Audio.SetActive(false);
            PolySpawn.gameObject.SetActive(false);
            PolyTitle.gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (I_Polyspawn) {
            P_Polyspawn = true;
            if (PolySpawn.volume < 1) {
                PolySpawn.volume += IncreaseVolumeAmount;
            }
        }

        else if (P_Polyspawn) {
            if (PolySpawn.volume > 0)
            {
                PolySpawn.volume -= IncreaseVolumeAmount;
            }
        }

        if (I_Polytitle)
        {
            P_Polyspawn = true;
            if (PolyTitle.volume < 1)
            {
                PolyTitle.volume += IncreaseVolumeAmount;
            }
        }

        if (!I_Polytitle && P_Polytitle)
        {
            if (PolyTitle.volume > 0)
            {
                PolyTitle.volume -= IncreaseVolumeAmount;
            }
        }

        if (I_ToiletPaper)
        {
            P_Polyspawn = true;
            if (ToiletPaperboss.volume < 1)
            {
                ToiletPaperboss.volume += IncreaseVolumeAmount;
            }
        }

        if (!I_ToiletPaper && P_ToiletPaper)
        {
            if (ToiletPaperboss.volume > 0)
            {
                ToiletPaperboss.volume -= IncreaseVolumeAmount;
            }
        }
    }

    public void SinglePlayerButtonPressed() {
        Debug.Log("Starting singleplayer");
        Loading = true;
        MainMenu.SetActive(false);
        LoadingScreen.SetActive(true);
        LoadingReason.text = "Starting singleplayer world...";
        singleton.StartHost();
        StartCoroutine(WaitToStart());
    }

    void SetupTerrain() {
        StartCoroutine(LoadParticles());
    }

    IEnumerator WaitToStart() {
        yield return new WaitForSeconds(1);
        Gettable = true;
        Game.SetActive(true);

        SetupTerrain();
    }

    IEnumerator LoadParticles() {
        LoadingReason.text = "Settling particles...";
        yield return new WaitForSeconds(5);
        LoadingReason.text = "Done!";
        yield return new WaitForSeconds(1);
        Loading = false;
        Started = true;
        BeforeGame.SetActive(false);
        Audio.SetActive(true);
        GameCanvas.SetActive(true);
        Game2Canvas.SetActive(true);
        Chat.SetActive(true);
        I_Polytitle = false;
    }

    IEnumerator Auth() {
        while (!NetworkClient.isConnected)
        {
            yield return new WaitForSeconds(0.1f);
        }
        NetworkClient.Send(new GetAuth() { });
    }

    IEnumerator TryAuth(NetworkConnection conn, int id)
    {
        string path = "";
        string LatestFail;
        if (!LocalhostServer)
        {
            path = "C:\\Users\\opc\\PycharmProjects\\Polygonal\\StringDataBack.txt";
        }
        else {
            path = "C:\\Users\\DarkDaniel107\\Desktop\\My stuff\\DiscordBots\\Polygonal\\StringDataBack.txt";
        }
        int failed = 0;
        bool done = false;
        while (!done)
        {
            StreamReader read;
            StreamWriter write;
            try
            {
                 read = new StreamReader(path);
            }
            catch (IOException){
                continue;
            }
            string data = read.ReadToEnd();
            string[] split = data.Split('*');
            foreach (string datasection in split)
            {
                try
                {
                    LatestFail = datasection.Split('/').ToString();
                    if (datasection.Split('/')[2] == id.ToString())
                    {
                        AuthSucsessMessage authSucsessMessage = new AuthSucsessMessage() { name = datasection.Split('/')[0], id = datasection.Split('/')[1] };
                        read.Close();
                        data = data.Replace(datasection + "*", "");
                        try
                        {
                            write = new StreamWriter(path);
                        }
                        catch (IOException) {
                            break;
                        }
                        write.Write(data);
                        write.Close();
                        conn.Send(authSucsessMessage);
                        done = true;
                        break;
                    }
                }
                catch (IndexOutOfRangeException) {
                    failed++;
                }
                if (failed > 10) {
                    Debug.LogError("FAILED GRATER THAN TEN DYING IMMEDIATLY");
                    yield break;
                }
            }
            read.Close();
            yield return new WaitForSeconds(1);
        }
    }

    public void DeathThingIguesstoendthegameidontknowwhattonamethisohwellsoletmetellyouastoryaboutdyingonedayiwasgoingonawalk() {
        Application.Quit();
    }

    [Server]
    void SendAuthmethod(NetworkConnection conn, GetAuth getAuth) {
        conn.Send(new SendAuth() { code = DA.GenerateAuthCode(maxID) });
        StartCoroutine(TryAuth(conn, maxID));
        maxID += 1;
    }

    [Client]
    void HasAuth(NetworkConnection conn, SendAuth sendAuth) {
        SendAuthr = true;
        AuthObejct.SetActive(true);
        MainMenu.SetActive(false);
        LoadingScreen.SetActive(false);

        Code.text = sendAuth.code.ToString();
        HelpfulCode.text = $"Please do \n.auth {sendAuth.code} \nin the Polygonal discord, bot - commands channel.";

    }

    [Client]
    void AuthSucsess(NetworkConnection conn, AuthSucsessMessage ASM)
    {
        Debug.Log("AuthSucsess!");
        username = ASM.name;
        if (AutoConnect)
        {
            MutiplayerInit();
        }
        else
        {
            LoadingScreen.SetActive(false);
            AuthObejct.SetActive(false);
            MainMenu.SetActive(true);
        }
    }

    [Client]
    IEnumerator Connect() {
        LoadingScreen.SetActive(true);
        AuthObejct.SetActive(false);
        MainMenu.SetActive(false);
        bool e = false;
        for (int F = 0; F < 10; F++)
        {
            yield return new WaitForSeconds(1);
            if (NetworkClient.isConnected)
            {
                LoadingReason.text = "Waiting for authination data...";
                e = true;
                break;
            }
        }
        if (e)
        {
            NetworkClient.Send(new GetAuth() { });
            yield return new WaitForSeconds(10);
            if (!SendAuthr)
            {
                LoadingReason.text = "Failed to get authination data, disconnecting...";
                yield return new WaitForSeconds(2);
                ServerNonConnectable();
            }
        }
        else 
        {
            Debug.Log("NonConnectable");
            LoadingReason.text = "Could not connect to server.";
            yield return new WaitForSeconds(2);
            ServerNonConnectable();
        }
    }

    public void ServerNonConnectable() {
        NetworkClient.Disconnect(); 
        NoConnectBackround.SetActive(true);
        LoadingScreen.SetActive(false);
        AuthObejct.SetActive(false);
        MainMenu.SetActive(true);
    }

    public void MutiplayerButton() {
        if (NetworkClient.isConnected)
        {
            MutiplayerInit();
        }
        else {
            singleton.StartClient();
            StartCoroutine(Connect());
            AutoConnect = true;
        }
    }

    void MutiplayerInit() {
        UnityEngine.Diagnostics.Utils.ForceCrash(UnityEngine.Diagnostics.ForcedCrashCategory.Abort);
        Chat.SetActive(true);
    }

    public void sendChatMessage(string Message) {
        NetworkClient.Send(new ChatMessagetoServer() { message = Message });
    }

    void ServerGetChatMessageMethod(NetworkConnection conn, ChatMessagetoServer message) {
        Debug.Log("Chatmessage");
        NetworkServer.SendToAll(new ChatMessageToClients() { message = message.message });
        if (LocalhostServer) {
            Debug.Log("Chatmessage2");
            using (StreamWriter sw = File.AppendText("C:\\Users\\DarkDaniel107\\Desktop\\My stuff\\DiscordBots\\Polygonal\\DiscordChatSync.txt"))
            {
                sw.WriteLine(message.message);
            }
        }
    }

    void ClientGetChatMessageMethod(NetworkConnection conn, ChatMessageToClients message) { 
        chatwindow.AppendMessage(message.message);
    }

    public void SetClientID(int id) {
        currentClientID = id;
    }
}
[Serializable] public struct GetAuth : NetworkMessage { }
[Serializable] public struct SendAuth : NetworkMessage { public int code; }
[Serializable] public struct AuthSucsessMessage : NetworkMessage { public string name; public string id; }

[Serializable] public struct ChatMessagetoServer : NetworkMessage { public string message; }
[Serializable] public struct ChatMessageToClients : NetworkMessage { public string message; }

