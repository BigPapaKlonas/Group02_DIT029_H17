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
        float positionZBack = -0.4f;
        for (int i = 0; i <= 11; i++)
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

            if (i < 5)
            {
                // Setting local position for X and Z axis
                line.transform.localPosition = new Vector3(-0.5f, 0, positionZBack);
                positionZBack += 0.2f;
            }
            else
            {
                // Setting local position for X and Z axis
                line.transform.localPosition = new Vector3(0.5f, 0, positionZ);
                positionZ += 0.2f;
            }

            if (i == 10)
            {
                line.transform.localScale = new Vector3(0.025f, 0.015f, 0.6f);
                line.transform.localRotation = Quaternion.Euler(0, 90, 0);
                line.transform.localPosition = new Vector3(0, 0, 0.5f);
            }
            else if (i == 11)
            {
                line.transform.localScale = new Vector3(0.025f, 0.015f, 0.6f);
                line.transform.localRotation = Quaternion.Euler(0, 90, 0);
                line.transform.localPosition = new Vector3(0, 0, -0.5f);
            }
            else
            {
                // Changing line scale
                line.transform.localScale = new Vector3(0.025f, 0.015f, 0.15f);
            }

            // Changing the global Y position
            line.transform.position = new Vector3(line.transform.position.x, 
                position + 0.1f, line.transform.position.z);
        }
        parallelBox = papaTransform;
    }
}
