using System;
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

        wssv.Start();
    }

	public static void InvokeReceivedMessage(JSONData data)
    {
        if (ReceivedMessage!= null)
        {
			ReceivedMessage(data);
        }
    }
}
