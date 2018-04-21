using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

public class Rotation : WebSocketBehavior
{
	protected override void OnMessage(MessageEventArgs e)
	{
        //Debug.Log(e.Data);
        string[] rotationData = e.Data.Split(';');
        SocketHost.instance.pitch = float.Parse(rotationData[0]);
        SocketHost.instance.yaw = float.Parse(rotationData[1]);
        SocketHost.instance.roll = float.Parse(rotationData[2]);
	}
}

public class SocketHost : MonoBehaviour
{
    public static SocketHost instance;
    public float pitch;
    public float yaw;
    public float roll;
    WebSocketServer wssv;

	void Awake()
	{
        instance = this;
	}

	void Start()
    {
        wssv = new WebSocketServer(9001);
        wssv.AddWebSocketService<Rotation>("/Rotation");
        wssv.Start();
    }
}
