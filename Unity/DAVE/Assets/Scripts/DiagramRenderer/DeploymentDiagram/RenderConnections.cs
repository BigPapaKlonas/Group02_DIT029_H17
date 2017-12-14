using System.Collections;
using UnityEngine;

public class RenderConnections : MonoBehaviour
{
    private ArrayList processes = new ArrayList();
    private ArrayList deviceList = new ArrayList();
    private ArrayList devices;

    public Material connnectionMaterial;

    public void CreateConnections(ArrayList msgs, float offSet)
    {
        devices = GetComponentInChildren<RenderDevices>().Devices;
        int nrOfProcesses = 0;
        foreach (Device d in devices)
        {
            if (d.GetOffset() == offSet)
            {
                nrOfProcesses += d.GetProcesses().Count;

            }
        }
        foreach (FindDeploymentConnections.MessageData msg in msgs)
        {
            int i = processes.IndexOf(msg.from);
            if (i < 0)
            {
                processes.Add(msg.from);
            }
            int j = processes.IndexOf(msg.to);
            if (j < 0)
            {
                processes.Add(msg.to);
            }
        }

        if (nrOfProcesses == processes.Count)
        {
            foreach (FindDeploymentConnections.MessageData msg in msgs)
                DrawConnection(FindDevice(msg.from, offSet).GetName(), FindDevice(msg.to, offSet).GetName(), offSet);
        }
        processes.Clear();
    }
    public Device FindDevice(string pr, float offSet)
    {
        foreach (Device d in devices)
        {
            if (d.GetOffset() == offSet && d.Contains(pr))
                return d;
        }
        return null;
    }

    void DrawConnection(string from, string to, float offSet)
    {
        GameObject df = GameObject.Find(offSet + from);
        GameObject dt = GameObject.Find(offSet + to);
        GameObject connObject = new GameObject("connObject");
        LineRenderer connection = connObject.AddComponent<LineRenderer>();
        connection.material = connnectionMaterial;
        connection.SetWidth(0.1F, 0.1F);
        connection.SetPosition(0, df.transform.position);
        connection.SetPosition(1, dt.transform.position);
    }

}
