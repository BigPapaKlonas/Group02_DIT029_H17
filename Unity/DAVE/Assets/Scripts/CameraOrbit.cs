using UnityEngine;

public class CameraOrbit : MonoBehaviour
{

    private Transform cameraPosition;   // The camera being rotated
    private Transform cameraParent;     // The object the camera is being rotated about

    private Vector3 cameraRotation;     // Store camera rotation frames
    private float cameraDistance = 10f; // Initial Distance from Camera

    public float mouseSpeed = 4f;       // Camera Rotation speed
    public float scrollSpeed = 2f;      // Camera Zoom speed
    public float rotateDampening = 10f; // Used for a smoother rotation
    public float scrollDampening = 6f;  // USed for a smoother zoom

    public bool rightClicked = false;   // Used to check if RMB (Right Mouse Button) has been pressed down

    // Initialization of transform objects
    void Start()
    {
        this.cameraPosition = this.transform;
        this.cameraParent = this.transform.parent;
    }

    // LateUpdate() is used to render camera last to avoid rotation/render issues
    void LateUpdate()
    {
        // If mouse scroll wheel has been moved call method ZoomCamera()
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            ZoomCamera();
        }

        // If RMB has been pressed set rightCicked to true
        if (Input.GetMouseButtonDown(1))
        {
            rightClicked = true;
        }
        // If RMB has been released set rightCicked to false
        else if (Input.GetMouseButtonUp(1))
        {
            rightClicked = false;
        }

        // Rotate only if RMD is pressed down
        if (rightClicked)
        {
            RotateCamera();   
        }

        //Actual Camera Rig Transformations
        Quaternion QT = Quaternion.Euler(cameraRotation.y, cameraRotation.x, 0);

        // Animates the rotation
        this.cameraParent.rotation = Quaternion.Lerp(this.cameraParent.rotation, QT, Time.deltaTime * rotateDampening);

        // Animates zooming in and out
        if (this.cameraPosition.localPosition.z != this.cameraDistance * -1f)
        {
            this.cameraPosition.localPosition = new Vector3(0f, 0f, Mathf.Lerp(this.cameraPosition.localPosition.z, 
                this.cameraDistance * -1f, Time.deltaTime * scrollDampening));
        }
    }

    void RotateCamera()
    {
        //Rotation of the Camera based on Mouse Coordinates
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            cameraRotation.x += Input.GetAxis("Mouse X") * mouseSpeed;  // Change camera's x coordinate
            cameraRotation.y += Input.GetAxis("Mouse Y") * mouseSpeed;  // Change camera's y coordinate

            //Clamps the camera's y coordinate so the camera does not flip or go over the horizon
            if (cameraRotation.y < 0f)
                cameraRotation.y = 0f;
            else if (cameraRotation.y > 90f)
                cameraRotation.y = 90f;
        }
    }

    void ZoomCamera()
    {
        float scrollLength = Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;  // Calculate the distance to scroll
        scrollLength *= (this.cameraDistance * 0.3f);                           // Smoother zoom, the further from the start the quicker the zoom                    

        this.cameraDistance += scrollLength * -1f;                              // Moves the camera when zooming
        this.cameraDistance = Mathf.Clamp(this.cameraDistance, 1.5f, 100f);     // Sets min and max bounds for zoom length
    }
}