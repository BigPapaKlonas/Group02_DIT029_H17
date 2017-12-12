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
    public string uName;

    private string[] systems;
    private string sendee;
    private string reciever;
    private string message;
    private bool awaitMessage;
    private MqttClient client;


    // Use this for initialization
    void Start() {

        Debug.Log(room);

        GameObject go = GameObject.Find(uName);
        spawner = (SSDSpawner)go.GetComponent(typeof(SSDSpawner));

        systems = new string[1];
        systems[0] = "start";

        // create client instance 
        client = new MqttClient(IPAddress.Parse("13.59.108.164"), 1883, false, null);

        // register to message received 
        client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

        string clientId = Guid.NewGuid().ToString();
        client.Connect(clientId);

        // subscribe to the topic "/home/temperature" with QoS 2 
        client.Subscribe(new string[] { room }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

    }
    void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e) {

        string SMessage = System.Text.Encoding.UTF8.GetString(e.Message);
        Debug.Log("Received: " + System.Text.Encoding.UTF8.GetString(e.Message));
        string[] array = SMessage.Split(' ');
        if ((systems[0] == "start" || Array.IndexOf(systems, array[0]) != -1) && array.Length > 1) {
            Debug.Log(SMessage + "in the if");
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
                spawner.from = sendee;
                spawner.to = reciever;
                spawner.newMessage = true;
                awaitMessage = false;
            } else if (systems[0] == "start") {
                systems = array;
                Debug.Log(systems);
            } else if (array[1] == "finished") {

            }
        }
    }

    private bool compArr<T, S>(T[] arrayA, S[] arrayB) {
        if (arrayA.Length != arrayB.Length) return false;

        for (int i = 0; i < arrayA.Length; i++) {
            if (!arrayA[i].Equals(arrayB[i])) return false;
        }

        return true;
    }
}