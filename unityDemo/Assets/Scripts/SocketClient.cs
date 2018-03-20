using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class SocketClient : MonoBehaviour {

	public static SocketClient Instance;
	private ClientWebSocket webSocket;

	private const int receiveChunkSize = 256;

	// End indication variables
	private const string SEND_TRIGGER = "END";
	private const string SPACE_BAR = "SPACE";
    private string totalText = "";
	private bool shouldSendText;

	///////////// Starter Methods /////////////
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
			WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

			if (result.MessageType == WebSocketMessageType.Close)
			{
				await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
			}
			else
			{
				byte[] receivedBytes = new byte[result.Count];
				Array.Copy(buffer, 0, receivedBytes, 0, result.Count);

				string receivedText = System.Text.Encoding.UTF8.GetString (receivedBytes);
				if (receivedText.Contains(SEND_TRIGGER)) {
					// Inform subscriber that text is ready to process
					shouldSendText = true;
				} else if (receivedText.Contains(SPACE_BAR)) {
                    print("Received SPACE_BAR");
                    totalText += " ";
                } else {
					print ("Received text " + receivedText);
					totalText += receivedText;
				}

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

	///////////////////// Getter and Setter Methods ///////////////////
	public string getTotalText() {
		return totalText;
	}

	private void clearTotalText() {
		totalText = "";
	}

	public bool getShouldSendText() {
		return shouldSendText;
	}

	private void offShouldSendText() {
		shouldSendText = false;
	}

	public void returnToEmptyState() {
		clearTotalText ();
		offShouldSendText ();
	}
}
