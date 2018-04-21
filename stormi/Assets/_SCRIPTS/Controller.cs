using UnityEngine;

public class Controller : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.localEulerAngles = new Vector3(-Mathf.Rad2Deg * SocketHost.instance.pitch, -Mathf.Rad2Deg * SocketHost.instance.yaw, -Mathf.Rad2Deg * SocketHost.instance.roll);
	}
}
