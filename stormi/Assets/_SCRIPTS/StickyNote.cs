using UnityEngine;

public class StickyNote : MonoBehaviour
{
    Collider collider;

    void Start()
    {
        collider = GetComponent<Collider>();
    }

    void Update()
    {
        // Update shader based on if grabbed?
    }

    public void PickedUpBy(Transform picker)
    {
        collider.enabled = false;
    }

    public void DroppedOff()
    {
        collider.enabled = true;
    }

    public void MoveToRaycast(RaycastHit hit)
    {
        transform.position = hit.point;
        transform.LookAt(hit.point - Vector3.Normalize(hit.normal), Vector3.up);
    }
}
