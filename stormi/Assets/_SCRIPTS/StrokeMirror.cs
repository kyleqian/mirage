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
	private float widthRatio;
	private float heightRatio;

	public Transform stroke;

	// Use this for initialization
	void Start () {
		strokeMirrorSize = GetComponent<Renderer> ().bounds.size;


		widthRatio = strokeMirrorSize.x / PHONE_WIDTH;
		heightRatio = strokeMirrorSize.y / PHONE_HEIGHT;
	}
	
	// Update is called once per frame
	void Update () {
		if (shouldDraw) {
			// For each stroke, draw a line through all its points
			//			foreach (List<List<float>> stroke in strokes) {
			//				// Two lists here. First is x points, next is y points
			//
			//			}


			Transform newStroke = Instantiate(stroke);
			newStroke.parent = gameObject.transform;
			newStroke.localPosition = Vector3.zero;
			newStroke.localRotation = Quaternion.identity;
			newStroke.localScale = Vector3.one;

			// Adjust to phone dimensions
			newStroke.gameObject.GetComponent<LineRenderer>().useWorldSpace = false;
			newStroke.gameObject.GetComponent<LineRenderer>().SetPosition(0, new Vector3(-strokeMirrorSize.x, -strokeMirrorSize.y, 0));
			newStroke.gameObject.GetComponent<LineRenderer>().SetPosition(1, new Vector3(strokeMirrorSize.x, strokeMirrorSize.y, 0));

			shouldDraw = false;
		}
	}

	public void draw(string strokesString) {
		strokes = deserializeStrokesString (strokesString);
		shouldDraw = true;
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
