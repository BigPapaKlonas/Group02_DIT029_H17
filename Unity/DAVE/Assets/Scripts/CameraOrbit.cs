using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Vector3 initialPosition;
    public float cameraDistance;            // Distance from Camera

    private Transform cameraPosition;       // The camera being rotated
    private Transform cameraParent;         // The object the camera is being rotated about

    private Vector3 cameraInitialPosition;  // Stores the camera's initial position
    private Vector3 cameraRotation;         // Store camera rotation frames
    private Vector3 oldPosition;            // CameraParent's position before draging
    private Vector3 newPosition;            // Used when draging cameraParent

    private float movingSpeed = 4f;         // Camera Rotation speed
    private float rotateDampening = 10f;    // Used for a smoother rotation
    private float scrollDampening = 6f;     // Used for a smoother zoom

    private bool rightClicked = false;      // Used to check if RMB has been pressed down

    // Initialization of transform objects
    void Start()
    {
        cameraParent = transform.parent;
        cameraPosition = transform;
        cameraRotation.x = -90f;            // Rotates camera on start to face the diagram

        cameraParent.position = initialPosition;        // Positions camera on start
        cameraInitialPosition = cameraParent.position;  // Saves initial camera position
    }

    // LateUpdate() is used to render camera last to avoid rotation/render issues
    void LateUpdate()
    {

        // Call method to move camera
        MoveCameraKeyboard();

        // Call method to rotate camera
        RotateCamera();

        // If mouse scroll wheel has been moved call method to zoom camera
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            ZoomCamera();
        }

        // Rotates y amount of degrees along x-axis and vise versa
        Quaternion QT = Quaternion.Euler(cameraRotation.y, cameraRotation.x, 0);

        // Animates the rotation
        cameraParent.rotation = Quaternion.Lerp(cameraParent.rotation, QT,
            Time.deltaTime * rotateDampening);

        // Animates zooming in and out
        if (cameraPosition.localPosition.z != cameraDistance * -1f)
        {
            cameraPosition.localPosition = new Vector3(
                0f,
                0f,
                Mathf.Lerp(
                    cameraPosition.localPosition.z,
                    cameraDistance * -1f,
                    Time.deltaTime * scrollDampening
                )
            );
        }
    }

    /*
     * Rotates camera using RMB
     **/ 
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
                // Change camera rotation's x value
                cameraRotation.x += Input.GetAxis("Mouse X") * movingSpeed;
                // Change camera rotation's y value
                cameraRotation.y += Input.GetAxis("Mouse Y") * movingSpeed;  

                //Clamps the y coordinate so the camera does not flip/go over y = 0
                if (cameraRotation.y < 0f)
                {
                    cameraRotation.y = 0f;
                }
                else if (cameraRotation.y > 90f)
                {
                    cameraRotation.y = 90f;
                }
            }
        }
    }

    /* 
     * Zooms in and out with scrolling pad
     **/
    void ZoomCamera()
    {
        // Calculate the distance to scroll, 0.5f just a multiplier, Scrollwhell
        float scrollLength = Input.GetAxis("Mouse ScrollWheel") * movingSpeed * 0.5f;
        // Smoother zoom, the further from the start the quicker the zoom 
        scrollLength *= (cameraDistance * 0.3f);

        // Moves the camera when zooming
        cameraDistance += scrollLength * -1f;
        // Sets min and max bounds for zoom length
        cameraDistance = Mathf.Clamp(cameraDistance, 0.1f, 100f);
    }

    /*
     * Resets camera position
     **/ 
    public void ResetCamera()
    {
        cameraParent.position = cameraInitialPosition;  // Sets current camera positon to initial
        cameraRotation.x = -90f;                        // Resets rotation
        cameraRotation.y = 0f;
    }

    public void SetPosition(Vector3 targetPosition)
    {
        cameraParent.position = targetPosition;
        cameraRotation.x = -90f;                        // Resets rotation
        cameraRotation.y = 0f;
    }

    /*
     * Moves the camera parent using the keyboard
     **/ 
    void MoveCameraKeyboard()
    {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            // Move camera right (relative to where it's facing)
            cameraParent.position += cameraParent.right * movingSpeed * 2.5f * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            // Move camera left (relative to where it's facing) by inverting .right
            cameraParent.position -= cameraParent.right * movingSpeed * 2.5f * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            // Move camera forward (relative to where it's facing)
            cameraParent.position += cameraParent.forward * movingSpeed * 2.5f * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            // Move camera back (relative to where it's facing) by inverting .forward
            cameraParent.position -= cameraParent.forward * movingSpeed * 2.5f * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            // Move camera up along Y axis by using Vector3.up
            cameraParent.position += Vector3.up * movingSpeed * 2.5f * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            // Move camera down along Y axis by inverting Vector3.up
            cameraParent.position -= Vector3.up * movingSpeed * 2.5f * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.R))
        {
            ResetCamera();
        }
    }
}
