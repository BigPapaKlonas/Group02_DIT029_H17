using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderConnections : MonoBehaviour
{

    ArrayList processes = new ArrayList();
    ArrayList deviceList = new ArrayList();
    public Material connnectionMaterial;
    ArrayList devices;
    public void CreateConnections(ArrayList msgs)
    {
        devices = RenderDevices.Devices;
        int nrOfProcesses = 0;
        foreach (Device d in devices)
            nrOfProcesses += d.GetProcesses().Count;

        foreach (FindDeploymentConnections.MessageData msg in msgs)
        {

            int i = processes.IndexOf(msg.from);
            int j = processes.IndexOf(msg.to);
            if (i < 0)
            {
                processes.Add( msg.from);
                deviceList.Add(FindDevice(msg.from));
            }

            if (j < 0)
            {
                processes.Add(msg.to);
                deviceList.Add(FindDevice(msg.to));
            }
        }

        if (nrOfProcesses == processes.Count)
        {
            foreach (FindDeploymentConnections.MessageData msg in msgs)
                DrawConnection(FindDevice(msg.from).GetName(), FindDevice(msg.to).GetName());
        }
    }
    public Device FindDevice(string pr)
    {
        foreach(Device d in devices)
        {
            if (d.Contains(pr))
                return d;
        }
        return null;
    }

    void DrawConnection(string from, string to)
    {

        GameObject df = GameObject.Find(from);
        GameObject dt = GameObject.Find(to);
        GameObject connObject = new GameObject("connObject");
        LineRenderer connection = connObject.AddComponent<LineRenderer>();
        connection.material = connnectionMaterial;
        connection.SetWidth(0.1F, 0.1F);
        connection.SetPosition(0, df.transform.position);
        connection.SetPosition(1, dt.transform.position);


    }

}
