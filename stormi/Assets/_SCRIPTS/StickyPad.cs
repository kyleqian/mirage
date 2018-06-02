using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;

public class StickyPad : MonoBehaviour {

//	public enum StickyPadState { DELETE, ADD_STICKY, WRITE };
//	public StickyPadState state;
	public GameObject NewStickyPrefab;
	public GameObject Whiteboard;
	public StrokeMirror StrokeMirror;

	// source of truth for current text
	private string currentText;
	private Text textBox;

	// janky because we're mixing state with events
	private bool shouldAddSticky = false;

	private System.Random random;
	private Vector3 whiteboardSize;

	void Start () {
        // Get the canvas, then get the text object, then get the text component
        random = new System.Random();
		textBox = getTextboxFromSticky(this.gameObject);
		whiteboardSize = Whiteboard.GetComponent<Renderer> ().bounds.size;
	}
	
	// Update is called once per frame
	void Update () {
		textBox.text = currentText;

		if (shouldAddSticky) {
			addSticky ();
			shouldAddSticky = false;
		}
	}

	public void draw(string strokesString) {
		StrokeMirror.draw (strokesString);
	}

	public void addSpace() {
		currentText += " ";
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


	private void addSticky() {
		// Get the current pointing location to board
		//		RaycastHit hit;

		//		if (Physics.Raycast (transform.position, transform.forward, out hit, Mathf.Infinity)) {
		//			if (hit.transform.tag == "Board") {


		// Place the sticky randomly on the whiteboard
		int randomX = random.Next(-(int)(whiteboardSize.x / 4), (int)(whiteboardSize.x / 4));
		int randomY = random.Next(-(int)(whiteboardSize.y / 2), (int)(whiteboardSize.y / 2));

		// Get the width and height 
		Vector3 randPosition = new Vector3(Whiteboard.transform.position.x + randomX, 
			Whiteboard.transform.position.y + randomY, 
			Whiteboard.transform.position.z - 0.05f);

		// Instantiate new sticky prefab on the board with the text
		GameObject newSticky = Instantiate(NewStickyPrefab,
			randPosition,
			Quaternion.identity) as GameObject;

		// Transfer text to that sticky
		getTextboxFromSticky(newSticky).text = currentText;

		// Reset the current sticky pad text
		wipeStickyPad();
	}

	private int getBeforeLastWordIndex(string words) {
		return words.LastIndexOf (" ");
	}

	private void wipeStickyPad() {
		currentText = "";
	}

	private Text getTextboxFromSticky(GameObject sticky) {
		return sticky.transform.GetChild(0).GetChild(0).GetComponent<Text>();
	}
}
