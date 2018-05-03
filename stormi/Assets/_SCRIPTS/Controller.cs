using UnityEngine;

public class Controller : MonoBehaviour
{
    public enum ControllerState { Controller, Sticky };
    public ControllerState state;

    bool controllerCurrentlySelecting;

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
            UpdateControllerController();
        }
        else if (state == ControllerState.Sticky)
        {
            UpdateControllerSticky();
        }
	}

    void OnReceivedMessage(string message)
    {
        switch (message)
        {
            // TEMP
            case "UP_PRESS":
                controllerCurrentlySelecting = true;
                break;
            case "DOWN_PRESS":
                controllerCurrentlySelecting = false;
                break;
            default:
                break;
        }
    }

    void UpdatePose()
    {
        transform.localEulerAngles = new Vector3(-Mathf.Rad2Deg * SocketHost.instance.pitch, -Mathf.Rad2Deg * SocketHost.instance.yaw, -Mathf.Rad2Deg * SocketHost.instance.roll);
    }

    void UpdateControllerController()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            if (controllerCurrentlySelecting)
            {
				Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);
                // Move around sticky/item
            }
        }
    }

    void UpdateControllerSticky()
    {
        
    }
}
