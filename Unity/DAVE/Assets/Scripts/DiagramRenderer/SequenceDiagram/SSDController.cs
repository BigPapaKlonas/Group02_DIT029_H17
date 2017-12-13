using UnityEngine;
using System.Collections;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Utility;
using uPLibrary.Networking.M2Mqtt.Exceptions;

using System;

public class SSDController : MonoBehaviour {

    public SSDSpawner spawner;
    

    public string room;

    private string[] systems;
    private string sendee;
    private string reciever;
    private string message;
    private bool awaitMessage;

    private string sendeePar;
    private string recieverPar;
    private string messagePar;
    private bool awaitMessagePar;

    private MqttClient client;


    // Use this for initialization
    void Start() {

        Debug.Log(room);

        GameObject go = GameObject.Find(room);
        spawner = (SSDSpawner)go.GetComponent(typeof(SSDSpawner));


        // create client instance 
        client = new MqttClient(IPAddress.Parse("13.59.108.164"), 1883, false, null);

        // register to message received 
        client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

        string clientId = Guid.NewGuid().ToString();
        client.Connect(clientId);

        client.Subscribe(new string[] { room }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

    }
    void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e) {

        string SMessage = System.Text.Encoding.UTF8.GetString(e.Message);
        Debug.Log("Received: " + System.Text.Encoding.UTF8.GetString(e.Message));
        string[] array = SMessage.Split(' ');
            
        if (array[1] == "initial") {
            spawner.systemBoxName = array[0];
            spawner.newSystem = true;
        } else if (array[1] == "preparemessage") {
            spawner.systemName = array[0];
            spawner.message = array[3];
            spawner.newActivation = true;

        } else if (array[1] == "idle") {
            spawner.systemName = array[0];
            spawner.stop = true;
        } else if (array[1] == "sentmessage") {
            spawner.endAct = true;
            awaitMessage = true;
            sendee = array[0];
            reciever = array[2];
            message = array[3];
        } else if (array[1] == "recievedmessage" && awaitMessage) {
            spawner.message = message;
            spawner.normFrom = sendee;
            spawner.normTo = reciever;
            spawner.newMessage = true;
            awaitMessage = false;
        } else if (array[1] == "finished") {

        } else if (array[1] == "nopar") {
            spawner.y = float.Parse(array[3]) * 2 + 50;
            spawner.parAmount = int.Parse(array[2]);
            spawner.width = int.Parse(array[1]);
            spawner.parBox = false;
        } else if (array[0] == "par") {
            spawner.y = float.Parse(array[3]) + 12;
            spawner.parAmount = int.Parse(array[2]);
            spawner.width = int.Parse(array[1]);
            spawner.parBox = true;
        } else if (array[1] == "parpreparemessage") {
            spawner.parSystem = array[0];
            spawner.parMsg = array[3];
            spawner.parAct = true;
        } else if (array[1] == "parsentmessage") {
            spawner.parStop = true;
            awaitMessagePar = true;
            sendeePar = array[0];
            recieverPar = array[2];
            messagePar = array[3];
        } else if (array[1] == "parrecievedmessage" && awaitMessage) {
            spawner.parMsg = messagePar;
            spawner.parFrom = sendeePar;
            spawner.parTo = recieverPar;
            spawner.parMessage = true;
            awaitMessagePar = false;
        }
    }

}