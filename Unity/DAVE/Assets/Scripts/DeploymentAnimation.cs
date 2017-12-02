using UnityEngine;

public class DeploymentAnimation : MonoBehaviour {

    public GameObject communicationPrefab;

    GameObject fromDevice, toDevice, communication, fromProcess, toProcess;
    float speed = 0.01f;
    Device from, to;
    StartMessages.MessageData msg;

    // Update is called once per frame
    void Update () {
        // Turn on process light
        if (communication.transform.position.Equals(fromDevice.transform.position))
        {
            fromProcess.GetComponentInChildren<Light>().intensity = 10;
            if (from.GetProcesses().Count > 8)
                GetComponent<RenderDevices>().ChangeProcessText(msg.from);      // Attached?

        }
                  
        // Communication light moving from the device sending a message to the device recieving the message
        float step = speed * Time.deltaTime;
        communication.transform.position = Vector3.MoveTowards(communication.transform.position, toDevice.transform.position, step);
        
        // Turn off process light 
        if (communication.transform.position.Equals(toDevice.transform.position))
        {
            fromProcess.GetComponentInChildren<Light>().intensity = 0;
            if (from.GetProcesses().Count > 8)
                GetComponent<RenderDevices>().ChangeProcessText("9...*");       // Attached?
        }
               
		
	}

    public void StartCommunication(StartMessages.MessageData message)
    {
        msg = message;
        from = GetComponent<RenderConnections>().FindDevice(msg.from);
        to = GetComponent<RenderConnections>().FindDevice(msg.to);
        fromDevice = GameObject.Find(from.GetName());
        toDevice = GameObject.Find(GetComponent<RenderConnections>().FindDevice(msg.to).GetName());
        communication = (GameObject)Instantiate(
            communicationPrefab,
            fromDevice.transform.position,
            this.transform.rotation);
        if (from.GetProcesses().Count < 8)
            fromProcess = GameObject.Find("proc:" + msg.from);
        else
            fromProcess = GameObject.Find(from + ":multi");

        if (from.GetProcesses().Count < 8)
            toProcess = GameObject.Find("proc:" + msg.to);
        else
            toProcess = GameObject.Find(to + ":multi");

        }
    // Byt eventuellt GameObjects till positioner
    public void SetSpeed(GameObject fromSystem, GameObject toSystem)
    {
        float deviceDist = Mathf.Sqrt(Mathf.Pow(fromDevice.transform.position.z - toDevice.transform.position.z, 2)
            + Mathf.Pow(fromDevice.transform.position.y - toDevice.transform.position.y, 2));
        float systemDist = Mathf.Abs(fromSystem.transform.position.z - toSystem.transform.position.z);
        speed = deviceDist * 0.01F / systemDist;
    }

}
