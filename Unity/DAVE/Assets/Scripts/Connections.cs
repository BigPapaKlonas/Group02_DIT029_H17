using System;

public class Connections
{
    string[] devices;
    bool[,] matrix;
	public Connections(Device[] d, MessageData[] msgs)
	{
        int length = d.Length;
        devices = new string[d.Length];
        matrix = new bool[d.Length, d.Length];
        foreach(var msg in msgs)
        {
            foreach(var from in d)
            {

                from.Contains(msg.from, msg.to);
                foreach(var to in d)
                {

                }
            }
        }
	}
    public struct MessageData
    {
        public string to;
        public string from;
        public string message;
    }
}
