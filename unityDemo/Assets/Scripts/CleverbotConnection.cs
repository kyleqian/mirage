using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CleverbotConnection : MonoBehaviour {

	//////////// Member variables ////////////
	public Text userText;
	private Text replyText;
	[System.Serializable]
	private class CleverbotReply
	{
		public string output;
	}
	private string currentUserText = "";

		
	///////////// Standard Methods //////////////
	void Start() {
		replyText = GetComponent<Text> ();
	}

	// Update is called once per frame
	void Update () {
//		bool hasTextUpdated = currentUserText != SocketClient.Instance.getTotalText ();
//		if (hasTextUpdated) {
		// Set current user text to new text
		currentUserText = SocketClient.Instance.getTotalText ();
		if (SocketClient.Instance.getShouldSendText()) {
			// Request a reply from Cleverbot
			StartCoroutine(GetReply(currentUserText));
		}
	}

	///////////// Helper Methods //////////////
	IEnumerator GetReply(string input)
	{
		UnityWebRequest request = UnityWebRequest.Get ("https://www.cleverbot.com/getreply?key=CC80ebgCf3oLzog7UyH8xwLslXQ&input="+input);
		yield return request.SendWebRequest ();

		if(request.isNetworkError || request.isHttpError) {
			print(request.error);
		}
		else {
			//deserialize, extract output, set text equal to output
			replyText.text = getCleverbotReply(request.downloadHandler.text);
		}

		// Reset cleverbot
		SocketClient.Instance.returnToEmptyState ();
	}

	private string getCleverbotReply(string fullJson)
	{
		string newJson = "{ \"array\": " + fullJson + "}";
		CleverbotReply cbReply = JsonUtility.FromJson<CleverbotReply> (fullJson);
		return cbReply.output;
	}
}
