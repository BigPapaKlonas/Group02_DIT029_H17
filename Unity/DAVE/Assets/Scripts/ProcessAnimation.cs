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
    public GameObject dest;
    public GameObject nextDest;
    private bool sent;
    Vector3 v;

    public float speed = 0.01f;
    float maxScale = 7f;
    float counter;
    Transform d;
    private Renderer rend;
    
    // Use this for initialization
    void Start () {
        vector3 = transform.position;
        originalScale = transform.localScale.y;
        endScale = originalScale;
        counter = 0;
        sent = false;
        rend = GetComponent<Renderer>();
        
        
	}
	
	// Update is called once per frame
	void Update () {
        //scl = scl + growFactor;
        //if (scl < 5){
        //    Vector3 temp = transform.localScale;

        //    //We change the values for this saved variable (not actual transform scale)

        //    temp.y = temp.y + growFactor;


        //    //We assign temp variable back to transform scale
        //    transform.localScale = temp;
        //}
        if (counter < 150) {
            counter++;
            place = transform.localScale;
            place.y = Mathf.MoveTowards(transform.localScale.y, endScale, Time.deltaTime * speed);
            transform.localScale = place;
            transform.position = vector3 - transform.up * (transform.localScale.y / 2 + originalScale / 2);
            endScale = maxScale;
        }
        else if(sent == false) {
            sent = true;
            SendMessage();
            
        }
	}

    void Grow(){

        transform.localScale += new Vector3(0, 0.1f, 0);

    }
    void SendMessage() {
        float y = this.transform.position.y - 0.3f;
        v = new Vector3(this.transform.position.x, y, this.transform.position.z);
        GameObject empty = (GameObject)Instantiate(emptyTarget, v, this.transform.rotation);
        GameObject messageGO = (GameObject)Instantiate(messagePrefab, this.transform.position, this.transform.rotation);
        MessageAnimation m = messageGO.GetComponent<MessageAnimation>();
        v = new Vector3(dest.transform.position.x, y, dest.transform.position.z);
        GameObject emptyGO = (GameObject)Instantiate(emptyTarget, v, this.transform.rotation);

        m.origin = empty.transform;
        
        m.destination = emptyGO.transform;
        m.nextd = nextDest;
        m.nextnextd = this.gameObject;

    }
    float BottomBox() {
        
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        float lowest = Mathf.Infinity;
        int i = 0;
        while (i < vertices.Length) {
            if (vertices[i].y < lowest) lowest = vertices[i].y;
            i++;
        }

        //Vector3 bottomBox = new Vector3(this.transform.position.x, lowest, this.transform.position.z);
        return lowest;

    }
}
