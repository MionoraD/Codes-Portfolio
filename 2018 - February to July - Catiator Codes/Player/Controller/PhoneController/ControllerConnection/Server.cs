using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class Server : MonoBehaviour
{
    string networkIPAddres;
    bool isOpen = false;

    private static List<NetworkConnection> currentconnections = new List<NetworkConnection>();

    /*
    private void OnGUI()
    {
        networkIPAddres = LocalIPAddress();
        GUI.Box(new Rect(10, Screen.height - 50, 100, 50), networkIPAddres);
        GUI.Label(new Rect(20, Screen.height - 35, 100, 20), "Status: " + NetworkServer.active);
        GUI.Label(new Rect(20, Screen.height - 20, 100, 20), "Connected: " + NetworkServer.connections.Count);
    }
    */

    public static string LocalIPAddress()
    {
        IPHostEntry host;
        string localIP = "";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartConnection();
    }

    void StartConnection()
    {
        if(!isOpen)
        {
            NetworkServer.Listen(25000);
            NetworkServer.RegisterHandler(888, ServerReceiveMessage);

            isOpen = true;
        }
    }

    private void ServerReceiveMessage(NetworkMessage message)
    {
        StringMessage msg = new StringMessage();
        msg.value = message.ReadMessage<StringMessage>().value;

        string[] deltas = msg.value.Split('|');

        if(transform.childCount == 0)
        {
            msg.value = DisconnectMessage();
            NetworkServer.SendToClient(message.conn.connectionId, 889, msg);
        }

        foreach (Transform child in transform)
        {
            ClientServerConnection connection = child.GetComponent<ClientServerConnection>();
            if (connection != null)
            {
                string strNr = deltas[0];
                int nr = int.Parse(strNr);

                if (deltas[1].Equals("start"))
                {
                    ConnectionAvailable(connection, nr, message.conn, deltas);
                }
                else if (deltas[1].Equals("character"))
                {
                    SetCharacter(connection, nr, deltas[0], deltas[2], true);
                }
                else if (deltas[1].Equals("cancel"))
                {
                    SetCharacter(connection, nr, deltas[0], "", false);
                }
                else if (deltas[1].Equals("game"))
                {
                    connection.UseMove(nr, deltas);
                }
            }
        }
    }

    private void ConnectionAvailable(ClientServerConnection connection, int nr, NetworkConnection conn, string[] deltas)
    {
        StringMessage msg = new StringMessage();
        if (connection.Available(nr))
        {
            msg.value = ConnectionMessage(connection);
            NetworkServer.SendToClient(conn.connectionId, 889, msg);
            currentconnections.Add(conn);

            connection.OpenConnection();

            return;
        }

        msg.value = DisconnectMessage();
        NetworkServer.SendToClient(conn.connectionId, 889, msg);
    }

    private string ConnectionMessage(ClientServerConnection connection)
    {
        string message = "Connection";
        message += "|" + connection.ColorString();

        return message;
    }

    private string DisconnectMessage()
    {
        string message = "Disconnect";
        return message;
    }

    private void SetCharacter(ClientServerConnection connection, int nr, string player, string character, bool chosen)
    {
        if (chosen)
            connection.ConfirmCharacter(nr, character);
        else
            connection.CancelCharacter(nr);
    }

    static public void StartGame()
    {
        StringMessage msg = new StringMessage();
        msg.value = "Game";

        foreach(NetworkConnection conn in currentconnections)
        {
            NetworkServer.SendToClient(conn.connectionId, 889, msg);
        }
    }
}
