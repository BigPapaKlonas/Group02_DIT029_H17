using UnityEngine;

public class PlaceParallelism : MonoBehaviour
{

    private Transform parallelBox;

    // Maximum Points for Activation Boxes
    private float xMax;
    private float yMax;
    private float zMax;

    // Minimum Points for Activation Boxes
    private float xMin;
    private float yMin;
    private float zMin;

    // Vectors use for parallelBox transformation
    private Vector3 maxObject;
    private Vector3 minObject;

    // The Y-Coordinate of the sequential activation box
    private float seqBoxY;

    // Array of game objects with "Activation_Box" tag
    GameObject[] myObjects;

    // Used for initialization
    void Start()
    {
        parallelBox = transform;
        myObjects = GameObject.FindGameObjectsWithTag("Activation_Box");
    }

    // Update is called once per frame
    void Update()
    {
        FindMaxMin(myObjects);

        parallelBox.position = Vector3.Lerp(maxObject, minObject, 0.5f);

        // Adding extra size to the max object to make the parallel box go outside the activation boxes
        maxObject = new Vector3(maxObject.x + 1f, maxObject.y + 1f, maxObject.z + 1f);
        parallelBox.localScale = maxObject - minObject;
    }

    /*
     * Finds the maximum and minimum points of the all the activation boxes present in scene
     * and makes Vector3 with these values, which are used for positioning and scalling
     * the parallelBox's transform
     */
    void FindMaxMin(GameObject[] gameObjectArray)
    {
        // Finding max and min points of the activation boxes
        foreach (GameObject obj in gameObjectArray)
        {
            xMax = Mathf.Max(obj.transform.position.x, xMax);
            yMax = Mathf.Max(obj.transform.position.y, yMax);
            zMax = Mathf.Max(obj.transform.position.z, zMax);

            xMin = Mathf.Min(obj.transform.position.x, xMin);
            yMin = Mathf.Min(obj.transform.position.y, yMin);
            zMin = Mathf.Min(obj.transform.position.z, zMin);
        }
        maxObject = new Vector3(xMax, yMax, zMax);
        minObject = new Vector3(xMin, yMin, zMin);
    }
}
