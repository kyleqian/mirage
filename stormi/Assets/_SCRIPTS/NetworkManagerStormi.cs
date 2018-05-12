using UnityEngine;
using UnityEngine.Networking;

public class NetworkManagerStormi : MonoBehaviour
{
	bool isStarted = false;
	NetworkManager nm;

	void Start()
	{
		nm = GetComponent<NetworkManager>();
	}

	void Update()
	{
		if (!isStarted)
		{
			#if UNITY_EDITOR || UNITY_STANDALONE
			if (Input.GetKeyDown(KeyCode.H))
			{
				nm.StartHost();
				isStarted = true;
			}
			else if (Input.GetKeyDown(KeyCode.S))
			{
				nm.StartServer();
				isStarted = true;
			}
			#else
			try
			{
				nm.StartClient();
			}
			catch
			{
				nm.StartHost();
			}
			isStarted = true;
			#endif
		}
	}
}
