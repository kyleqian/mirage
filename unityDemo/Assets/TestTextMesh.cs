using UnityEngine;

public class TestTextMesh : MonoBehaviour {

    private TextMesh tm;

    void Start()
    {
        tm = GetComponent<TextMesh>();
    }

	void Update () {
        tm.text = SocketClient.Instance.getTotalText();
	}
}
