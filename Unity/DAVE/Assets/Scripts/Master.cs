using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master : MonoBehaviour {

  public GameObject activationBoxPrefab;
  private Vector3 origoOfSystemBox;
  public string[] destinationTags;
  public Queue destList;
  public string systemBoxTag;

  void Start () {
    NewMessage();
    StartMessageChain();
  }

  void Update () {

    
  }

  void NewMessage () {
    destList = new Queue();

    foreach (string tag in destinationTags) {
      GameObject tmp = GameObject.FindWithTag(tag);
      destList.Enqueue(tmp);
    }
  }

  void StartMessageChain() {

    if(destList.Count > 0){

    GameObject first = (GameObject)destList.Dequeue();
    Vector3 positioning = new Vector3(
      first.transform.position.x,
      first.transform.position.y + 0.5f,
      first.transform.position.z
    );

    GameObject activationBoxGO = (GameObject)Instantiate(
      activationBoxPrefab,
      positioning,
      this.transform.rotation
    );

    ProcessAnimation p = activationBoxGO.GetComponent<ProcessAnimation>();
    p.destList = destList;

    }
  }

}
