using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;

public class StickyPad : MonoBehaviour {

//	public enum StickyPadState { DELETE, ADD_STICKY, WRITE };
//	public StickyPadState state;
	public GameObject NewStickyPrefab;
	public GameObject whiteboard;
	public Transform stroke;

	// source of truth for current text
	private string currentText;
	private Text textBox;
	// janky because we're mixing state with events
	private bool shouldAddSticky = false;
	private bool shouldDraw = false;
	private List<List<List<float>>> strokesArr = new List<List<List<float>>>();

	private System.Random random;
	private Vector3 whiteboardSize;
	private const float PHONE_WIDTH = 600f;
	private const float PHONE_HEIGHT = 400f;
	private const string TEST_STROKES = "[\n" +
		"[\n" +
		"[\n" +
		"2\n" +
		"4\n" +
		"],\n" +
		"[\n" +
		"3,\n" +
		"5,\n" +
		"]\n" +
		"]\n" +
		"]\n";

	void Start () {
        // Get the canvas, then get the text object, then get the text component
        random = new System.Random();
		textBox = getTextboxFromSticky(this.gameObject);
		whiteboardSize = whiteboard.GetComponent<Renderer> ().bounds.size;
	}
	
	// Update is called once per frame
	void Update () {
		textBox.text = currentText;

		if (shouldAddSticky) {
			addSticky ();
			shouldAddSticky = false;
		}

		if (shouldDraw) {
			// For each stroke, draw a line through all its points
			Instantiate(stroke, new Vector3(0, 0, 0), Quaternion.identity);
			shouldDraw = false;
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

	public void draw(string strokesString) {
		strokesArr = deserializeStrokesString (strokesString);
		shouldDraw = true;
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

	private List<List<List<float>>> deserializeHelper(string[] strokesStrings) {
		int level = 0;
		List<List<List<float>>> strokes = new List<List<List<float>>> ();
		List<List<float>> curStroke = new List<List<float>> ();

		for (int i = 0; i < strokesStrings.Length - 1; i++) {
			string curElem = strokesStrings [i].Trim ().TrimEnd (new char[] { ',' });

			if (level == 0 && curElem == "[") {
				level += 1;
			} else if (level == 1) {
				if (curElem == "[") {
					level += 1;
				} else if (curElem == "]") {
					
					level -= 1;
				}
			} else if (level == 2) {
				if (curElem == "[") {
					level += 1;
				} else if (curElem == "]") {
					// Reached the end of the current stroke
					strokes.Add (curStroke);
					curStroke = new List<List<float>> ();

					level -= 1;
				}
			}
			else if (level == 3) {

				List<float> curList = new List<float> ();


				while (curElem != "]") {
					curList.Add (float.Parse(curElem));

					i += 1;
					curElem = strokesStrings [i].Trim ().TrimEnd (new char[] { ',' });
				}

				curStroke.Add (curList);
				level -= 1;
			}
		}

		return strokes;

	}

	private List<List<List<float>>> deserializeStrokesString(string strokesString) {
		strokesString = strokesString.Trim ().TrimEnd (new char[] { ',' });
		Debug.Log (strokesString);
		string[] strokesStringSplit = strokesString.Split (
						                              new [] { Environment.NewLine },
						                              StringSplitOptions.None
					                              );

		// Init an array
		return deserializeHelper (strokesStringSplit);
	}

//	private List<List<float[]>> strokeStringToArr(string strokesString) {
//		string[] strokesStringSplit = strokesString.Split (
//			                              new [] { Environment.NewLine },
//			                              StringSplitOptions.None
//		                              );
//
//		// Init an array
//		List<List<float[]>> strokePoints = new List<List<float[]>> ();
//
//		int bracketCount = 0;
//		float [] curPoint = new float[2];
//		char[] charsToTrim = { ',' };
//		// Ignore the first and last elements b/c they are just the opening and closing brackets
//		for (int i = 1; i < strokesStringSplit.Length - 1; i++) {
//			string strokeElem = strokesStringSplit [i].Trim ().TrimEnd(charsToTrim);
//
//			if (strokeElem == "[") {
//				bracketCount++;
//			} else if (strokeElem == "]") {
//				bracketCount--;
//			}
//
//			if (bracketCount == 2 && strokeElem == "[") {
//				// Next is a list of x points and a list of y points
//				string curValue = strokesStringSplit[i].Trim().TrimEnd(charsToTrim); 
//
//				// First get the list of x points
//				while (curValue != "]") {
//					br
//				}
//
//			} 
//
////			else if (bracketCount == 2 && strokeElem == "]") {
////				// Two lists are over
////			} else if (bracketCount == 
////
////			curPoint [0] = float.Parse(strokesStringSplit[i + 1].TrimEnd(charsToTrim));
////			curPoint [1] = float.Parse(strokesStringSplit[i + 2]);
////
////			strokePoints.Add ((float [])curPoint.Clone());
////		}
//
//		return strokePoints;
//	}

	private void addSticky() {
		// Get the current pointing location to board
		//		RaycastHit hit;

		//		if (Physics.Raycast (transform.position, transform.forward, out hit, Mathf.Infinity)) {
		//			if (hit.transform.tag == "Board") {


		// Place the sticky randomly on the whiteboard
		int randomX = random.Next(-(int)(whiteboardSize.x / 4), (int)(whiteboardSize.x / 4));
		int randomY = random.Next(-(int)(whiteboardSize.y / 2), (int)(whiteboardSize.y / 2));

		// Get the width and height 
		Vector3 randPosition = new Vector3(whiteboard.transform.position.x + randomX, 
			whiteboard.transform.position.y + randomY, 
			whiteboard.transform.position.z - 0.05f);

		// Instantiate new sticky prefab on the board with the text
		GameObject newSticky = Instantiate(NewStickyPrefab,
			randPosition,
			Quaternion.identity) as GameObject;

		// Transfer text to that sticky
		getTextboxFromSticky(newSticky).text = currentText;

		// Reset the current sticky pad text
		wipeStickyPad();
		//			}
		//		}
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
