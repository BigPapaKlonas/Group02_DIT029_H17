using UnityEngine;
using UnityEngine.UI;

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

    // LogCamera
    public Camera logCamera;
    private bool logCameraEnabled;

    // ToggleKey for disabling camera mode
    public KeyCode toggleKey = KeyCode.L;

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
        // Check when logCameraEnabled is true
        if (logCameraEnabled)
        {
			// Change color depending on press
			GameObject buttonColor = GameObject.Find ("LKeyImage");
			buttonColor.GetComponent<Image> ().color = Color.green;

            if (Input.GetKeyDown(toggleKey))    // Disable log camera mode on toggle key pressed
            {
				// Change color depending on press
				buttonColor.GetComponent<Image> ().color = Color.white;

                playerObject.GetComponentInChildren<Camera>().enabled = true;
                logCamera.enabled = false;
                playerObject.GetComponent<Detection>().enabled = true;
                logCameraEnabled = false;
            }
        }


        // Change camera position and rotation only on button click
        if (Input.GetKeyDown(KeyCode.B) && !noClip)
        {
            BirdsView();
        }

        // Change camera position and rotation only on button click
        if (Input.GetKeyDown(KeyCode.I))
        {
            // Reseting camera position/rotation when going to No clip from Birds View
            cameraChild.localPosition = new Vector3(
                0,
                0.5f,
                0
            );
            cameraChild.rotation = Quaternion.Euler(0, 0, 0);
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

			// Change color depending on press
			GameObject buttonColor = GameObject.Find ("BKeyImage");
			buttonColor.GetComponent<Image> ().color = Color.green;

            // Adding a moving script in case swapping from Cursor mode
            playerObject.GetComponent<PlayerMovement>().enabled = true;

            // Removing script that rotates camera with mouse movement
            cameraChild.GetComponent<MouseLook>().enabled = false;

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

			// Change color depending on press
			GameObject buttonColor = GameObject.Find ("BKeyImage");
			buttonColor.GetComponent<Image> ().color = Color.white;


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
            cameraChild.gameObject.GetComponent<MouseLook>().enabled = true;
        }

        // If cursor mode on while swapping, disable movement 
        // Previous if will get all the position and rotation data
        if (cursorEnabled)
        {
            playerObject.GetComponent<PlayerMovement>().enabled = false;
            cameraChild.GetComponent<MouseLook>().enabled = false;
            if(cameraChild.gameObject.GetComponent<CameraMover>() != null)
            {
                Destroy(cameraChild.gameObject.GetComponent<CameraMover>());
            }
        }

    }

    /*
     * Used to enable the player to fly around
     **/
    public void NoClipMode()
    {

        if (!noClip)
        {
            // Set bool to true (currently in no clip mode)
            noClip = true;

			// Change color depending on press
			GameObject buttonColor = GameObject.Find ("IKeyImage");
			buttonColor.GetComponent<Image> ().color = Color.green;

            // Removing forces from player object so it does not drift away
            playerObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

            // Adding trigger to disable going through house walls
            playerObject.GetComponent<Collider>().isTrigger = true;

            // Get the playerRotation before switching to No Clip
            playerRotation = playerObject.transform.rotation;

            // Removing script that rotates camera with mouse movement and moves player
            cameraChild.GetComponent<MouseLook>().enabled = false;
            playerObject.GetComponent<PlayerMovement>().enabled = false;

            // Add camera orbiting script and reinitialize the camera's position and rotation
            // to face the way it faced before swapping to no clip
            if (cameraChild.gameObject.GetComponent<CameraMover>() == null)
            {
                cameraChild.gameObject.AddComponent<CameraMover>();
            }

            cameraChild.gameObject.GetComponent<CameraMover>().initialPosition
               = playerObject.transform.position;
        }

        // Execute when returning to 1st person view
        else
        {
            // Set bool to false (1st person mode)
            noClip = false;

			// Change color depending on press
			GameObject buttonColor = GameObject.Find ("IKeyImage");
			buttonColor.GetComponent<Image> ().color = Color.white;


            // Adding trigger to disable going through house walls
            playerObject.GetComponent<Collider>().isTrigger = false;

            // Adding 1st Person movement contols
            playerObject.GetComponent<PlayerMovement>().enabled = true;

            // Reset camera position in case zoom was used
            cameraChild.localPosition = new Vector3(
                0,
                0.5f,
                0
            );

            // Reseting rotation in case rotate was used
            playerObject.transform.rotation = playerRotation;

            Destroy(cameraChild.gameObject.GetComponent<CameraMover>());

            // Adding script that rotates camera with mouse movement
            cameraChild.gameObject.GetComponent<MouseLook>().enabled = true;
        }

        // If cursor mode on while swapping, disable movement 
        // Previous if will get all the position and rotation data
        if (cursorEnabled)
        {
            playerObject.GetComponent<PlayerMovement>().enabled = false;
            cameraChild.GetComponent<MouseLook>().enabled = false;
            cameraChild.GetComponent<CameraMover>().enabled = false;
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
            Debug.Log("!cursorEnabled");
            cursorEnabled = true;
            Cursor.visible = true;

			// Change color depending on press
			GameObject buttonColor = GameObject.Find ("CKeyImage");
			buttonColor.GetComponent<Image> ().color = Color.green;


            // Removing forces from player object so it does not drift away
            playerObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

            // Removing script that rotates camera with mouse movement and moves player
            cameraChild.GetComponent<MouseLook>().enabled = false;
            playerObject.GetComponent<PlayerMovement>().enabled = false;
            Destroy(cameraChild.gameObject.GetComponent<CameraMover>());
        }

        // If swapping to birdsview enable movement controll
        else if (cursorEnabled && eagleVision)
        {
            Debug.Log("cursorEnabled && eagleVision");

			// Change color depending on press
			GameObject buttonColor = GameObject.Find ("CKeyImage");
			buttonColor.GetComponent<Image> ().color = Color.white;

            cursorEnabled = false;
            Cursor.visible = false;

            playerObject.GetComponent<PlayerMovement>().enabled = true;
        }

        // If swapping to noClip disable camera orbit script
        else if (cursorEnabled && noClip)
        {
            Debug.Log("cursorEnabled && noClip");

			// Change color depending on press
			GameObject buttonColor = GameObject.Find ("CKeyImage");
			buttonColor.GetComponent<Image> ().color = Color.white;

            cursorEnabled = false;
            Cursor.visible = false;

            // Add camera orbiting script and reinitialize the camera's position and rotation
            // to face the way it faced before swapping to no clip
            cameraChild.gameObject.AddComponent<CameraMover>();

            cameraChild.gameObject.GetComponent<CameraMover>().initialPosition
               = playerObject.transform.position;
        }

        // Remove cursor from screen
        else
        {

			// Change color depending on press
			GameObject buttonColor = GameObject.Find ("CKeyImage");
			buttonColor.GetComponent<Image> ().color = Color.white;

            Debug.Log("else");

            cursorEnabled = false;
            Cursor.visible = false;

            // Adding 1st Person movement contols
            playerObject.GetComponent<PlayerMovement>().enabled = true;

            if (cameraChild.gameObject.GetComponent<CameraMover>() != null)
            {
                Destroy(cameraChild.gameObject.GetComponent<CameraMover>());
            }

            // Adding script that rotates camera with mouse movement
            cameraChild.gameObject.GetComponent<MouseLook>().enabled = true;
        }
    }

 
    public void SetPosition(Vector3 targetPosition)
    {
        playerObject.GetComponent<Detection>().enabled = false;
        playerObject.GetComponentInChildren<Camera>().enabled = false;
        logCamera.enabled = true;
        logCamera.transform.position = targetPosition;
        logCamera.transform.rotation = Quaternion.Euler(0, -90, 0); // Resets rotation

        logCameraEnabled = true;
    }

}