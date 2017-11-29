using UnityEngine;

/*
 * Script use to swap between different camera modes; 1st person, Birds View, and No Clip.
 * 1st person mode is used to let the player walk around the terrain
 * Birds View provides a top down perspective on the terrain
 * No Clip mode enables the player to fly around the scene 
 **/ 
public class CameraModeController : MonoBehaviour
{

    // Player Object
    public GameObject playerObject;

    // Bools used to monitor between the different camera controll modes
    private bool eagleVision = false;
    private bool noClip = false;
    private bool cursorEnabled = false;

    // Player's child camera 
    private Transform cameraChild;

    // Original player rotation
    private Quaternion playerRotation;

    /*
     * Retrieve camera child on start-up
     **/
    private void Start()
    {
        Cursor.visible = false;
        cameraChild = playerObject.transform.Find("Camera");
    }

    private void Update()
    {
        // Change camera position and rotation only on button click
        if (Input.GetKeyDown(KeyCode.V) && !noClip)
        {
            BirdsView();
        }

        // Change camera position and rotation only on button click
        if (Input.GetKeyDown(KeyCode.N))
        {
            // Reseting camera position when going to No clip from Birds View
            cameraChild.localPosition = new Vector3(
                0,
                0.5f,
                0
            );
            eagleVision = false;

            NoClipMode();
        }

        // Change camera position and rotation only on button click
        if (Input.GetKeyDown(KeyCode.C))
        {
            CameraCursorToggle();
        }

    }

    void BirdsView()
    {

        // Execute when Bird View selected and in 1st Person Mode
        if (!eagleVision)
        {
            // Set bool to true (bird view mode)
            eagleVision = true;

            // Removing script that rotates camera with mouse movement
            Destroy(cameraChild.GetComponent<MouseLook>());

            // Get the playerRotation before switching to Bird's view
            playerRotation = playerObject.transform.rotation;

            // Player movement script is based on where the player is looking
            // by reseting the player rotation, the player will move in global rotation
            playerObject.transform.rotation = Quaternion.Euler(0, 0, 0);

            // Position the camera above the player. The camera y-position 
            // is 1 less than entered, because the camera is Player object child
            cameraChild.position = new Vector3(
                playerObject.transform.position.x,
                31,
                playerObject.transform.position.z
            );

            // Rotate camera to look down
            cameraChild.rotation = Quaternion.Euler(90, 0, 0);
        }
        // Execute when returning to 1st person view
        else
        {
            // Set bool to false (1st person mode)
            eagleVision = false;

            // Setting the child position for the camera
            cameraChild.localPosition = new Vector3(
                0,
                0.5f,
                0
            );

            // Setting the player and camera rotations to face the original way
            playerObject.transform.rotation = playerRotation;
            cameraChild.rotation = playerRotation;

            // Adding script that rotates camera with mouse movement
            cameraChild.gameObject.AddComponent<MouseLook>();

            // Setting player object as a variable, so the movement is global
            cameraChild.GetComponent<MouseLook>().characterBody = playerObject;
        }
    }

    /*
     * Used to enable the player to fly around
     **/ 
    void NoClipMode()
    {

        if (!noClip)
        {
            // Set bool to true (currently in no clip mode)
            noClip = true;

            // Removing forces from player object so it does not drift away
            playerObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

            // Adding trigger to disable going through house walls
            playerObject.GetComponent<Collider>().isTrigger = true;

            // Get the playerRotation before switching to No Clip
            playerRotation = playerObject.transform.rotation;

            // Removing script that rotates camera with mouse movement and moves player
            Destroy(cameraChild.GetComponent<MouseLook>());
            Destroy(playerObject.GetComponent<PlayerMovement>());

            // Add camera orbiting script and initialize the camera's position and rotation
            // to face the way it faced before swapping to no clip
            cameraChild.gameObject.AddComponent<CameraOrbit>();
            cameraChild.gameObject.GetComponent<CameraOrbit>().initialPosition
                = playerObject.transform.position;
            cameraChild.gameObject.GetComponent<CameraOrbit>().cameraDistance = 0;
        }
        // Execute when returning to 1st person view
        else
        {
            // Set bool to false (1st person mode)
            noClip = false;

            // Adding trigger to disable going through house walls
            playerObject.GetComponent<Collider>().isTrigger = false;

            // Adding 1st Person movement contols
            playerObject.AddComponent<PlayerMovement>();
            playerObject.GetComponent<PlayerMovement>().LookTransform = playerObject.transform;

            // Reset camera position in case zoom was used
            cameraChild.localPosition = new Vector3(
                0,
                0.5f,
                0
            );

            // Reseting rotation in case rotate was used
            playerObject.transform.rotation = playerRotation;

            Destroy(cameraChild.gameObject.GetComponent<CameraOrbit>());

            // Adding script that rotates camera with mouse movement
            cameraChild.gameObject.AddComponent<MouseLook>();
            cameraChild.GetComponent<MouseLook>().characterBody = playerObject;
        }
    }

    /*
     * Toggles between cursor visibility and camera movement
     **/
    void CameraCursorToggle()
    {
        // Showing cursor
        if (!cursorEnabled)
        {
            cursorEnabled = true;
            Cursor.visible = true;

            // Removing script that rotates camera with mouse movement and moves player
            Destroy(cameraChild.GetComponent<MouseLook>());
            Destroy(playerObject.GetComponent<PlayerMovement>());
        }
        // Remove cursor from screen
        else
        {
            cursorEnabled = false;
            Cursor.visible = false;

            // Adding 1st Person movement contols
            playerObject.AddComponent<PlayerMovement>();
            playerObject.GetComponent<PlayerMovement>().LookTransform = playerObject.transform;

            Destroy(cameraChild.gameObject.GetComponent<CameraOrbit>());

            // Adding script that rotates camera with mouse movement
            cameraChild.gameObject.AddComponent<MouseLook>();
            cameraChild.GetComponent<MouseLook>().characterBody = playerObject;
        }
    }
}