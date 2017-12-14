using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMessages : MonoBehaviour
{

    public struct MessageData
    {
        public string to;
        public string from;
        public string message;
    }

    public GameObject activationBoxPrefab;
    public static Queue<float> actSizeList;
    public static Queue<MessageData> messageDataList;
    MessageData messageData;
    float length;
    float contentCount;
    float size;

    public void NewMessage(JSONSequence json)
    {
        actSizeList = new Queue<float>();
        messageDataList = new Queue<MessageData>();
        messageData = new MessageData();

        // Check for parallelism:
        if (json.Diagram.Content.Count > 1)
        {

            float offset = 0;

            contentCount = json.Diagram.Content.Count - 1;

            // Add parallel box
            GameObject parBox = (GameObject)Instantiate(Resources.Load("ParallelBox"));

            foreach (var content in json.Diagram.Content)
            {
                Queue destList = new Queue();
                Queue destListPar = new Queue();

                foreach (var names in content.SubContent)
                {
                    if (content.SubContent.Count > 3)
                    {
                        Debug.Log("less than 4");
                        GameObject tmpFrom = GameObject.Find(names.From);
                        GameObject tmpTo = GameObject.Find(names.To);
                        destList.Enqueue(tmpFrom);
                        destList.Enqueue(tmpTo);

                        string MessageString = "";
                        foreach (var msg in names.Message)
                            MessageString += msg + ", ";
                        MessageString = MessageString.Remove(MessageString.Length - 2);

                        messageData.to = names.To;
                        messageData.from = names.From;
                        messageData.message = MessageString;

                        messageDataList.Enqueue(messageData);
                        messageDataList.Enqueue(messageData);
                        size++;
                    }

                    else
                    {


                        GameObject tmpFrom = GameObject.Find(names.From);
                        GameObject tmpTo = GameObject.Find(names.To);
                        destListPar.Enqueue(tmpFrom);
                        destListPar.Enqueue(tmpTo);

                        string MessageString = "";
                        foreach (var msg in names.Message)
                            MessageString += msg + ", ";
                        MessageString = MessageString.Remove(MessageString.Length - 2);
                        messageData.to = names.To;
                        messageData.from = names.From;
                        messageData.message = MessageString;

                        messageDataList.Enqueue(messageData);
                        messageDataList.Enqueue(messageData);
                    }


                    float rand = Random.Range(0.5f, 1.5f);
                    actSizeList.Enqueue(rand);
                    rand = Random.Range(0.5f, 1.5f);
                    actSizeList.Enqueue(rand);
                }
                MessageData[] tmpData = new MessageData[messageDataList.Count];
                messageDataList.CopyTo(tmpData, 0);
                Debug.Log("I run parallelism \n");
                //GetComponent<RenderConnections>().CreateConnections(new ArrayList(tmpData));
                StartMessageChain(destList, offset);
                StartParMessageChain(destListPar, offset);
                //offset += json.Diagram.Content.Count + 0.5f;
                PlaceParLine(json, -(size / 2), parBox);

            }
            // No parallelism:
        }
        else
        {
            foreach (var content in json.Diagram.Content)
            {
                Queue destList = new Queue();
                foreach (var names in content.SubContent)
                {
                    GameObject tmpFrom = GameObject.Find(names.From);
                    GameObject tmpTo = GameObject.Find(names.To);
                    destList.Enqueue(tmpFrom);
                    destList.Enqueue(tmpTo);
                    string MessageString = "";
                    foreach (var msg in names.Message)
                        MessageString += msg + ", ";
                    MessageString = MessageString.Remove(MessageString.Length - 2);
                    messageData.to = names.To;
                    messageData.from = names.From;
                    messageData.message = MessageString;

                    messageDataList.Enqueue(messageData);
                    messageDataList.Enqueue(messageData);
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
                MessageData[] tmpData = new MessageData[messageDataList.Count];
                messageDataList.CopyTo(tmpData, 0);
                ////GetComponent<RenderConnections>().CreateConnections(new ArrayList(tmpData));
                StartMessageChain(destList, 0f);
            }
        }
    }


    void StartMessageChain(Queue queue, float yOffset)
    {

        if (queue.Count > 0)
        {

            GameObject first = (GameObject)queue.Dequeue();

            Vector3 positioning = new Vector3(
              first.transform.position.x,
              first.transform.position.y - 1f,
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
            p.endSize = Sum(actSizeList);

        }
    }

    void StartParMessageChain(Queue queue, float yOffset)
    {

        if (queue.Count > 0)
        {

            GameObject first = (GameObject)queue.Dequeue();

            Vector3 positioning = new Vector3(
              first.transform.position.x,
              first.transform.position.y - (size * 2) - 1.5f,
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
            p.endSize = Sum(actSizeList);

        }
    }

    private float Sum(Queue<float> list)
    {
        float sum = 0;
        foreach (float a in list)
        {
            sum = sum + a;
        }
        if (sum > list.Count)
        {
            return actSizeList.Dequeue() / 2;
        }
        else
        {
            return actSizeList.Dequeue();
        }
    }

    void PlaceParLine(JSONSequence json, float positionY,
    GameObject parBox)
    {
        if (contentCount <= 0)
        {
            return;
        }

        if (json.Diagram.Node.Equals("par"))
        {
            RenderParallelBox line = parBox.GetComponent<RenderParallelBox>();
            line.cube = parBox.GetComponent<MeshFilter>().mesh;

            line.AddLine(positionY, parBox.transform, 11);
            contentCount--;
        }
    }
}
