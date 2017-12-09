using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSDSpawner : MonoBehaviour {

    public string room;
    public bool endAct;
    public float y;

    public GameObject ssdControllerPrefab;
    private Vector3 myPos;

    //Systemboxes
    public GameObject systemBoxPrefab;
    public bool newSystem;
    public string systemBoxName;
    public int size;

    private int p = 0;
    //Messages
    public GameObject messagePrefab;
    public GameObject emptyTarget;
    public GameObject messageText;

    public bool newMessage;
    public string from;
    public string to;
    public string message;


    private Vector3 thisPos;
    private Vector3 nextPos;
    private GameObject next;
    private GameObject current;

    //Activationboxes
    public GameObject activationBoxPrefab;
    public bool newActivation;
    public bool stop;
    public GameObject systemBox;
    public string systemName;

    void Start () {

        size = 15;

        this.name = room;


        myPos = this.transform.position;

        y = size + myPos.y - 1f;

        GameObject ssdControllerGO = (GameObject)Instantiate(
          ssdControllerPrefab,
          this.transform.position,
          this.transform.rotation
        );

        SSDController ssdController = ssdControllerGO.GetComponent<SSDController>();
        ssdController.room = room;

    }
	
	// Update is called once per frame
	void Update () {

        if (newSystem) {
            SpawnSystem();
            newSystem = false;
        } else if (newActivation) {
            SpawnActivation();
            newActivation = false;
        } else if (newMessage) {
            SpawnMsg();
            newMessage = false;
        } else if (endAct) {
            GameObject.Find(message + systemName).SendMessage("Stop");
            endAct = false;
        }

    }

    private void SpawnActivation() {

        systemBox = GameObject.Find(systemName);

        Vector3 positioning = new Vector3(
             systemBox.transform.position.x,
             y,
             systemBox.transform.position.z
           );

        GameObject activationBoxGO = (GameObject)Instantiate(
          activationBoxPrefab,
          positioning,
          this.transform.rotation
        );

        ProcessAnimation p = activationBoxGO.GetComponent<ProcessAnimation>();
        p.name = message + systemName;
        p.current = systemBox;
        p.room = room;

    }

    private void SpawnMsg() {

        next = GameObject.Find(to);
        current = GameObject.Find(from);

        thisPos = new Vector3(
          current.transform.position.x,
          y,
          current.transform.position.z
        );
        GameObject empty = (GameObject)Instantiate(
          emptyTarget,
          thisPos,
          current.transform.rotation
        );
        GameObject messageGO = (GameObject)Instantiate(
          messagePrefab,
          current.transform.position,
          current.transform.rotation
        );
        MessageAnimation m = messageGO.GetComponent<MessageAnimation>();
        nextPos = new Vector3(
          next.transform.position.x,
          y,
          next.transform.position.z
        );
        GameObject emptyGO = (GameObject)Instantiate(emptyTarget, nextPos, this.transform.rotation);

        m.origin = empty.transform;
        m.destination = emptyGO.transform;
        m.room = room;

        m.current = next;

        GameObject messageTextGO = (GameObject)Instantiate(messageText, thisPos, this.transform.rotation);
        MessageText mT = messageTextGO.GetComponent<MessageText>();
        mT.target = emptyGO.transform.position;
        mT.origin = empty.transform.position;
        mT.method = message;


        
    }

    private void SpawnSystem() {


        Vector3 position = new Vector3(myPos.x, y, myPos.z + p);
        GameObject box = (GameObject)Instantiate(
          systemBoxPrefab,
          position,
          this.transform.rotation
        );
        box.transform.parent = transform;
        box.name = systemBoxName;
        // Changes the Z position
        p = p + 3;

        box.GetComponentInChildren<TextMesh>().text = systemBoxName;


    }
}
