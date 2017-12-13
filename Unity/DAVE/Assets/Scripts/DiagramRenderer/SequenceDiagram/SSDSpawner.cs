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
    public bool parAct;
    public bool parBox;
    public bool parMessage;
    public bool parStop;
    
    public int width;
    public int parAmount;
    public string parSystem;
    public string parTo;
    public string parFrom;
    public float parY;
    public string parMsg;

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
    public string normFrom;
    public string normTo;
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

        size = 15;
        
        this.name = room;


        myPos = this.transform.position;

        y = size + myPos.y + 1;

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
            SpawnActivation(message, systemName, y, false);
            newActivation = false;
        } else if (newMessage) {
            SpawnMsg(normTo, normFrom, message, y, false);
            newMessage = false;
        } else if (endAct) {
            GameObject.Find(message + systemName).SendMessage("Stop");
            endAct = false;
        } else if (parMessage) {
            SpawnMsg(parTo, parFrom, parMsg, parY, true);
            parMessage = false;
        } else if (parAct) {
            SpawnActivation(parMsg, parSystem, parY, true);
            parAct = false;
        } else if (parStop) {
            GameObject.Find(parMsg + systemName).SendMessage("Stop");
            parStop = false;
        } else if (parBox) {
            SpawnParalellism(width, parAmount);
            parBox = false;
        }

    }
    private void SpawnActivation(string msg, string system, float actY, bool isPar)
    {

        systemBox = GameObject.Find(system);

        Vector3 positioning = new Vector3(
             systemBox.transform.position.x,
             actY,
             systemBox.transform.position.z
           );

        GameObject activationBoxGO = (GameObject)Instantiate(
          activationBoxPrefab,
          positioning,
          this.transform.rotation
        );

        ProcessAnimation p = activationBoxGO.GetComponent<ProcessAnimation>();
        p.name = msg + systemName;
        p.current = systemBox;
        p.room = room;
        
        if(isPar){
            
        }


    }

    private void SpawnMsg(string to, string from, string msg, float msgY, bool isPar)
    {

        next = GameObject.Find(to);
        current = GameObject.Find(from);

        thisPos = new Vector3(
          current.transform.position.x,
          msgY,
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
          msgY,
          next.transform.position.z
        );
        GameObject emptyGO = (GameObject)Instantiate(emptyTarget, nextPos, this.transform.rotation);

        m.origin = empty.transform;
        m.destination = emptyGO.transform;
        m.room = room;
        m.isPar = isPar;

        m.current = next;

        Quaternion rotationTextMesh = Quaternion.Euler(0, -90, 0);
        GameObject messageTextGO = (GameObject)Instantiate(messageText, thisPos, rotationTextMesh);
        MessageText mT = messageTextGO.GetComponent<MessageText>();
        mT.target = emptyGO.transform.position;
        mT.origin = empty.transform.position;
        mT.method = msg;
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
        Vector3 position = new Vector3(myPos.x, y - 8, myPos.z + zSize + 1);
        Debug.Log(position);
        GameObject parBox = (GameObject)Instantiate(
          parBoxPrefab,
          position,
          this.transform.rotation
        );

        parBox.transform.localScale += new Vector3(0, (y - 1)/2, (zSize * 3));
        RenderParallelBox line = parBox.GetComponent<RenderParallelBox>();
        line.cube = parBox.GetComponent<MeshFilter>().mesh;
        int positionY = 10;
        while(parCount > 0) 
        {
            positionY += 5;
            line.AddLine(positionY, parBox.transform, zSize);
            parCount--;
        }
    }
}
