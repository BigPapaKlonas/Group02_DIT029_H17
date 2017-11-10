using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master : MonoBehaviour {

    public GameObject activationBoxPrefab;
    private Vector3 origoOfSystemBox;
    public string[] destinationTags;
    public Queue destList;
    public string systemBoxTag;
    public Queue<float> actSizeList;
    float length;
    

    void Start() {
        NewMessage();
        StartMessageChain();
        Debug.Log(actSizeList.Count);
    }

    void Update() {


    }

    void NewMessage() {
        destList = new Queue();
        length = destinationTags.Length;
        actSizeList = new Queue<float>();
        foreach (string tag in destinationTags) {
            GameObject tmp = GameObject.FindWithTag(tag);
            destList.Enqueue(tmp);

            if (actSizeList.Count < length - 1) {
                float rand = Random.Range(0.5f, 1.5f);
                actSizeList.Enqueue(rand);
                Debug.Log("rand" + rand);
            } else {
                
                actSizeList.Enqueue(Sum(actSizeList));
                
            }
        }
        
    }

    void StartMessageChain() {

        if (destList.Count > 0) {

            GameObject first = (GameObject)destList.Dequeue();
            Vector3 positioning = new Vector3(
              first.transform.position.x,
              first.transform.position.y - 0.5f,
              first.transform.position.z
            );

            GameObject activationBoxGO = (GameObject)Instantiate(
              activationBoxPrefab,
              positioning,
              this.transform.rotation
            );

            ProcessAnimation p = activationBoxGO.GetComponent<ProcessAnimation>();
            p.destList = destList;
            p.current = first;
            p.endSize = actSizeList.Dequeue();

        }
    }

    private float Sum(Queue<float> list) {
        float sum = 0;
        foreach(float a in list) {
            sum = sum + a;
        }

        float lastFloat = length - sum;
        return lastFloat;
    }
}
