using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StickyPad : MonoBehaviour {

	Text textBox;
	public bool currentlySelecting;

	void Start () {
		// Get the canvas, then get the text object, then get the text component
//		textBox = transform.GetChild(0).GetChild(0).GetComponent<Text>();
//		print (textBox);
	}
	
	// Update is called once per frame
	void Update () {
//		textBox.text = SocketHost.instance.curText;
	}
}
