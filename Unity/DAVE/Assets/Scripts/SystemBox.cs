using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemBox : MonoBehaviour {

	public GameObject lTarget;
	public GameObject lineSegmentPrefab;
	float cooldown = 0.5f;
	float cooldownRemaining = 0;
	GameObject target;

    private Light systemLight;
    public bool lightSwitch;
    public List<GameObject> lifeLine;
    float counter = 0;
    public bool activeLifeLine;
    // Use this for initialization
    void Start () {

        activeLifeLine = true;
        lightSwitch = false;
        systemLight = GetComponent<Light>();
        systemLight.enabled = false;
		Vector3 v = new Vector3(this.transform.position.x, 0, this.transform.position.z);
		target = (GameObject)Instantiate(lTarget, v, this.transform.rotation);
		
	}
	
	// Update is called once per frame
	void Update () {

        if (lightSwitch == false && counter > 200) { pauseLifeline(); }
        else
        {
            if (activeLifeLine == false) { startLifeline(); }
            counter++;
            cooldownRemaining -= Time.deltaTime;
            if (cooldownRemaining <= 0)
            {
                cooldownRemaining = cooldown;
                newLifeLine(target);
            }
        }
        if (lightSwitch == false)
        {
            systemLight.enabled = false;
        }
        else
        {
            systemLight.enabled = true;
        }
    }

	void newLifeLine(GameObject t){

		GameObject lifeLineGO = (GameObject)Instantiate(lineSegmentPrefab, this.transform.position, this.transform.rotation);
		LineSegment l = lifeLineGO.GetComponent<LineSegment>();
        l.transform.parent = transform;
		l.target = t.transform;
        l.parentSystem = this.gameObject;
        lifeLine.Add(lifeLineGO);
	}
    void pauseLifeline(){
        foreach (GameObject segment in lifeLine){
            segment.GetComponent<LineSegment>().pause = true;
            activeLifeLine = false;
        }
    }
    void startLifeline(){
        foreach (GameObject segment in lifeLine){
            segment.GetComponent<LineSegment>().pause = false;
            activeLifeLine = true;
        }
    }
}
