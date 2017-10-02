using UnityEngine;

public class CameraOrbit : MonoBehaviour
{

    private Transform cameraPosition;   // The camera being rotated
    private Transform cameraParent;     // The object the camera is being rotated about

    private Vector3 cameraRotation;     // Store camera rotation frames
    private Vector3 oldPosition;         // CameraParent's position before draging
    private Vector3 newPosition;        //

    private float cameraDistance = 10f; // Initial Distance from Camera
    private float mouseSpeed = 4f;      // Camera Rotation speed
    private float scrollSpeed = 2f;     // Camera Zoom speed
    private float rotateDampening = 10f;// Used for a smoother rotation
    private float scrollDampening = 6f; // USed for a smoother zoom

    bool dragging = false;
    Plane movePlane;

    private bool rightClicked = false;   // Used to check if RMB (Right Mouse Button) has been pressed down

    // Initialization of transform objects
    void Start()
    {
        this.cameraPosition = this.transform;
        this.cameraParent = this.transform.parent;
    }

    // LateUpdate() is used to render camera last to avoid rotation/render issues
    void LateUpdate()
    {
        // Call method to move camera
        MoveCamera();

        // Call method to rotate camera
        RotateCamera();

        // If mouse scroll wheel has been moved call method to zoom camera
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            ZoomCamera();
        }

        //Actual Camera Transformations
        Quaternion QT = Quaternion.Euler(cameraRotation.y, cameraRotation.x, 0); // Rotates y amount of degrees along x-axis and vise versa

        // Animates the rotation
        this.cameraParent.rotation = Quaternion.Lerp(this.cameraParent.rotation, QT, Time.deltaTime * rotateDampening);

        // Animates zooming in and out
        if (this.cameraPosition.localPosition.z != this.cameraDistance * -1f)
        {
            this.cameraPosition.localPosition = new Vector3(0f, 0f, Mathf.Lerp(this.cameraPosition.localPosition.z, 
                this.cameraDistance * -1f, Time.deltaTime * scrollDampening));
        }
    }

    void OnMouseDown()
    {
        dragging = true;
        movePlane = new Plane(-Camera.main.transform.forward, transform.position);
    }

    void RotateCamera()
    {
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

        // If RMD is pressed down rotate camera
        if (rightClicked)
        {
            //Rotation of the Camera based on Mouse Coordinates
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                cameraRotation.x += Input.GetAxis("Mouse X") * mouseSpeed;  // Change camera's x coordinate
                cameraRotation.y += Input.GetAxis("Mouse Y") * mouseSpeed;  // Change camera's y coordinate

                //Clamps the camera's y coordinate so the camera does not flip or go over the horizon while rotating
                if (cameraRotation.y < 0f)
                    cameraRotation.y = 0f;
                else if (cameraRotation.y > 90f)
                    cameraRotation.y = 90f;
            }
        }
    }

    void ZoomCamera()
    {
        float scrollLength = Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;  // Calculate the distance to scroll
        scrollLength *= (this.cameraDistance * 0.3f);                           // Smoother zoom, the further from the start the quicker the zoom                    

        this.cameraDistance += scrollLength * -1f;                              // Moves the camera when zooming
        this.cameraDistance = Mathf.Clamp(this.cameraDistance, 1.5f, 100f);     // Sets min and max bounds for zoom length
    }

    void MoveCamera()
    {
        // If LMB has been pressed down save the camera parent's locations
        if (Input.GetMouseButtonDown(0))
        {
            oldPosition = cameraParent.position;                                    // oldPosition initialized to camera parent's location when LMB clicked
            newPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);   // Getting the position of where the parent was moved
        }

        // Moves the camera parent object
        if (Input.GetMouseButton(0))
        {
            Vector3 currentPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition) - newPosition; // Get the difference between where the mouse clicked and where it moved
            cameraParent.position = oldPosition - currentPosition * (mouseSpeed * 1.5f);                    // Moving the camera parent's position by reinitializing cameraParent's position
        }
    }
}