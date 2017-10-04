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
    private bool sent;

    public GameObject activationBoxPrefab;
    public GameObject nextd;
    public GameObject nextnextd;
    private Vector3 v;

    // Use this for initialization
    void Start () {

        messageLine = GetComponent<LineRenderer>();
        messageLine.SetPosition(0, origin.position);
        messageLine.SetWidth(.1f, .1f);
        dist = Vector3.Distance(origin.position, destination.position);

        sent = false;

        v = new Vector3(destination.position.x, destination.position.y, destination.position.z);

    }
	
	// Update is called once per frame
	void Update () {
		
        if(counter < dist) {

            counter += .1f / speed;
            
            
            Vector3 distThisFrame = Vector3.Lerp(origin.position, destination.position, counter); 
            messageLine.SetPosition(1, distThisFrame);
           
        }else if(sent == false) {
            sent = true;
            MessageRecieved();
        }
	}

    void MessageRecieved()
    {
        //Vector3 v = new Vector3(destination.position.x, destination.position.y, destination.position.z);
        GameObject activationBoxGO = (GameObject)Instantiate(activationBoxPrefab, v, this.transform.rotation);
        ProcessAnimation p = activationBoxGO.GetComponent<ProcessAnimation>();
        p.dest = nextd;
        p.nextDest = nextnextd;
        //Vector3 v = new Vector3(dest.transform.position.x, this.transform.position.y, dest.transform.position.z);
        //GameObject emptyGO = (GameObject)Instantiate(emptyTarget, v, this.transform.rotation);

        //m.origin = this.transform;

        //m.destination = emptyGO.transform;
    }
}
