using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMessage : MonoBehaviour {

    public GameObject messagePrefab;
    public GameObject emptyTarget;
    public GameObject messageText;

    public bool newSpawn;
    public string from;
    public string to;
    public string message;


    private Vector3 thisPos;
    private Vector3 nextPos;
    private float y;
    private GameObject next;
    private GameObject current;

    void Start() {
        newSpawn = false;
        y = 9;
    }
    void Update() {
        if (newSpawn) {
            SpawnMsg();
            newSpawn = false;
        }
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

        m.current = next;

        GameObject messageTextGO = (GameObject)Instantiate(messageText, thisPos, this.transform.rotation);
        MessageText mT = messageTextGO.GetComponent<MessageText>();
        mT.target = emptyGO.transform.position;
        mT.origin = empty.transform.position;
        mT.method = message;


        y = y - 3f;
    }

}
