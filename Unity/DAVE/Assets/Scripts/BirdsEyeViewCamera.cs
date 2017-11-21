using UnityEngine;
using System.Collections;

public class BirdsEyeViewCamera : MonoBehaviour
{

    // Player Object
    public GameObject playerObject;

    // Bool used to monitor bird view selection
    private bool eagleVision = false;

    // Player's child camera 
    private Transform cameraChild;

    // Original player rotation
    private Quaternion playerRotation;

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
        if (Input.GetKeyDown(KeyCode.V))
        {

            // Execute when Bird View selected
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

                // Position the camera above the player
                cameraChild.position = new Vector3(
                    cameraPosition.x,
                    31,                 // The camera y-position is 1 less than entered
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

                // Set camera to equal player position
                cameraChild.position = new Vector3(
                    playerObject.transform.position.x,
                    // The camera y-position is 1 less than entered
                    1.5f,  
                    playerObject.transform.position.z
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
    }
}