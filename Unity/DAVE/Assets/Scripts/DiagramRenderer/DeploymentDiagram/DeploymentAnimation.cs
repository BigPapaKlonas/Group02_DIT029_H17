using UnityEngine;

public class DeploymentAnimation : MonoBehaviour {

    public GameObject communicationPrefab;

    GameObject fromDevice, toDevice, communication, fromProcess, toProcess = null;
    float speed = 0.5f;
    Device from, to;
    string ToS, FromS;
    public Material activeMaterial, inactiveMaterial;
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
                fromProcess.GetComponent<Renderer>().material = activeMaterial;
                if (from.GetProcesses().Count > 8)
                {
                    GetComponent<RenderDevices>().ChangeProcessText(FromS);
                    fromProcess.GetComponentInChildren<Light>().intensity = 100;
                }

            }
            // Communication light moving from the device sending a message to the device recieving the message

            float step = speed * Time.deltaTime;
            communication.transform.position = Vector3.MoveTowards(communication.transform.position, toDevice.transform.position, step);

            // Turn off process light 
            if (communication.transform.position.Equals(toDevice.transform.position))
            {
                fromProcess.GetComponent<Renderer>().material = inactiveMaterial;
                fromProcess.GetComponentInChildren<Light>().intensity = 0;
                if (from.GetProcesses().Count > 8)
                    GetComponent<RenderDevices>().ChangeProcessText("9...*"); 
                send = false;
            }



        }
	}

    public void StartCommunication(string From, string To)
    {
        if (!start) {

            FromS = From;
            ToS = To;
            // Get the name of the devices the processes are mapped to
            from = GetComponent<RenderConnections>().FindDevice(FromS);
        to = GetComponent<RenderConnections>().FindDevice(ToS);
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
                fromProcess = GameObject.Find("proc:" + FromS);
            else
                fromProcess = GameObject.Find(from.GetName() + ":multi");

        if (from.GetProcesses().Count < 8)
            toProcess = GameObject.Find("proc:" + ToS);
         else
         toProcess = GameObject.Find(to.GetName() + ":multi");
        start = true;
        }

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
