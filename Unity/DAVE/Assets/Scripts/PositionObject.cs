using UnityEngine;

public class PositionObject : MonoBehaviour
{

    public float xPosition;
    public float yPosition;
    public float zPosition;

    private Vector3 scaleObject;

    // Initialization
    void Start()
    {
        this.scaleObject = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        display();
    }

    void display()
    {
        scaleObject = new Vector3(xPosition, yPosition, zPosition);
        transform.position = scaleObject;
    }
}
