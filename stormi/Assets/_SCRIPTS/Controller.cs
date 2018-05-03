using UnityEngine;

public class Controller : MonoBehaviour
{
    //bool currFrameSelecting;
    //bool currFrameUnselecting;
    //bool currentlySelecting;
    //public bool CurrentlySelecting {
    //    get {
    //        return currentlySelecting;
    //    }
    //    set
    //    {
    //        if (currentlySelecting != value)
    //        {
    //            currFrameSelecting = value;
    //            currFrameUnselecting = !value;
    //        }
    //        currentlySelecting = value;
    //    }
    //}

    // Hacky
    bool inputDown;
    public bool InputDown
    {
        get { return inputDown; }
        set
        {
            inputUp = false;
            inputDown = value;
        }
    }

    // Hacky
    bool inputUp;
    public bool InputUp
    {
        get { return inputUp; }
        set
        {
            inputDown = false;
            inputUp = value;
        }
    }

	void OnEnable()
	{
        //currentlySelecting = false;
	}

	void Update()
    {
		RaycastHit hit;

		if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
		{
			//if (currentlySelecting)
			//{
			//	Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);
			//	// Move around sticky/item
			//}
		}
	}
}
