using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSegment : MonoBehaviour {

	float speed = 2.5f;
	public Transform target;
    public bool pause;
    public GameObject parentSystem;
	// Use this for initialization
	void Start () {
        pause = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (pause == false){ 
            Vector3 dir = target.position - this.transform.localPosition;
            float distThisFrame = speed * Time.deltaTime;
            if (dir.magnitude <= distThisFrame)
            {
                parentSystem.GetComponent<SystemBox>().lifeLine.Remove(gameObject);
                Destroy(gameObject);
            }
            else
            {
                transform.Translate(dir.normalized * distThisFrame, Space.World);
            }
        }
		
	}
}
