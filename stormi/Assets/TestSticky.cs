using UnityEngine;

public class TestSticky : MonoBehaviour
{
    public bool grabbed;

    Transform initParent;

    void Start() {
        initParent = transform.parent;
    }

	void Update()
    {
		// Update shader based on if grabbed
	}

    public void PickedUpBy(Transform picker)
    {
        transform.parent = picker;
    }

    public void DroppedOff()
    {
        transform.parent = initParent;
    }
}
