using System.Collections;
using UnityEngine;

public class StartMessages : MonoBehaviour {

  public GameObject activationBoxPrefab;

  public void NewMessage (JSONSequence json) {

    // Check for parallelism:
    if(json.Diagram.Content.Count > 1){

      float offset = 0;

      foreach (var content in json.Diagram.Content) {
        Queue destList = new Queue();
        foreach (var names in content.SubContent){
          GameObject tmpFrom = GameObject.Find(names.From);
          GameObject tmpTo = GameObject.Find(names.To);
          destList.Enqueue(tmpFrom);
          destList.Enqueue(tmpTo);
        }
        StartMessageChain(destList, offset);
        offset += json.Diagram.Content.Count + 0.5f;

      }
      // No parallelism:
    } else {
        foreach (var content in json.Diagram.Content) {
          Queue destList = new Queue();
          foreach (var names in content.SubContent){
            GameObject tmpFrom = GameObject.Find(names.From);
            GameObject tmpTo = GameObject.Find(names.To);
            destList.Enqueue(tmpFrom);
            destList.Enqueue(tmpTo);
          }
          StartMessageChain(destList, 0f);
        }
    }

    //GameObject parBox = (GameObject)Instantiate(Resources.Load("ParallelBox"));
  }

  void StartMessageChain(Queue queue, float yOffset) {

    if(queue.Count > 0){

    GameObject first = (GameObject) queue.Dequeue();

    Vector3 positioning = new Vector3(
      first.transform.position.x,
      first.transform.position.y - 1 - yOffset,
      first.transform.position.z
    );

    GameObject activationBoxGO = (GameObject)Instantiate(
      activationBoxPrefab,
      positioning,
      this.transform.rotation
    );

    ProcessAnimation p = activationBoxGO.GetComponent<ProcessAnimation>();
    p.destList = queue;

    }
  }

}
