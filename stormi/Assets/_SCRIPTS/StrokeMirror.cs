using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrokeMirror : MonoBehaviour {

	private bool shouldDraw = false;
	private List<List<List<float>>> strokes = new List<List<List<float>>>();

	private Vector3 strokeMirrorSize;
	private const float PHONE_WIDTH = 600f;
	private const float PHONE_HEIGHT = 400f;
	private Vector3 STROKE_DEFAULT = new Vector3(-0.5f, 0.5f, -0.6f);

	public Transform strokeObject;

	// Use this for initialization
	void Start () {
		strokeMirrorSize = GetComponent<Renderer> ().bounds.size;

//
//		widthRatio = strokeMirrorSize.x / PHONE_WIDTH;
//		heightRatio = strokeMirrorSize.y / PHONE_HEIGHT;
	}
	
	// Update is called once per frame
	void Update () {
		if (shouldDraw) {
			Debug.Log ("Staring a new draw with " + strokes.Count + " strokes");
			eraseCurrentStrokes ();

			// For each stroke, draw a line through all its points
			foreach (List<List<float>> stroke in strokes) {
				instantiateNewStroke(stroke);
			}

			shouldDraw = false;
		}
	}

	public void draw(string strokesString) {
		strokes = deserializeStrokesString (strokesString);
		shouldDraw = true;
	}

	private void eraseCurrentStrokes() {
		foreach (Transform child in gameObject.transform) {
			Destroy (child.gameObject);
		}
	}

	private void instantiateNewStroke(List<List<float>> stroke) {
		Transform newStroke = Instantiate(strokeObject);
		newStroke.parent = gameObject.transform;
		newStroke.localPosition = STROKE_DEFAULT;
		newStroke.localRotation = Quaternion.identity;
		newStroke.localScale = Vector3.one;

		newStroke.gameObject.GetComponent<LineRenderer> ().positionCount = stroke [0].Count;

		// Two lists here. First is x points, next is y points
		for (int i = 0; i < stroke [0].Count; i++) {
			float xCood = stroke [0] [i];
			float yCood = stroke [1] [i];

			// Adjust to phone dimensions
			newStroke.gameObject.GetComponent<LineRenderer>().SetPosition(i, new Vector3(xCood/PHONE_WIDTH, -yCood/PHONE_HEIGHT, 0f));
		}
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

		string[] strokesStringSplit = strokesString.Split (
			new [] { Environment.NewLine },
			StringSplitOptions.None
		);

		// Init an array
		return deserializeHelper (strokesStringSplit);
	}
}
