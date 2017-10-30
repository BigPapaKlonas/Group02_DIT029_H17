using UnityEngine;
using System.Collections.Generic;

public class AddParallelLine : MonoBehaviour {

    //public Transform parallelBox;
    public Mesh cube;

	// Use this for initialization
	void Start ()
    {

    }

    public void AddLine(float position, Transform parallelBox)
    {
        Transform papaTransform = parallelBox;
        parallelBox.position = new Vector3(0, 0, 0);

        float positionZ = -0.4f;
        for(int i = 0; i <= 4; i++)
        {
            GameObject line = new GameObject("Line");
            line.transform.SetParent(parallelBox, true);
            
            // Add cube mesh
            line.AddComponent<MeshFilter>().sharedMesh = this.cube;
            line.AddComponent<BoxCollider>();

            // Adding material to game objects
            Material newMat = Resources.Load("MessageArrow", typeof(Material)) as Material;
            line.AddComponent<MeshRenderer>();
            line.GetComponent<Renderer>().material = newMat;

            //parallelBox.localScale = new Vector3(1, 1, 1);
            //parallelBox = papaTransform;

            // Changing line scale
            line.transform.localScale = new Vector3(0.025f, 0.015f, 0.15f);

            // Setting local position for X and Z axis
            line.transform.localPosition = new Vector3(0.5f, 0, positionZ);

            // Changing the global Y position
            line.transform.position = new Vector3(line.transform.position.x, 
                position + 0.1f, line.transform.position.z);

            positionZ += 0.2f;
        }
        parallelBox = papaTransform;
    }
}
