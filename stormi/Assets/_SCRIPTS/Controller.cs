using UnityEngine;

public class Controller : MonoBehaviour
{
    //public enum ControllerState { Neutral, MoveSticky, MoveBoard };

    public bool inputDown;
    public bool inputUp;

    //ControllerState state;
    GameObject objectBeingMoved;

	void OnEnable()
	{
        //state = ControllerState.Neutral;
	}

	void Update()
    {
        PerformRaycast();
        inputDown = false;
        inputUp = false;
	}

    void PerformRaycast()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);

            // Highlight stuff

            if (inputDown)
            {
                // Select thing based on what it is, and ATTACH
                if (hit.transform.tag == "Sticky")
                {
                    //state = ControllerState.MoveSticky;
                    objectBeingMoved = hit.transform.gameObject;
                    objectBeingMoved.GetComponent<TestSticky>().PickedUpBy(transform.parent);
                }
                else if (hit.transform.tag == "Board")
                {
                    //state = ControllerState.MoveBoard;
                }
            }
            else if (inputUp)
            {
                // Let go of thing, and do anything else
                //state = ControllerState.Neutral;

                objectBeingMoved.GetComponent<TestSticky>().DroppedOff();
                objectBeingMoved = null;
            }

            //switch (state)
            //{
            //    case ControllerState.MoveBoard:
            //        // Keep in some axis
            //        break;
            //    case ControllerState.MoveSticky:
            //        // Keep in some axis / on the board / throw away
            //        break;
            //}
        }
    }
}
