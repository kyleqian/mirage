using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StickyPad : MonoBehaviour {

	private enum StickyPadState { DELETE, ADD_STICKY, WRITE };
	public StickyPadState state;
	Text textBox;

//	void OnEnable() {
//
//	}
//
//	void OnDisable() {
//
//	}

	void Start () {
		// Get the canvas, then get the text object, then get the text component
//		textBox = transform.GetChild(0).GetChild(0).GetComponent<Text>();
//		print (textBox);
	}
	
	// Update is called once per frame
	void Update () {
		switch (state) {
		case StickyPadState.DELETE:
			textBox.text = textBox.text.Substring(getBeforeLastWordIndex(textBox.text.TrimEnd()));
			break;
		case StickyPadState.ADD_STICKY:
			break;
		case StickyPadState.WRITE:
			break;
		default:
			break;
		}
//		textBox.text = SocketHost.instance.curText;
	}

	int getBeforeLastWordIndex(string words) {
		int beforeLastWordIndex = words.LastIndexOf (" ");
		beforeLastWordIndex = beforeLastWordIndex >= 0 ? beforeLastWordIndex : 0;
	}
}
