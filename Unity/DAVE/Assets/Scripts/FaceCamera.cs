using UnityEngine;

public class FaceCamera : MonoBehaviour
{

    public new Camera camera;

    void Update()
    {
        transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward,
            camera.transform.rotation * Vector3.up);
    }
}