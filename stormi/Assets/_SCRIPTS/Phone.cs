using UnityEngine;

public class Phone : MonoBehaviour
{
    public enum ControllerState { Controller, Sticky };
    public ControllerState state;
	public StickyPad stickyPad;
	public Controller controller;
	private const string UP_PRESS = "UP_PRESS";
	private const string DOWN_PRESS = "DOWN_PRESS";

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
        state = ControllerState.Controller;
	}
	
	void Update()
    {
        UpdatePose();

        if (state == ControllerState.Controller)
        {
//            UpdateControllerController();
        }
        else if (state == ControllerState.Sticky)
        {
//            UpdateControllerSticky();
        }
	}

    void OnReceivedMessage(string message)
    {
		if (state == ControllerState.Controller) {
			switch (message)
			{
			// TEMP
			case UP_PRESS:
				
			case DOWN_PRESS:

			default:
				// Received text
				break;
			}
		} else {
			switch (message)
			{
			// TEMP
			case UP_PRESS:

			case DOWN_PRESS:

			default:
				// Received text
				break;
			}
		}
        
    }

    void UpdatePose()
    {
        transform.localEulerAngles = new Vector3(-Mathf.Rad2Deg * SocketHost.instance.pitch, -Mathf.Rad2Deg * SocketHost.instance.yaw, -Mathf.Rad2Deg * SocketHost.instance.roll);
    }
}
