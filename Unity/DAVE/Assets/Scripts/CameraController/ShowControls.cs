using UnityEngine.UI;
using UnityEngine;

public class ShowControls : MonoBehaviour {

    public GameObject CameraControlPanel;
    private Image Arrow;
    public Sprite downArrow;
    public Sprite upArrow;

    // Gets objects and sets click listener
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
        Arrow = GameObject.FindGameObjectWithTag("arrow_controls").GetComponent<Image>();
    }

    private void OnClick()
    {
        // In case the panel is visable
        if (CameraControlPanel.GetComponent<CanvasRenderer>().GetAlpha() == 1)
        {
            Arrow.sprite = upArrow;                 // Make drop down arrow point upwards

            // Make the panel and its children, the text and pictures, transparent
            CameraControlPanel.GetComponent<CanvasRenderer>().SetAlpha(0);
            foreach (var children in CameraControlPanel.GetComponentsInChildren<CanvasRenderer>())
            { 
                children.SetAlpha(0);
            }
        }
        else
        {
            CameraControlPanel.GetComponent<CanvasRenderer>().SetAlpha(1);
            foreach (var children in CameraControlPanel.GetComponentsInChildren<CanvasRenderer>())
            {
                children.SetAlpha(1);
            }
            Arrow.sprite = downArrow;
        }
    }
}
