using UnityEngine;

public class Phone : MonoBehaviour
{
    public enum PhoneState { Controller, Sticky };

    public PhoneState state;
	public StickyPad stickyPad;
	public Controller controller;

	const string UP_PRESS = "UP_PRESS";
	const string DOWN_PRESS = "DOWN_PRESS";
    const string ORIENTATION_LANDSCAPE = "ORIENTATION_LANDSCAPE";
    const string ORIENTATION_PORTRAIT = "ORIENTATION_PORTRAIT";

	void OnEnable()
	{
		SocketHost.ReceivedMessage += OnReceivedMessage;
    }

	void OnDisable()
	{
        SocketHost.ReceivedMessage -= OnReceivedMessage;
	}

	void Start()
    {
		state = PhoneState.Sticky;
    }
    
    void Update()
    {
        UpdateVisibility();
        UpdatePose();
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
    			// TEMP
    			case UP_PRESS:
                    controller.inputDown = true;
                    break;
    			case DOWN_PRESS:
                    controller.inputUp = true;
                    break;
    			default:
    				// Received text
    				break;
			}
        }
        else if (state == PhoneState.Sticky)
        {
			switch (message)
			{
    			// TEMP
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
}
