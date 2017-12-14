using System;
using System.Collections;

public class Device
{

    private string name;
    private ArrayList processes = new ArrayList();
    private float offSet;

    public Device(string n)
    {
        name = n;
    }

    public Device(string n, string p, float o)
    {
        name = n;
        processes.Add(p);
        offSet = o;
    }

    public void AddProcess(string p)
    {
        processes.Add(p);
    }

    public ArrayList GetProcesses()
    {
        return processes;
    }

    public string GetName()
    {
        return name;
    }
    public Boolean Contains(string process)
    {
        return processes.Contains(process);
    }

    public void SetOffset(float offset)
    {
        offSet = offset;
    }
    public float GetOffset()
    {
        return offSet;
    }
}
