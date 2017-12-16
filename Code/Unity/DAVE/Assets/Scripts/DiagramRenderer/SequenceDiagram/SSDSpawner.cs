using System;
using UnityEngine;

public class SSDSpawner : MonoBehaviour
{

    public string room;
    public bool endAct;
    public float y;

    public GameObject ssdControllerPrefab;
    private Vector3 myPos;

    //Parelellism
    public GameObject parBoxPrefab;

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

    void Start()
    {

        
        
        this.name = room;

        y = 40;

        myPos = this.transform.position;


        GameObject ssdControllerGO = (GameObject)Instantiate(
          ssdControllerPrefab,
          this.transform.position,
          this.transform.rotation
        );

        SSDController ssdController = ssdControllerGO.GetComponent<SSDController>();
        ssdController.room = room;


    }

    // Update is called once per frame
    void Update() {

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
    private void SpawnActivation()
    {

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

    private void SpawnMsg()
    {

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

        Quaternion rotationTextMesh = Quaternion.Euler(0, -90, 0);
        GameObject messageTextGO = (GameObject)Instantiate(messageText, thisPos, rotationTextMesh);
        MessageText mT = messageTextGO.GetComponent<MessageText>();
        mT.target = emptyGO.transform.position;
        mT.origin = empty.transform.position;
        mT.method = message;
        mT.to = to;
        mT.from = from;



    }

    private void SpawnSystem()
    {
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

        box.GetComponentInChildren<TextMesh>().text = systemBoxName.Split(':')[0];
    }

    private void SpawnParalellism(int zSize, int parCount) 
    {
        Vector3 position = new Vector3(myPos.x, y - 1, myPos.z);
        GameObject parBox = (GameObject)Instantiate(
          parBoxPrefab,
          position,
          this.transform.rotation
        );

        parBox.transform.localScale += new Vector3(0, y - 1, zSize);
        RenderParallelBox line = parBox.GetComponent<RenderParallelBox>();
        line.cube = parBox.GetComponent<MeshFilter>().mesh;
        int positionY = 0;
        while(parCount > 0) 
        {
            positionY += 5;
            line.AddLine(positionY, parBox.transform, zSize);
            parCount--;
        }
    }
}
