using UnityEngine;

public class ScaleObject : MonoBehaviour
{

    public float xScale;
    public float yScale;
    public float zScale;

    private Vector3 scaleObject;
    private Transform child;

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
        transform.localScale = transform.localScale;
        scaleObject = new Vector3(xScale, yScale, zScale);
        transform.localScale = scaleObject;
    }
}
