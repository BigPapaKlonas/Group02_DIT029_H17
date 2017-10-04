using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageAnimation : MonoBehaviour {

    private LineRenderer messageLine;
    private float counter;
    private float dist;
    public Transform origin;
    public Transform destination;
    public float speed  = 50f;

    public TextMesh textMesh;

    // Use this for initialization
    void Start () {

        messageLine = GetComponent<LineRenderer>();
        messageLine.SetPosition(0, origin.position);
        messageLine.SetWidth(.1f, .1f);
        dist = Vector3.Distance(origin.position, destination.position);

        textMesh = messageLine.GetComponent(typeof(TextMesh)) as TextMesh;
        textMesh.text = "TEST";

    }
	
	// Update is called once per frame
	void Update () {
		
        if(counter < dist) {

            counter += .1f / speed;
            
            
            Vector3 distThisFrame = Vector3.Lerp(origin.position, destination.position, counter); 
            messageLine.SetPosition(1, distThisFrame);
            textMesh.transform.position = distThisFrame;
        }
	}
}
