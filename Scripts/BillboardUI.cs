using UnityEngine;

public class BillboardUI : MonoBehaviour
{
    private Transform cam;

    void Start()
    {
        if (Camera.main != null)
            cam = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (cam == null) return;

        // direction from camera to object
        Vector3 dir = transform.position - cam.position;
        dir.y = 0f; // keeps camera it upright

        if (dir.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(dir);
    }
}
