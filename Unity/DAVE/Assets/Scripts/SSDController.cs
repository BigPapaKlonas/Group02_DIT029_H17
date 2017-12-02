using UnityEngine;
using System.Collections;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Utility;
using uPLibrary.Networking.M2Mqtt.Exceptions;

using System;

public class SSDController : MonoBehaviour {

    public SpawnSystemBox systemSpawner;
    public SpawnActivationBox activationSpawner;
    public SpawnMessage messageSpawner;

    public string room;

    private string sendee;
    private string reciever;
    private string message;
    private bool awaitMessage;
    private MqttClient client;
    // Use this for initialization
    void Start() {
        // create client instance 
        client = new MqttClient(IPAddress.Parse("13.59.108.164"), 1883, false, null);

        // register to message received 
        client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

        string clientId = Guid.NewGuid().ToString();
        client.Connect(clientId);

        // subscribe to the topic "/home/temperature" with QoS 2 
        client.Subscribe(new string[] { "root/shaun/diagram" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

    }
    void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e) {

        Debug.Log("Received: " + System.Text.Encoding.UTF8.GetString(e.Message));
        string SMessage = System.Text.Encoding.UTF8.GetString(e.Message);
        Debug.Log("Received: " + SMessage);
        string[] array = SMessage.Split(' ');
        if (array[1] == "initial") {
            systemSpawner.systemBoxName = array[0];
            systemSpawner.newSpawn = true;
        } else if (array[1] == "preparemessage") {
            activationSpawner.systemName = array[0];
            activationSpawner.newSpawn = true;
        } else if (array[1] == "idle") {
            activationSpawner.systemName = array[0];
            activationSpawner.stop = true;
        } else if (array[1] == "sentmessage") {
            awaitMessage = true;
            sendee = array[0];
            reciever = array[2];
            message = array[3];
        } else if (array[1] == "recievedmessage" && awaitMessage) {
            messageSpawner.message = message;
            messageSpawner.from = sendee;
            messageSpawner.to = reciever;
            messageSpawner.newSpawn = true;
        } else if (array[1] == "finished") {
            //destroy gameobject array[0]

        }
    }
}