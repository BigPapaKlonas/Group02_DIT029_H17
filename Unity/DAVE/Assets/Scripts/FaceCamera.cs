using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    // Camera you want to face
    public new Camera camera;

    void Update()
    {
        // Makes the attached object's transform look at the camera
        this.transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward,
            camera.transform.rotation * Vector3.up);
    }
}
