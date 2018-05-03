using UnityEngine;

public class Controller : MonoBehaviour
{
    public enum ControllerState { Neutral, MoveSticky, MoveBoard };

    public bool inputDown;
    public bool inputUp;

    ControllerState state;

	void OnEnable()
	{
        state = ControllerState.Neutral;
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
                
            }
            else if (inputUp)
            {
                
            }


            switch (state)
            {
                case ControllerState.MoveBoard:
                    break;
                case ControllerState.MoveSticky:
                    break;
                default:
                    break;
            }



            //if (currentlySelecting)
            //{
            //  Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);
            //  // Move around sticky/item
            //}
        }
    }
}
