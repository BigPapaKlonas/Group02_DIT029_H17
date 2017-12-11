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

    private void OnClick()
    {
        player.transform.position = new Vector3(0, 1, 0);
    }
}
