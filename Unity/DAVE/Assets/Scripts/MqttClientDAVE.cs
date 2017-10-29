using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System;
using UnityEngine;

public class MqttClientDAVE
{
    private MqttClient client;

    //Tries to establish a connection to the broker at the IP and Port provided and subscribes to SubsribeTopic
    public MqttClientDAVE(String IPAdress, int Port, String SubscribeTopic)
    {
        // create client instance 
        client = new MqttClient(IPAddress.Parse(IPAdress), Port, false, null);

        // register to message received 
        client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;

        // generates unique client ID
        string clientId = Guid.NewGuid().ToString();
        client.Connect(clientId);

        // subscribe to SubscribeTopic with QoS 2 (EXACTLY_ONCE)
        client.Subscribe(new string[] { SubscribeTopic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
    }

    // Publishes PublicMsg to PublishTopic
    public void Publish(String PublishTopic, String PublishMsg)
    {
        client.Publish(PublishTopic, System.Text.Encoding.UTF8.GetBytes(PublishMsg), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
    }

    //Logs what was received on the subscribed topic SubscribeTopic
    void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        Debug.Log("Received: " + System.Text.Encoding.UTF8.GetString(e.Message));
    }
}
