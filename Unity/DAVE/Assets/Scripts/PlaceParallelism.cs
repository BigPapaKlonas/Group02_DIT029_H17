using UnityEngine;

public class PlaceParallelism : MonoBehaviour {

    private Transform parallelBox;
    private Vector3 targetLocation;

    private float xMax;
    private float yMax;
    private float zMax;

    private float xMin;
    private float yMin;
    private float zMin;

    private Vector3 maxObject;
    private Vector3 minObject;

    GameObject[] myObjects;

    // Use this for initialization
    void Start () {
        parallelBox = transform;
        myObjects = GameObject.FindGameObjectsWithTag("Activation_Box");
}
	
	// Update is called once per frame
	void Update () {
        FindMax(myObjects);
        FindMin(myObjects);

        parallelBox.position = (maxObject + minObject) / 2;
        parallelBox.localScale = maxObject - minObject;
        
        PositionObject myInstance = parallelBox.GetComponent<PositionObject>();
        myInstance.xPosition = parallelBox.position.x;
        myInstance.yPosition = parallelBox.position.y;
        myInstance.zPosition = parallelBox.position.z;

        ScaleObject myScale = parallelBox.GetComponent<ScaleObject>();
        myScale.xScale = parallelBox.localScale.x + 1.5f;
        myScale.yScale = parallelBox.localScale.y + 1.5f;
        myScale.zScale = parallelBox.localScale.z + 1.5f;
    }

    void FindMax(GameObject[] gameObjectArray)
    {
        foreach (GameObject obj in gameObjectArray)
        {
            xMax = Mathf.Max(obj.transform.position.x, xMax);
            yMax = Mathf.Max(obj.transform.position.y, yMax);
            zMax = Mathf.Max(obj.transform.position.z, zMax);
        }
        maxObject = new Vector3(xMax, yMax, zMax);
    }

    void FindMin(GameObject[] gameObjectArray)
    {
        foreach (GameObject obj in gameObjectArray)
        {
            xMin = Mathf.Min(obj.transform.position.x, xMin);
            yMin = Mathf.Min(obj.transform.position.y, yMin);
            zMin = Mathf.Min(obj.transform.position.z, zMin); 
        }
        minObject = new Vector3(xMin, yMin, zMin);
    }
}
