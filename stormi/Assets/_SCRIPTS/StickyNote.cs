using UnityEngine;

public class StickyNote : MonoBehaviour
{
    public bool grabbed;

    Transform initParent;
    Quaternion boardRotation = Quaternion.Euler(0, 0, 0); // Identity?

    void Start()
    {
        initParent = transform.parent;
    }

    void Update()
    {
        // Update shader based on if grabbed?
    }

    public void PickedUpBy(Transform picker)
    {
        //transform.parent = picker;
        GetComponent<Collider>().enabled = false;
    }

    public void DroppedOff()
    {
        //transform.parent = initParent;
        GetComponent<Collider>().enabled = true;
    }

    public void MoveToRaycast(Vector3 point)
    {
        transform.position = point;
        transform.rotation = boardRotation;
    }
}
