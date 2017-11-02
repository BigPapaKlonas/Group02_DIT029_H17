using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ResetCameraBtn : MonoBehaviour, IPointerDownHandler
{
    public Camera mainCamera;
    private Button resetCameraBtn;
    private CameraOrbit cameraOrbitScript;


    void Start()
    {
        resetCameraBtn = GetComponent<Button>();
        resetCameraBtn.onClick.AddListener(OnClick);
        cameraOrbitScript = (CameraOrbit)mainCamera.GetComponent(typeof(CameraOrbit));  // Gets the cameraOrbit script

    }

    public void OnPointerDown(PointerEventData eventData) { }

    private void OnClick()
    {
        cameraOrbitScript.ResetCamera();    // Calls if the button is clicked
    }
}