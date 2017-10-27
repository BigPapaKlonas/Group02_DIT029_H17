using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessAnimation : MonoBehaviour {
    Vector3 vector3;
    float originalScale;
    float endScale;
    Vector3 place;
    public GameObject messagePrefab;
    public GameObject emptyTarget;
    public Queue destList;
    public GameObject current;
    private bool sent;
    Vector3 nextPos;
    public GameObject messageText;

    public float speed = 0.01f;
    float maxScale = 7f;
    float counter;
    Transform d;
    int endInt;
    
    void Start () {

      
        vector3 = transform.position;
        originalScale = transform.localScale.y;
        endScale = originalScale;
        counter = 0;
        sent = false;
        endInt = Random.Range(50, 150);
     
	}

	
	void Update () {
        
        if (counter < endInt) {
            current.GetComponent<SystemBox>().lightSwitch = true;
            counter++;
            place = transform.localScale;
            place.y = Mathf.MoveTowards(transform.localScale.y, endScale, Time.deltaTime * speed);
            transform.localScale = place;
            transform.position = vector3 - transform.up * (transform.localScale.y / 2 + originalScale / 2);
            endScale = maxScale;
            
        }
        else if(sent == false) {
            current.GetComponent<SystemBox>().lightSwitch = false;
            sent = true;
            SendMessage();

        }
	}

    
    void SendMessage() {
      
      if(destList.Count > 0){
        GameObject next = (GameObject)destList.Dequeue();
       
        float y = transform.position.y - transform.localScale.y / 2;
        y = y + 0.1f;
        nextPos = new Vector3(
          this.transform.position.x,
          y,
          this.transform.position.z
        );
        GameObject empty = (GameObject)Instantiate(
          emptyTarget,
          nextPos,
          this.transform.rotation
        );
        GameObject messageGO = (GameObject)Instantiate(
          messagePrefab,
          this.transform.position,
          this.transform.rotation
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
        m.destList = destList;

        GameObject messageTextGO = (GameObject)Instantiate(messageText, this.transform.position, this.transform.rotation);
        MessageText mT = messageTextGO.GetComponent<MessageText>();
        mT.target = emptyGO.transform;
        mT.method = "placeholder";
      }
    }
    
}
