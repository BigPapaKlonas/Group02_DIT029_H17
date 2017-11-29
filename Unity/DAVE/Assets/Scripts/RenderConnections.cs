using System;
using System.Collections;

public class RenderConnections
{

    bool[,] connections;
    int i, j = -1;
    public RenderConnections(ArrayList devices, MessageData[] msgs)
    {

        connections = new bool[devices.Count, devices.Count];
        // Find in which device a message is sent from and to
        foreach (MessageData msg in msgs)
        {
            foreach (Device device in devices)
            {
                if (device.Contains(msg.from))
                    i = devices.IndexOf(device);

                if (device.Contains(msg.to))
                    j = devices.IndexOf(device);

                if (i > -1 && j > -1)
                {
                    AddConnection(i, j);
                    DrawConnections(i, j);
                    i = j = -1;
                }
            }
        }
    }

    void DrawConnections(int i, int j)
    {
        
    }
    void AddConnection(int i, int j)
    {
        connections[i, j] = true;
        connections[j, i] = true;
    }
    public struct MessageData
    {
        public string to;
        public string from;
        public string message;
    }
}
