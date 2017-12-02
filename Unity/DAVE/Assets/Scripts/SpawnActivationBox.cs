using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnActivationBox : MonoBehaviour {

    public GameObject activationBoxPrefab;
    public bool newSpawn;
    public bool stop;
    public GameObject systemBox;
    public string systemName;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (newSpawn) {
            SpawnActivation();
            newSpawn = false;
        }
    }

    private void SpawnActivation() {

        systemBox = GameObject.Find(systemName);

        Vector3 positioning = new Vector3(
             systemBox.transform.position.x,
             systemBox.transform.position.y - 1f,
             systemBox.transform.position.z
           );

        GameObject activationBoxGO = (GameObject)Instantiate(
          activationBoxPrefab,
          positioning,
          this.transform.rotation
        );

        ProcessAnimation p = activationBoxGO.GetComponent<ProcessAnimation>();
        p.name = systemName;
        p.current = systemBox;

    }

}
