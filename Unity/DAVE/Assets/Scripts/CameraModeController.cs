using UnityEngine;

public class CameraModeController : MonoBehaviour
{

    // Player Object
    public GameObject playerObject;

    // Bool used to monitor bird view selection
    private bool eagleVision = false;
    private bool noClip = false;

    // Player's child camera 
    private Transform cameraChild;

    // Original player rotation
    private Quaternion playerRotation;

    private Vector3 initialPosition;

    /*
     * Retrieve camera child on start-up
     **/
    private void Start()
    {
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
            cameraChild.localPosition = new Vector3(
                0,
                0.5f,
                0
            );

            // Setting the player and camera rotations to face the original way
            playerObject.transform.rotation = playerRotation;
            cameraChild.rotation = playerRotation;

            eagleVision = false;
            NoClipMode();
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

            // Get the position
            Vector3 cameraPosition = cameraChild.position;

            // Position the camera above the player. The camera y-position 
            // is 1 less than entered, because the camera is Player object child
            cameraChild.position = new Vector3(
                cameraPosition.x,
                31,
                cameraPosition.z
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

    void NoClipMode()
    {

        if (!noClip)
        {
            // Set bool to true (no clip mode)
            noClip = true;

            // Removing forces from player object so it does not drift away
            playerObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

            // Get the playerRotation before switching to No Clip
            playerRotation = playerObject.transform.rotation;

            // Removing script that rotates camera with mouse movement and moves player
            Destroy(cameraChild.GetComponent<MouseLook>());
            Destroy(playerObject.GetComponent<PlayerMovement>());

            // Add camera orbiting script
            cameraChild.gameObject.AddComponent<CameraOrbit>();
            cameraChild.gameObject.GetComponent<CameraOrbit>().initialPosition
                = playerObject.transform.position;
            cameraChild.gameObject.GetComponent<CameraOrbit>().cameraDistance = 0;

            playerRotation = playerObject.transform.rotation;

            // Rotate camera to look down
            cameraChild.rotation = playerRotation;
        }
        // Execute when returning to 1st person view
        else
        {
            // Set bool to false (1st person mode)
            noClip = false;

            // Adding 1st Person movement contols
            playerObject.AddComponent<PlayerMovement>();
            playerObject.GetComponent<PlayerMovement>().LookTransform = playerObject.transform;

            // Reset camera position in case zoom was used
            cameraChild.localPosition = new Vector3(
                0,
                0.5f,
                0
            );

            // Setting the player and camera rotations to face the original way
            playerObject.transform.rotation = playerRotation;
            cameraChild.rotation = playerRotation;

            Destroy(cameraChild.gameObject.GetComponent<CameraOrbit>());

            // Adding script that rotates camera with mouse movement
            cameraChild.gameObject.AddComponent<MouseLook>();
            cameraChild.GetComponent<MouseLook>().characterBody = playerObject;
        }
    }
}