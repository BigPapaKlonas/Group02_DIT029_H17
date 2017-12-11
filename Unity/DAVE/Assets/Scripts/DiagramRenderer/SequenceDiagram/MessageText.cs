using System;
using UnityEngine;

public class MessageText : MonoBehaviour {

    float speed = 1f;
    public Vector3 target;
    public Vector3 origin;
    public string to;
    public string from;
    public string method;
    Vector3 midPoint;
    private GameObject player;
    float distanceToSequence;

    void Start()
    {
        GetComponent<TextMesh>().text = method;
        player = GameObject.FindGameObjectWithTag("Player");

        midPoint = Vector3.Lerp(origin, target, 0.5f);
        Vector3 distanceBetween = Vector3.Lerp(origin, target, 0);

        if (Mathf.Sign(distanceBetween.x) == 1)
        {
            distanceToSequence = distanceBetween.x - 10;
        }
        else
        {
            distanceToSequence = distanceBetween.x + 10;
        }

        Debug.Log("logmsg" + "*"+ midPoint.y + "*" + midPoint.z + "*" + distanceToSequence + "*" + 
            "To: " + to + "\r\nFrom: " + from + "\r\nMessage: " + method);
    }

    void Update()
    {
        //transform.LookAt(transform.position + player.transform.rotation * Vector3.forward,
        //    player.transform.rotation * Vector3.up);
        Vector3 dir = midPoint - this.transform.localPosition;
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
