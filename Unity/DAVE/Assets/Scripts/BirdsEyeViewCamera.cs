using UnityEngine;
using System.Collections;

public class BirdsEyeViewCamera : MonoBehaviour
{

    public GameObject playerObject;
    public float smoothTime = 0.3f;

    private Vector3 velocity = Vector3.zero;
    private bool eagleVision = true;
    private Transform cameraChild;

    private void Start()
    {
        cameraChild = playerObject.transform.Find("Camera");
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.V))
        {


            if (eagleVision)
            {
                eagleVision = false;
                Destroy(cameraChild.GetComponent<MouseLook>());

                Vector3 cameraPosition = cameraChild.position;

                cameraChild.position = new Vector3(
                    cameraPosition.x,
                    31,
                    cameraPosition.z
                );

                cameraChild.rotation = Quaternion.Euler(90, 0, 0);

            }
            else
            {
                Vector3 playerPosition = playerObject.transform.position;
                Quaternion playerRotation = playerObject.transform.rotation;

                cameraChild.position = new Vector3(
                    playerPosition.x,
                    1.5f,
                    playerPosition.z
                );

                cameraChild.rotation = playerRotation;

                eagleVision = true;
                cameraChild.gameObject.AddComponent<MouseLook>();

            }
        }   
    }
}