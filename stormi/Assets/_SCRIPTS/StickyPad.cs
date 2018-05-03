using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StickyPad : MonoBehaviour {

//	public enum StickyPadState { DELETE, ADD_STICKY, WRITE };
//	public StickyPadState state;
	Text textBox;
	private string currentText;

//	void OnEnable() {
//
//	}
//
//	void OnDisable() {
//
//	}

	void Start () {
		// Get the canvas, then get the text object, then get the text component
		textBox = transform.GetChild(0).GetChild(0).GetComponent<Text>();
//		print (textBox);
	}
	
	// Update is called once per frame
	void Update () {
		textBox.text = currentText;
//		switch (state) {
//		case StickyPadState.DELETE:
//			
//			break;
//		case StickyPadState.ADD_STICKY:
//			break;
//		case StickyPadState.WRITE:
//			break;
//		default:
//			break;
//		}
//		textBox.text = SocketHost.instance.curText;
	}

	public void addSpace() {
		currentText += " ";
	}

	public void addSticky() {
		// Filler
	}

	public void deleteLastWord() {
		currentText = textBox.text.Substring(0, getBeforeLastWordIndex(textBox.text.TrimEnd()));
	}

	public void write(string lastText) {
		currentText += lastText;
	}

	private int getBeforeLastWordIndex(string words) {
		int beforeLastWordIndex = words.LastIndexOf (" ");
		beforeLastWordIndex = beforeLastWordIndex >= 0 ? beforeLastWordIndex : 0;
		print (beforeLastWordIndex);
		return beforeLastWordIndex;
	}
}
