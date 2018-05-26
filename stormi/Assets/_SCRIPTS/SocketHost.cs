using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

[Serializable]
public class JSONData {
	public string type;
	public string text;
	public string strokes;
}

public class M_Rotation : WebSocketBehavior
{
    protected override void OnMessage(MessageEventArgs e)
    {
        string[] rotationData = e.Data.Split(';');
        SocketHost.instance.pitch = float.Parse(rotationData[0]);
        SocketHost.instance.yaw = float.Parse(rotationData[1]);
        SocketHost.instance.roll = float.Parse(rotationData[2]);
    }
}

public class M_Input : WebSocketBehavior
{
    protected override void OnMessage(MessageEventArgs e)
    {
        SocketHost.instance.curText = e.Data;
		SocketHost.InvokeReceivedMessage(JsonUtility.FromJson<JSONData>(e.Data));
    }
}

public class SocketHost : MonoBehaviour
{
    public static SocketHost instance;
    public float pitch;
    public float yaw;
    public float roll;
    public string curText;

	public delegate void OnReceivedMessage(JSONData data);
    public static event OnReceivedMessage ReceivedMessage;

    WebSocketServer wssv;
    UdpClient udpClient;
    bool broadcasting;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        wssv = new WebSocketServer(9001);

        wssv.AddWebSocketService<M_Rotation>("/M_Rotation", () => new M_Rotation()
        {
            IgnoreExtensions = true
        });

        wssv.AddWebSocketService<M_Input>("/M_Input", () => new M_Input()
        {
            IgnoreExtensions = true
        });

        // Listen for connections
        wssv.Start();

        // Turn on/off broadcasting
        InvokeRepeating("BroadcastWatchman", 0, 0.5f);
    }

    void BroadcastWatchman()
    {
        int totalConnections = 0;
        foreach (var h in wssv.WebSocketServices.Hosts)
        {
            totalConnections += h.Sessions.Count;
        }
        
        print("Total websocket connections: " + totalConnections);

        if (totalConnections == wssv.WebSocketServices.Count)
        {
            if (broadcasting)
            {
                StopBroadcastIp();
            }
        }
        else
        {
            if (!broadcasting)
            {
                StartBroadcastIp();
            }
        }
    }

    void StartBroadcastIp()
    {
        broadcasting = true;
        udpClient = new UdpClient(9002, AddressFamily.InterNetwork);
		udpClient.Connect(new IPEndPoint(IPAddress.Broadcast, 9003));
		InvokeRepeating("BroadcastIp", 0, 0.2f);
    }

    void StopBroadcastIp()
    {
        broadcasting = false;
        udpClient.Close();
        CancelInvoke("BroadcastIp");
    }

    void BroadcastIp()
    {
		string ipAddress = Network.player.ipAddress.ToString();

		if (ipAddress != "")
        {
			byte[] b = Encoding.ASCII.GetBytes(ipAddress);
			udpClient.Send(b, b.Length);
		}
    }

	public static void InvokeReceivedMessage(JSONData data)
    {
        if (ReceivedMessage!= null)
        {
			ReceivedMessage(data);
        }
    }
}
