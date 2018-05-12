using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Phone : NetworkBehaviour
{
    public enum PhoneState { Controller, Sticky };

    public PhoneState state;
	public StickyPad stickyPad;
	public Controller controller;
    public GameObject NewStickyPrefab;

	const string UP_PRESS = "UP_PRESS";
	const string SPACE = "SEND_SPACE";
	const string MULTISWIPE = "SEND_MULTI_SWIPE";
	const string DOWN_PRESS = "DOWN_PRESS";
    const string ORIENTATION_LANDSCAPE = "ORIENTATION_LANDSCAPE";
    const string ORIENTATION_PORTRAIT = "ORIENTATION_PORTRAIT";
    const string PORTRAIT_TOUCH_BEGAN = "PORTRAIT_TOUCH_BEGAN";
    const string PORTRAIT_TOUCH_ENDED = "PORTRAIT_TOUCH_ENDED";

	// void OnEnable()
	// {
    //     SocketHost.ReceivedMessage += OnReceivedMessage;
    // }

	// void OnDisable()
	// {
    //     SocketHost.ReceivedMessage -= OnReceivedMessage;
	// }

	void Start()
    {
		state = PhoneState.Controller;

        if (isLocalPlayer)
        {
            // DOES NOT HAVE CORRESPONDING REMOVE LISTENER RIGHT NOW
            SocketHost.ReceivedMessage += OnReceivedMessage;
        }
    }
    
    void Update()
    {
        UpdateVisibility();

        if (isLocalPlayer)
        {
            UpdatePose();
        }
	}

    void OnReceivedMessage(string message)
    {
        if (message == ORIENTATION_PORTRAIT)
        {
            state = PhoneState.Controller;
            return;
        }
        else if (message == ORIENTATION_LANDSCAPE)
        {
            state = PhoneState.Sticky;
            return;
        }

		if (state == PhoneState.Controller)
        {
			switch (message)
			{
                case PORTRAIT_TOUCH_BEGAN:
                    controller.inputDown = true;
                    break;
                case PORTRAIT_TOUCH_ENDED:
                    controller.inputUp = true;
                    break;
			}
        }
        else if (state == PhoneState.Sticky)
        {
			switch (message)
			{
			case SPACE:
				stickyPad.addSpace ();
				break;
			case MULTISWIPE:
				stickyPad.toggleAddSticky ();
				break;
            case UP_PRESS:
                // Nothing on Unity side for now
                break;
			case DOWN_PRESS:
				stickyPad.deleteLastWord ();
				break;
			default:
				stickyPad.write (message);
				// Received text
				break;
			}
		}
    }

    void UpdatePose()
    {
        transform.localEulerAngles = new Vector3(-Mathf.Rad2Deg * SocketHost.instance.pitch, -Mathf.Rad2Deg * SocketHost.instance.yaw, -Mathf.Rad2Deg * SocketHost.instance.roll);
    }

    void UpdateVisibility()
    {
        controller.gameObject.SetActive(state == PhoneState.Controller);
        stickyPad.gameObject.SetActive(state == PhoneState.Sticky);
    }

    public void SpawnSticky(Vector3 position, string text)
    {
        if (isLocalPlayer)
        {
            CmdSpawnSticky(position, text);
        }
    }

    public void DestroySticky(GameObject sticky)
    {
        if (isLocalPlayer)
        {
            CmdDestroySticky(sticky);
        }
    }

    [Command]
    void CmdSpawnSticky(Vector3 position, string text)
    {
        GameObject newSticky = Instantiate(NewStickyPrefab,
			position,
			Quaternion.identity) as GameObject;

		// Transfer text to that sticky
		newSticky.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = text;
        
        NetworkServer.SpawnWithClientAuthority(newSticky, connectionToClient);
    }

    [Command]
    void CmdDestroySticky(GameObject sticky)
    {
        NetworkServer.Destroy(sticky);
    }
}
