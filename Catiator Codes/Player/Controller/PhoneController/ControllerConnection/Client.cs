using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public class Client : MonoBehaviour
{
    string name;
    static int player = 0;
    Color clr;

    string character;

    string localIPAddres;
    string networkIPAddres;

    static NetworkClient client;
    bool hasConnected = false;

    [Header("Screens")]
    [SerializeField] private GameObject login;
    [SerializeField] private GameObject loading, selection, waiting, controller;
    [SerializeField] private Text loadingMessage, waitingMessage, characterName;

    private void OnGUI()
    {
        localIPAddres = Server.LocalIPAddress();
        GUI.Box(new Rect(10, Screen.height - 50, 100, 50), localIPAddres);
        GUI.Label(new Rect(20, Screen.height - 35, 100, 20), "Status: " + client.isConnected);

        if (!client.isConnected)
        {
            if (GUI.Button(new Rect(10, 10, 60, 50), "Connect"))
            {
                Connect();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        client = new NetworkClient();
    }

    public void SetName(string _name)
    {
        name = _name;
    }
    public void SetIPAddres(string _ipaddress)
    {
        networkIPAddres = _ipaddress;
    }
    public void SetPlayer(int index)
    {
        player = index;
    }

    public void Connect()
    {
        Debug.Log("Connect: " + player + " " + name + " on " + networkIPAddres);
        Loading("Connecting");

        if (!client.isConnected)
        {
            client.Connect(networkIPAddres, 25000);
            client.RegisterHandler(889, ReceiveMessage);
        }

        StartCoroutine(StartConnection());
    }

    void Loading(string msg)
    {
        login.SetActive(false);
        selection.SetActive(false);
        waiting.SetActive(false);
        controller.SetActive(false);

        loadingMessage.text = msg;
        loading.SetActive(true);
    }

    private IEnumerator StartConnection()
    {
        yield return new WaitForSeconds(1f);

        string message = "start";
        message += "|" + name;
        SendInfo(message);

        hasConnected = true;
    }

    static public void SendInfo(string data)
    {
        if (client.isConnected)
        {
            string message = "" + player;
            message += "|" + data;

            StringMessage msg = new StringMessage();
            msg.value = message;
            client.Send(888, msg);
        }
    }

    void Update()
    {
        // If client has disconnected from game
        if (hasConnected && !client.isConnected)
        {
            Loading("Disconnecting");
            Disconnect();
        }
    }

    void Disconnect()
    {
        login.SetActive(true);
        loading.SetActive(false);

        hasConnected = false;
    }

    public void ReceiveMessage(NetworkMessage message)
    {
        StringMessage msg = new StringMessage();
        msg.value = message.ReadMessage<StringMessage>().value;

        string[] deltas = msg.value.Split('|');

        if (deltas[0].Equals("Connection"))
        {
            float r = float.Parse(deltas[1]) / 1000;
            float g = float.Parse(deltas[2]) / 1000;
            float b = float.Parse(deltas[3]) / 1000;

            clr = new Color(r, g, b);

            selection.SetActive(true);
            loading.SetActive(false);
        }
        else if (deltas[0].Equals("Game"))
        {
            StartGame();
        }
        else if (deltas[0].Equals("Disconnect"))
        {
            Disconnect();
        }
    }

    public void ChooseCharacter(string _character)
    {
        character = _character;
        characterName.text = character;

        string message = "character";
        message += "|" + character;
        SendInfo(message);

        Loading("Chosen Character: " + character);

        waitingMessage.text = "Chosen Character: " + character;
        waiting.SetActive(true);
        loading.SetActive(false);
    }

    public void CancelCharacter()
    {
        string message = "cancel";
        SendInfo(message);

        Loading("Cancel chosen character");

        selection.SetActive(true);
        loading.SetActive(false);
    }

    public void StartGame()
    {
        Loading("Game is loading");

        loading.SetActive(false);
        controller.SetActive(true);
    }

    static public void UseButton(string button, string data)
    {
        string message = "game";
        message += "|" + button;
        message += "|" + data;
        SendInfo(message);
    }
}
