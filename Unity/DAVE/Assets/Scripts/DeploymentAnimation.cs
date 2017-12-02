using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeploymentAnimation : MonoBehaviour {

    public GameObject communicationPrefab;
    StartMessages.MessageData msg;
    int yDirection, zDirection;
    Vector3 center;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update () {
		
	}

    void AnimateCommunication(StartMessages.MessageData msg)
    {
        

    }

    void StartCommunication(StartMessages.MessageData newMsg, GameObject fromProcess)
    {
        msg = newMsg;
        GameObject fromDevice = GameObject.Find(GetComponent<RenderConnections>().FindDevice(msg.from));
        GameObject toDevice = GameObject.Find(GetComponent<RenderConnections>().FindDevice(msg.to));
        ActivateProcess(true, msg.from);
        GameObject communication = (GameObject)Instantiate(
            communicationPrefab,
            fromDevice.transform.position,
            this.transform.rotation);
        if(fromDevice.transform.position.y > toDevice.transform.position.y)
            yDirection = -1;
        else
            yDirection = 1;
        if(fromDevice.transform.position.z > toDevice.transform.position.z)
            zDirection = 1;
        else
            zDirection = -1;

        center = GetComponent<RenderDevices>().center;
        float yDist = Mathf.Abs(center.y - fromDevice.transform.position.y);
        float zDist = Mathf.Abs(center.z - fromDevice.transform.position.z);
        float rel;
        if (yDist > zDist)
        {
            rel = zDist / yDist;
            for (; !communication.transform.position.Equals(toDevice.transform.position);)
                communication.transform.position += new Vector3(0, 0.1F, 0.1F * rel);

        }
        else
        {
            rel = yDist / zDist;
            for (; !communication.transform.position.Equals(toDevice.transform.position);)
                communication.transform.position += new Vector3(0, 0.1F, 0.1F * rel);
        }
            


        

    }
    void ActivateProcess(bool light, string process)
    {
        GameObject fromProcess = GameObject.Find(process);
        if(light)
            fromProcess.GetComponentInChildren<Light>().intensity = 20;
        else
            fromProcess.GetComponentInChildren<Light>().intensity = 20;
    }

}
