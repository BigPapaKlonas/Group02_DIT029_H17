using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeploymentAnimation : MonoBehaviour {

	// Use this for initialization
	void Start () {

        drawConnections();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void drawConnections()
    {
        GameObject from = GameObject.Find("artu1");
        GameObject to = GameObject.Find("artg");

        // Draw the line
        float startY, endY, startZ, endZ;
        float deltaY = from.transform.position.y - to.transform.position.y;
        float deltaZ = from.transform.position.z - to.transform.position.z;
        if (deltaY < 0)
        {
            GameObject tmp = to;
            to = from;
            from = to;
        }
           


    }

}
