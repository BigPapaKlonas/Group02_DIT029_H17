using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindDeploymentConnections : MonoBehaviour
{

    public struct MessageData
    {
        public string to;
        public string from;
        public string message;
    }


    public static Queue<MessageData> messageDataList;
    MessageData messageData;
    float length;
    float contentCount;
    float size;

    public void NewMessage(JSONSequence json, float offSet)
    {
        messageDataList = new Queue<MessageData>();
        messageData = new MessageData();


            foreach (var content in json.Diagram.Content)
            {
                foreach (var names in content.SubContent)
                {
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
                MessageData[] tmpData = new MessageData[messageDataList.Count];
                messageDataList.CopyTo(tmpData, 0);
                GetComponent<RenderConnections>().CreateConnections(new ArrayList(tmpData), offSet);
            
        }
    }
}
