using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;


public class DisplayText : MonoBehaviour {

	private Text userTextField;
	// Use this for initialization
	void Start () {
		SocketClient.Instance.OnTextReceived.AddListener (writeText);

		userTextField = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void writeText(string inputString) {
		userTextField.text += inputString;
	}
}
