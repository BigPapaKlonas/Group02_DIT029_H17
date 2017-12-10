using UnityEngine;
using UnityEngine.UI;

public class ResetCamera : MonoBehaviour
{
    private Button resetCameraBtn;
    private CameraMover cameraMoverScript;

    void Start()
    {
        resetCameraBtn = GetComponent<Button>();
        resetCameraBtn.onClick.AddListener(OnClick);
        // Gets main camera
        Camera mainCamera = Camera.main;
        // Gets the cameraMover script
        cameraMoverScript = (CameraMover)mainCamera.GetComponent(typeof(CameraMover));  
    }

    private void OnClick()
    {
        // Reset the camera when the R key is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            cameraMoverScript.ResetCamera();    // Calls if the button is clicked
        }
    }
}
