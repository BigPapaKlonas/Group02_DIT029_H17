using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System;

public class MqttClientDAVE
{
    private MqttClient client;

    //Tries to establish a connection to the broker at the IP and Port provided and subscribes to SubsribeTopic
    public MqttClientDAVE(String brokerIpAdress, int brokerPort, String clientID)
    {
        // create client instance 
        client = new MqttClient(IPAddress.Parse(brokerIpAdress), brokerPort, false, null);

        // sets the client ID for this connection
        //string clientID = Guid.NewGuid().ToString();
        client.Connect(clientID);
    }

    public MqttClient GetMqttClient()
    {
        return client;
    }

    // Subscribes to SubscribeTopic
    public void Subscribe(String SubscribeTopic)
    {
        client.Subscribe(new string[]{ SubscribeTopic } , new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
    }

    // Unsubscribes from UnsubscribeTopic
    public void Unsubscribe(String UnsubscribeTopic)
    {
        client.Unsubscribe(new string[] { UnsubscribeTopic });
    }

    // Publishes PublicMsg to PublishTopic
    public void Publish(String PublishTopic, String PublishMsg, Boolean retainMsg)
    {
        client.Publish(PublishTopic, System.Text.Encoding.UTF8.GetBytes(PublishMsg), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, retainMsg);
    }
}
