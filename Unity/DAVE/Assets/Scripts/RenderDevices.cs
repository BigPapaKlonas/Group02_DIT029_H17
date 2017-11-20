using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderNodes : MonoBehaviour
{

    public GameObject devicePrefab;
    public GameObject processPrefab;
    public GameObject communicationPrefab;
    private float zPos;
    ArrayList Devices = new ArrayList();
    // Temporary
    Node[] devices = new Node[3];
    Artifact[] artifacts1 = new Artifact[1];
    Artifact[] artifacts2 = new Artifact[2];
    Artifact[] artifacts3 = new Artifact[3];

    private void Start()
    {
        artifacts2[0] = new Artifact("u1", "User");
        artifacts2[1] = new Artifact("u2", "User");
        artifacts1[0] = new Artifact("k", "Gateway");
        artifacts3[0] = new Artifact("u3", "User");
        artifacts3[1] = new Artifact("u4", "User");
        artifacts3[2] = new Artifact("g", "Gateway");

        for (var i = 0; i < devices.Length; i++)
        {
            if (i % 3 == 0)
                devices[i] = new Node("" + i, "<<device>>", artifacts1);
            else if (i % 3 == 1)
                devices[i] = new Node("" + i, "<<device>>", artifacts2);
            else
                devices[i] = new Node("" + i, "<<device>>", artifacts3);

        }
        CreateNodes();

        Vector3 nPosition = new Vector3(-1, -1, -1);
        GameObject newCom = (GameObject)Instantiate(
            communicationPrefab,
            nPosition,
            this.transform.rotation
            );

    }

    public void CreateDevices(JSONDeployment json)
    {
        // Create devices containing their processes
        foreach (var pair in json.Mapping)
        {
            int i = Devices.IndexOf(pair.Device);
            if (i == -1)
            {
                Devices.Add(new Device(pair.Device, pair.Process));
            }
            else
            {

                Device tmp = (Device)Devices[i];
                tmp.AddProcess(pair.Process);
                Devices[i] = tmp;
            }
        }

        foreach (var device in Devices)
        {

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

    public void CreateNodes()
    {
        int i = 0;
        Vector3 center = new Vector3(0, 0, 0);
        foreach (var node in devices)
        {

            Vector3 pos = PlaceInCircle(center, (float)devices.Length, (float)(360 / devices.Length * i++));
            GameObject newNode = (GameObject)Instantiate(
                devicePrefab,
                pos,
                this.transform.rotation);

            if (node.arts.Length > 1)
            {
                int length = node.arts.Length - 1;
                newNode.transform.localScale += new Vector3(0, (float)(0.8 * length), 0);
                newNode.transform.localPosition += new Vector3(0, (float)(0.8 * length / 2), 0);
                newNode.GetComponentInChildren<TextMesh>().transform.localScale += new Vector3(0, 0, 0);
            }
            newNode.name = node.name;

            newNode.GetComponentInChildren<TextMesh>().text = node.cat + "\n" + node.name;

            pos += new Vector3(0.1F, -0.1F, 0);
            foreach (var artifact in node.arts)
            {

                //Creating the processes
                GameObject newArtifact = (GameObject)Instantiate(
                    processPrefab,
                    //aPosition,
                    pos,
                    this.transform.rotation
                    );
                newArtifact.name = "art" + artifact.name;
                if (artifact.name.Equals("g"))
                    newArtifact.GetComponentInChildren<Light>().intensity = 0;

                newArtifact.GetComponentInChildren<TextMesh>().text = artifact.name + ":" + artifact.classType;

                pos.y += 0.8F; //new Vector3(0, 0.8F, 0);

            }
        }
    }

}
