using UnityEngine;

public class DeploymentAnimation : MonoBehaviour {

    public GameObject communicationPrefab;

    GameObject fromDevice, toDevice, communication, fromProcess, toProcess = null;
    public string From, To, FromDevice, ToDevice; 
    float speed = 0.5f;
    Device from, to;
    StartMessages.MessageData msg;
    public bool send;
    bool start;
    // Update is called once per frame
    void Update () {

        if (send && start)
        {
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
                send = false;
            }



        }
	}

    public void StartCommunication()//StartMessages.MessageData message)
    {
        //msg = message;
        // Get the name of the devices the processes are mapped to
        from = GetComponent<RenderConnections>().FindDevice(From);//msg.from);
        to = GetComponent<RenderConnections>().FindDevice(To);//msg.to);
        // Find the gameobject with the name of the device
        fromDevice = GameObject.Find(from.GetName());
        toDevice = GameObject.Find(to.GetName());

        // Create the communication light in the posistion of the device sending the message
        communication = (GameObject)Instantiate(
            communicationPrefab,
            fromDevice.transform.position,
            this.transform.rotation);

        // Find the process
        if (from.GetProcesses().Count < 8)
            fromProcess = GameObject.Find("proc:" + From);// msg.from);
         else
          fromProcess = GameObject.Find(from + ":multi");

        if (from.GetProcesses().Count < 8)
            toProcess = GameObject.Find("proc:" + To);// msg.to);
         else
         toProcess = GameObject.Find(to + ":multi");
        start = true;

    }
    // Needs the distance of the activation boxes as arguments
    public void SetSpeed(GameObject fromSystem, GameObject toSystem)
    {
        float deviceDist = Mathf.Sqrt(Mathf.Pow(fromDevice.transform.position.z - toDevice.transform.position.z, 2)
            + Mathf.Pow(fromDevice.transform.position.y - toDevice.transform.position.y, 2));
        float systemDist = Mathf.Abs(fromSystem.transform.position.z - toSystem.transform.position.z);
        speed = deviceDist * 0.01F / systemDist;
    }

}
