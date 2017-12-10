using UnityEngine;

public class MessageText : MonoBehaviour {

    public new Camera camera;
    float speed = 1f;
    public Vector3 target;
    public Vector3 origin;
    public string to;
    public string from;
    public string method;
    Vector3 midPoint;
    

    void Start()
    {
        GetComponent<TextMesh>().text = method;
        camera = Camera.main;
        

        midPoint = Vector3.Lerp(origin, target, 0.5f);

        Debug.Log("logmsg" + "*"+ midPoint.y + "*" + midPoint.z + "*" + "To: " + to + "\r\nFrom: " + from + "\r\nMessage: " + method);
    }

    void Update()
    {
        transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward,
            camera.transform.rotation * Vector3.up);
        Vector3 dir = midPoint - this.transform.localPosition;
        //dir.y = dir.y + 0.5f;
        //dir.z = dir.z / 2;
        float distThisFrame = speed * Time.deltaTime;
        if (dir.magnitude <= distThisFrame)
        {
            
        }
        else
        {
            transform.Translate(dir.normalized * distThisFrame, Space.World);
        }
    }
}
