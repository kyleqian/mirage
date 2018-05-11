using UnityEngine;

public class Controller : MonoBehaviour
{
    //public enum ControllerState { Neutral, MoveSticky, MoveBoard };

    public bool inputDown;
    public bool inputUp;

    //ControllerState state;
    GameObject objectBeingMoved;
    LineRenderer lineRenderer;

	void OnEnable()
	{
        //state = ControllerState.Neutral;
	}

	void Start()
	{
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
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
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, hit.point + (hit.point - transform.position) * 100);
            //Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);

            // Highlight stuff

            // AHHHHHHHHH
            if (objectBeingMoved)
            {
                if (inputUp)
                {
                    objectBeingMoved.GetComponent<StickyNote>().DroppedOff();

                    // Drop object HACK HACK
                    if (hit.point.x < -15 || hit.point.x > 15 ||
                       hit.point.y < -5 || hit.point.y > 5)
                    {
						Destroy(objectBeingMoved);
                    }

					objectBeingMoved = null;

                    return;
                }

                objectBeingMoved.GetComponent<StickyNote>().MoveToRaycast(hit.point);
                return;
            }

            if (inputDown)
            {
                // Select thing based on what it is, and ATTACH
                if (hit.transform.tag == "Sticky")
                {
                    //state = ControllerState.MoveSticky;
                    objectBeingMoved = hit.transform.gameObject;
                    objectBeingMoved.GetComponent<StickyNote>().PickedUpBy(transform.parent);
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

                if (objectBeingMoved)
                {
					objectBeingMoved.GetComponent<StickyNote>().DroppedOff();
					objectBeingMoved = null;
                }
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }
}
