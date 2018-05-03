using UnityEngine;

public class Controller : MonoBehaviour
{
    public enum ControllerState { Controller, Sticky };
    public ControllerState state;
//	public static const string UP_PRESS = "UP_PRESS";
//	public static const string DOWN_PRESS = "DOWN_PRESS";

    bool currentlySelecting;

	void Start()
    {
        state = ControllerState.Controller;
	}
	
	void Update()
    {
		RaycastHit hit;

		if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
		{
			if (currentlySelecting)
			{
				Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);
				// Move around sticky/item
			}
		}
	}
}
