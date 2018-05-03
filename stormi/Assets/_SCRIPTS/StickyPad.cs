using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StickyPad : MonoBehaviour {

//	public enum StickyPadState { DELETE, ADD_STICKY, WRITE };
//	public StickyPadState state;
	public GameObject NewStickyPrefab;
	Text textBox;
	private string currentText;

	// janky because we're mixing state with events
	bool shouldAddSticky = false;

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

		if (shouldAddSticky) {
			addSticky ();
			shouldAddSticky = false;
		}

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

	private void addSticky() {
		print("Adding sticky...");
		// Get the current pointing location to board
		RaycastHit hit;

		if (Physics.Raycast (transform.position, transform.forward, out hit, Mathf.Infinity)) {
			if (hit.transform.tag == "Board") {
				// Instantiate new sticky prefab on the board with the text
				GameObject newStick = Instantiate(NewStickyPrefab,
					hit.point,
					Quaternion.identity) as GameObject;

				// Reset the current sticky pad text
			}
		}




	}

	public void toggleAddSticky() {
		shouldAddSticky = true;
	}

	public void deleteLastWord() {
		currentText = textBox.text.Substring(0, getBeforeLastWordIndex(textBox.text.TrimEnd()) + 1);
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
