using UnityEngine;
using UnityEngine.UI;

public class ResetCamera : MonoBehaviour
{
    private Button resetCameraBtn;
    private CameraMover cameraMoverScript;
    private GameObject player;


    void Start()
    {
        resetCameraBtn = GetComponent<Button>();
        resetCameraBtn.onClick.AddListener(OnClick);
        // Gets player object
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Resets the player position (the camera is attached to player) to its original spawn position
    private void OnClick()
    {
        player.transform.position = new Vector3(-40, 1, -90);
    }
}
