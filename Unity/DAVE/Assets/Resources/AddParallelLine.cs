using UnityEngine;
using System.Collections.Generic;

public class AddParallelLine : MonoBehaviour {

    public Transform parallelBox;
    public Mesh cube;
    public float linePosition;

    private List<float> positionList = new List<float>();

	// Use this for initialization
	void Start ()
    {
        float linePositionY = this.linePosition;
        parallelBox = this.transform;
    }

    public void AddLine(float position)
    {
        //linePosition = position;
        print("addlinepos" + position);
        float positionZ = -0.4f;
        for(int i = 0; i <= 4; i++)
        {
            GameObject line = new GameObject("Line");
            line.transform.parent = parallelBox;
            
            // Gets cude mesh from parent
            line.AddComponent<MeshFilter>().sharedMesh = this.cube;
            line.AddComponent<BoxCollider>();

            // Adding material to game objects
            Material newMat = Resources.Load("MessageArrow", typeof(Material)) as Material;
            line.AddComponent<MeshRenderer>();
            line.GetComponent<Renderer>().material = newMat;

            line.transform.localPosition = new Vector3(0.5f, 0, positionZ);

            line.transform.localScale = new Vector3(0.025f, 0.015f, 0.15f);

            line.transform.position = new Vector3(line.transform.position.x, 
                position + 0.1f, line.transform.position.z);

            positionZ += 0.2f;
        }
    }
    
}
