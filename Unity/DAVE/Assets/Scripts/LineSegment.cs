using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSegment : MonoBehaviour {

	float speed = 3f;
	public Transform target;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 dir =  target.position - this.transform.localPosition;
		float distThisFrame = speed * Time.deltaTime;
		if(dir.magnitude <= distThisFrame){
			Destroy(gameObject);
		}else{
			transform.Translate(dir.normalized * distThisFrame, Space.World);
		}
		
	}
}
