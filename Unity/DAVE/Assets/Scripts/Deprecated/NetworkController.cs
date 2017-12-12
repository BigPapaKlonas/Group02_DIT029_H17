using System;
using System.IO;
using System.Threading;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkController : MonoBehaviour
{

    private Message testMsg;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Use this for initialization
    void Start()
    {

        byte[] msgCon = System.Text.Encoding.ASCII.GetBytes("TEST");
        testMsg = new Message(msgCon);

        startServer();

        send(testMsg);
    }

    // Update is called once per frame
    void Update()
    {
        processMessage();
    }

    static TcpClient client = null;
    static BinaryReader reader = null;
    static BinaryWriter writer = null;
    static Thread networkThread = null;
    private static Queue<Message> messageQueue = new Queue<Message>();

    static void addItemToQueue(Message item)
    {
        lock (messageQueue)
        {
            messageQueue.Enqueue(item);
        }
    }

    static Message getItemFromQueue()
    {
        lock (messageQueue)
        {
            if (messageQueue.Count > 0)
            {
                return messageQueue.Dequeue();
            }
            else
            {
                return null;
            }
        }
    }

    static void processMessage()
    {
        Message msg = getItemFromQueue();
        if (msg != null)
        {
            Debug.Log("Returned: " + msg);
        }
    }

    static void startServer()
    {
        if (networkThread == null)
        {
            connect();
            networkThread = new Thread(() => {
                while (reader != null)
                {
                    Message msg = Message.ReadFromStream(reader);
                    addItemToQueue(msg);
                }
                lock (networkThread)
                {
                    networkThread = null;
                }
            });
            networkThread.Start();
        }
    }

    static void connect()
    {
        if (client == null)
        {
            string server = "localhost";
            int port = 3000;
            client = new TcpClient(server, port);
            Stream stream = client.GetStream();
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);
        }
    }

    public static void send(Message msg)
    {
        msg.WriteToStream(writer);
        writer.Flush();
    }
}