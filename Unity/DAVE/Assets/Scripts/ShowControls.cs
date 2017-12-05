using UnityEngine.UI;
using UnityEngine;

public class ShowControls : MonoBehaviour {

    private GameObject CameraControlPanel;
    private Image Arrow;
    public Sprite downArrow;
    public Sprite upArrow;

    // Gets objects and sets click listener
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
        CameraControlPanel = GameObject.FindGameObjectWithTag("CameraControlPanel");
        Arrow = GameObject.FindGameObjectWithTag("arrow_controls").GetComponent<Image>();
    }

    private void OnClick()
    {
        Debug.Log("clicked");
        /*
        if (CameraControlPanel.activeSelf)          // If active
        {
            CameraControlPanel.SetActive(false);    // Deactivates panel
            Arrow.sprite = upArrow;                 // Make drop down arrow point upwards
        }
        else
        { 
            Arrow.sprite = downArrow;
            CameraControlPanel.SetActive(true);
        }
        **/
    }
}
