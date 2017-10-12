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
        myScale.xScale = parallelBox.localScale.x + 1;
        myScale.yScale = parallelBox.localScale.y + 1;
        myScale.zScale = parallelBox.localScale.z + 1;
    }

    void FindMax(GameObject[] gameObjectArray)
    {
        foreach (GameObject obj in gameObjectArray)
        {
            if (obj.transform.position.x > xMax)
            {
                xMax = obj.transform.position.x;
            }
            if (obj.transform.position.y > yMax)
            {
                yMax = obj.transform.position.y;
            }
            if (obj.transform.position.z > zMax)
            {
                zMax = obj.transform.position.z;
            }
        }

        maxObject = new Vector3(xMax, yMax, zMax);
    }

    void FindMin(GameObject[] gameObjectArray)
    {
        foreach (GameObject obj in gameObjectArray)
        {
            if (obj.transform.position.x < xMin)
            {
                xMin = obj.transform.position.x;
            }
            if (obj.transform.position.y < yMin)
            {
                yMin = obj.transform.position.y;
            }
            if (obj.transform.position.z < zMin)
            {
                zMin = obj.transform.position.z;
            }
        }
        minObject = new Vector3(xMin, yMin, zMin);
    }
}
