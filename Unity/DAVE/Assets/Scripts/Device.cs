using System;
using System.Collections;

public class Device 
{

    private string name;
    private ArrayList processes = new ArrayList();

    public Device(string n)
	{
        name = n;
	}

    public Device(string n, string p)
    {
        name = n;
        processes.Add(p);
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
    public Boolean Contains(string from, string to)
    {
        return processes.Contains(from) || processes.Contains(to);
    }
}
