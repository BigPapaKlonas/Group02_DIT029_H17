using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderDevices : MonoBehaviour
{

    public GameObject devicePrefab;
    public GameObject processPrefab;
    public GameObject communicationPrefab;
    ArrayList Devices = new ArrayList();
    ArrayList DeviceNames = new ArrayList();

    public void CreateDevices(JSONDeployment json)
    {
        // Create devices containing their processes
        int i;
        float yPos = 4.890001F;
        int biggest = 1;
        foreach (var pair in json.Mapping)
        {
            i = DeviceNames.IndexOf(pair.Device);
            if (i == -1)
            {
                Devices.Add(new Device(pair.Device, pair.Process));
                DeviceNames.Add(pair.Device);
            }
            else
            {

                Device tmp = (Device)Devices[i];
                tmp.AddProcess(pair.Process);
                if(tmp.GetProcesses().Count > biggest)
                {
                    biggest++;
                    yPos += 1; 
                }
                Devices[i] = tmp;
            }
        }

        i = 0;
        Vector3 center = new Vector3(0, yPos, 9.880002F);

        foreach (Device device in Devices)
        {
            Vector3 pos = PlaceInCircle(center, (float)Devices.Count, (float)(360 / Devices.Count * i++));
            GameObject newDevice = (GameObject)Instantiate(
                devicePrefab,
                pos,
                this.transform.rotation);

            if (device.GetProcesses().Count > 1)
            {
                int length = device.GetProcesses().Count - 1;
                newDevice.transform.localScale += new Vector3(0, (float)(0.8 * length), 0);
                newDevice.transform.localPosition += new Vector3(0, (float)(0.8 * length / 2), 0);
                newDevice.GetComponentInChildren<TextMesh>().transform.localScale += new Vector3(0, 0, 0);
            }
            newDevice.name = device.GetName();

            newDevice.GetComponentInChildren<TextMesh>().text = "<<Device>>\n" + device.GetName();

            pos += new Vector3(0.1F, -0.1F, 0);
            foreach (string process in device.GetProcesses())
            {

                //Creating the processes
                GameObject newProcess = (GameObject)Instantiate(
                    processPrefab,
                    pos,
                    this.transform.rotation
                    );
                newProcess.name = "proc:" + process;

                //TURN OFF THE LIGHT
                newProcess.GetComponentInChildren<Light>().intensity = 0;

                newProcess.GetComponentInChildren<TextMesh>().text =  ":" + process;

                pos.y += 0.8F;

            }
        }
    }

    Vector3 PlaceInCircle(Vector3 center, float radius, float ang)
    {
        Vector3 pos;
        pos.x = center.x;
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        return pos;

    }

}
