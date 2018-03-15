using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;


public class DisplayText : MonoBehaviour {

	private Text userTextField;

	// Use this for initialization
	void Start () {
		userTextField = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		userTextField.text = SocketClient.Instance.getTotalText ();	
	}
}
