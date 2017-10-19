using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master : MonoBehaviour {

  public GameObject activationBoxPrefab;
  public string[] destinationTags;
  public Queue destList;

  private bool sent;

  void Start () {

    sent = false;
  }

  void Update () {

    if(GameObject.FindWithTag("SystemBox") != null && sent == false){
      NewMessage();
      StartMessageChain();
      sent = true;
    }

  }

  void NewMessage () {
    destList = new Queue();

    foreach (string tag in destinationTags) {
      GameObject tmp = GameObject.Find(tag);
      destList.Enqueue(tmp);
    }
  }

  void StartMessageChain() {

    if(destList.Count > 0){

    GameObject first = (GameObject) destList.Dequeue();

    Vector3 positioning = new Vector3(
      first.transform.position.x,
      first.transform.position.y - 1,
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
