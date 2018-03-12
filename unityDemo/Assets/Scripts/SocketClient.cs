using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SocketClient : MonoBehaviour {
//	public class TextReceivedEvent : UnityEvent<string> {
//	}

	public static SocketClient Instance;

	ClientWebSocket webSocket;
	const int receiveChunkSize = 256;

//	public TextReceivedEvent OnTextReceived = new TextReceivedEvent();

	public Text text;

	void Awake() {
		Instance = this;
	}

	void Start () {
		Connect ();
	}
	
	void Update () {
	}

	async Task Receive() {
		byte[] buffer = new byte[receiveChunkSize];

		while (webSocket.State == WebSocketState.Open) {
			var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

			if (result.MessageType == WebSocketMessageType.Close)
			{
				await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
			}
			else
			{
				text.text = System.Text.Encoding.UTF8.GetString (buffer);
//				OnTextReceived.Invoke (System.Text.Encoding.UTF8.GetString(buffer));
//				Debug.Log(System.Text.Encoding.UTF8.GetString(buffer));
			}
		}
	}

	async void Connect() {
		try
		{
			webSocket = new ClientWebSocket();
			await webSocket.ConnectAsync(new Uri("ws://mirage-relay-server.herokuapp.com/"), CancellationToken.None);
			// await Task.WhenAll(Receive(webSocket), Send(webSocket));

			await Receive();
		}
		catch (Exception ex)
		{
			Console.WriteLine("Exception: {0}", ex);
		}
		finally
		{
			//if (webSocket != null)
			//	webSocket.Dispose();

			// Console.WriteLine();
		}
	}
}
