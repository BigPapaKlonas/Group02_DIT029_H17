using UnityEngine;
using UnityEngine.Networking;

public class MyNetworkManager : MonoBehaviour
{

    public bool isAtStartup = true;

    NetworkClient myClient;

    // client function
    public void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("Connected to server");
    }


    public void OnDisconnected(NetworkMessage netMsg)
    {
        Debug.Log("Disconnected from server");
    }

    public void OnError(NetworkMessage netMsg)
    {
        Debug.Log("Error connecting");
    }

    void Start()
    {
        myClient = new NetworkClient();

        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        myClient.RegisterHandler(MsgType.Disconnect, OnDisconnected);
        myClient.RegisterHandler(MsgType.Error, OnError);

        myClient.Connect("127.0.0.1", 3000);

    }


}