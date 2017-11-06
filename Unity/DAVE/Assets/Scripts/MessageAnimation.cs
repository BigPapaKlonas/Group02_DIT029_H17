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
    public Queue destList;
    public GameObject current;
    public GameObject currentEmpty;
    private Vector3 v;
    public Vector3 currentPos;
    public Vector3 distThisFrame;
    GameObject arrowhead;
    public bool left = false;


    // Use this for initialization
    void Start () {

        arrowhead = this.transform.Find("Arrowhead").gameObject;
        messageLine = GetComponent<LineRenderer>();
        messageLine.SetPosition(0, origin.position);
        messageLine.SetPosition(1, origin.position);
        //TODO add end pos to avoid weird bug
        messageLine.SetWidth(.1f, .1f);
        
        dist = Vector3.Distance(origin.position, destination.position);

        sent = false;

        v = new Vector3(destination.position.x, destination.position.y, destination.position.z);

        currentPos = new Vector3(
        current.transform.position.x,
        destination.position.y + 0.1f,
        current.transform.position.z
        );
        if(origin.position.z > v.z) {
            
            left = true;

        }

    }

	// Update is called once per frame
	void Update () {
        
        if (counter < 1) {
            
            counter += .1f / speed;


            distThisFrame = Vector3.Lerp(origin.position, destination.position, counter);
            messageLine.SetPosition(1, distThisFrame);
            //Debug.Log(left);
            arrowhead.GetComponent<Arrowhead>().changePos(left, distThisFrame);
           

        }else if(sent == false) {

            MessageRecieved();
            sent = true;
            
        }
	}

    void MessageRecieved() {
        

        GameObject activationBoxGO = (GameObject)Instantiate(
        activationBoxPrefab,
        currentPos,
        this.transform.rotation
        );

        ProcessAnimation p = activationBoxGO.GetComponent<ProcessAnimation>();
        p.destList = destList;
        p.current = current;
        p.endSize = StartMessages.actSizeList.Dequeue();
        


        // ToDo: p.nextDest = nextnextd;
        //Vector3 v = new Vector3(dest.transform.position.x, this.transform.position.y, dest.transform.position.z);
        //GameObject emptyGO = (GameObject)Instantiate(emptyTarget, v, this.transform.rotation);

        //m.origin = this.transform;

        //m.destination = emptyGO.transform;
        
    }
}
