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

    private float seqBoxPosition = 0;
    private bool isSeqBox;

    private Vector3 maxObject;
    private Vector3 minObject;

    GameObject[] myObjects;
    private float parPosition;
    private float pos;

    // Use this for initialization
    void Start () {
        parallelBox = transform;
        myObjects = GameObject.FindGameObjectsWithTag("Activation_Box");
    }
	
	// Update is called once per frame
	void Update () {
        FindSeqBox();
        FindMaxMin(myObjects);
        ScaleLineChildren();

        parallelBox.position = Vector3.Lerp(maxObject, minObject, 0.5f);
        parallelBox.localScale = maxObject - minObject;
        
        PositionObject myInstance = parallelBox.GetComponent<PositionObject>();
        myInstance.xPosition = parallelBox.position.x;
        myInstance.yPosition = parallelBox.position.y;
        myInstance.zPosition = parallelBox.position.z;

        ScaleObject myScale = parallelBox.GetComponent<ScaleObject>();
        myScale.xScale = parallelBox.localScale.x + 1f;
        myScale.yScale = parallelBox.localScale.y + 1f;
        myScale.zScale = parallelBox.localScale.z + 1f;

        parPosition = parallelBox.position.y + parallelBox.localScale.y / 2;
    }

    void FindMaxMin(GameObject[] gameObjectArray)
    {
        foreach (GameObject obj in gameObjectArray)
        {
            xMax = Mathf.Max(obj.transform.position.x, xMax);
            yMax = Mathf.Max(obj.transform.position.y, yMax);
            zMax = Mathf.Max(obj.transform.position.z, zMax);

            xMin = Mathf.Min(obj.transform.position.x, xMin);
            yMin = Mathf.Min(obj.transform.position.y, yMin);
            zMin = Mathf.Min(obj.transform.position.z, zMin);

            // Finds the top of the seq activation box
            /*
             * Check if box is from "seq" node. If true then assign 
             * seqBoxPosition to be the y of the obj, which will then be used to set the 
             * offset of line texture
            if (obj == isSeqBox)
            {
                seqBoxPosition = obj.transform.position.y 
                    + (obj.transform.localScale.y / 2);
            }
            **/
        }
        maxObject = new Vector3(xMax, yMax, zMax);
        minObject = new Vector3(xMin, yMin, zMin);
    }

    void ScaleLineChildren()
    {
        float scaleLineX = parallelBox.GetChild(0).transform.localScale.x;
        float scaleLineY = 0.05f;
        float scaleLineZ = parallelBox.GetChild(0).transform.localScale.z;

        foreach (Transform line in parallelBox.transform)
        {
            line.transform.localScale = new Vector3(scaleLineX, scaleLineY, scaleLineZ);
            line.transform.position = new Vector3(line.transform.position.x, 
                pos, line.transform.position.z);
        }
        // Scalling side line
        parallelBox.GetChild(5).transform.localScale = new Vector3(scaleLineX, scaleLineY, 0.5f);
        parallelBox.GetChild(11).transform.localScale = new Vector3(scaleLineX, scaleLineY, 0.5f);
    }

    void FindSeqBox()
    {
        GameObject posObj = myObjects[2];
        pos = posObj.transform.position.y + posObj.transform.localScale.y / 2 + 0.02f;
    }
}
