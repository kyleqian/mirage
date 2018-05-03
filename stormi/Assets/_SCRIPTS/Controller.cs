using UnityEngine;

public class Controller : MonoBehaviour
{
    public enum ControllerState { Controller, Sticky };
    public ControllerState state;

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

    void UpdatePose()
    {
        transform.localEulerAngles = new Vector3(-Mathf.Rad2Deg * SocketHost.instance.pitch, -Mathf.Rad2Deg * SocketHost.instance.yaw, -Mathf.Rad2Deg * SocketHost.instance.roll);
    }

    void UpdateControllerController()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.yellow);
        }
    }

    void UpdateControllerSticky()
    {
        
    }
}
