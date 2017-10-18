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
        Debug.Log("MessageRecieved: " + destList.Count);

          Debug.Log("current: " + current.transform.position.y);

          Vector3 currentPos = new Vector3(
            current.transform.position.x,
            destination.position.y,
            current.transform.position.z
          );

          GameObject activationBoxGO = (GameObject)Instantiate(
            activationBoxPrefab,
            currentPos,
            this.transform.rotation
          );

          Debug.Log("activationBoxGO: " + currentPos);

          ProcessAnimation p = activationBoxGO.GetComponent<ProcessAnimation>();
          p.destList = destList;


        // ToDo: p.nextDest = nextnextd;
        //Vector3 v = new Vector3(dest.transform.position.x, this.transform.position.y, dest.transform.position.z);
        //GameObject emptyGO = (GameObject)Instantiate(emptyTarget, v, this.transform.rotation);

        //m.origin = this.transform;

        //m.destination = emptyGO.transform;
    }
}
