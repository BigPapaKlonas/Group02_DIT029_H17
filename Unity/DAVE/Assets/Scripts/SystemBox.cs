using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemBox : MonoBehaviour {

	public GameObject lineSegmentPrefab;
	float cooldown = 0.4f;
	float cooldownRemaining = 0;
	GameObject target;
	// Use this for initialization
	void Start () {

		Vector3 v = new Vector3(this.transform.position.x, 0, this.transform.position.z);
		target = (GameObject)Instantiate(lineSegmentPrefab, v, this.transform.rotation);
		
	}
	
	// Update is called once per frame
	void Update () {

		cooldownRemaining -= Time.deltaTime;
		if(cooldownRemaining <= 0){
			cooldownRemaining = cooldown;
			newLifeLine(target);
		}
		
	}

	void newLifeLine(GameObject t){

		GameObject lifeLineGO = (GameObject)Instantiate(lineSegmentPrefab, this.transform.position, this.transform.rotation);
		LineSegment l = lifeLineGO.GetComponent<LineSegment>();
		l.target = t.transform;
	}
}
