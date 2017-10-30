using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMessages : MonoBehaviour {

  public GameObject activationBoxPrefab;
  public static Queue<float> actSizeList;
  public static Queue<string> messageNameList;
  float length;

  public void NewMessage (JSONSequence json) {
    actSizeList = new Queue<float>();
    messageNameList = new Queue<string>();
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

          string MessageString = "";
          foreach(var msg in names.Message)
            MessageString += msg + ", ";

          MessageString = MessageString.Remove(MessageString.Length - 1);
          messageNameList.Enqueue(MessageString);

          Debug.Log("here");
          if (actSizeList.Count < length - 1) {
            Debug.Log("there");
                float rand = Random.Range(0.5f, 1.5f);
                actSizeList.Enqueue(rand);
                Debug.Log("rand" + rand);
            } else {
                
                actSizeList.Enqueue(Sum(actSizeList));
                Debug.Log("rand");
            }
        }
        
        StartMessageChain(destList, offset);
        //offset += json.Diagram.Content.Count + 0.5f;

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
            string MessageString = "";
          foreach(var msg in names.Message)
            MessageString += msg + ", ";
            MessageString = MessageString.Remove(MessageString.Length - 2);
            messageNameList.Enqueue(MessageString);
          messageNameList.Enqueue(MessageString);
            //if (actSizeList.Count < json.Diagram.Content.Count -1) {
            //Debug.Log("there");
            float rand = Random.Range(0.5f, 1.5f);
            actSizeList.Enqueue(rand);
            Debug.Log("rand" + rand);
            rand = Random.Range(0.5f, 1.5f);
            actSizeList.Enqueue(rand);
            //} else {
                
              //  actSizeList.Enqueue(Sum(actSizeList));
                
            //s}
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
      first.transform.position.y - 0.5f,
      first.transform.position.z
    );

    GameObject activationBoxGO = (GameObject)Instantiate(
      activationBoxPrefab,
      positioning,
      this.transform.rotation
    );

    ProcessAnimation p = activationBoxGO.GetComponent<ProcessAnimation>();
    p.destList = queue;
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
