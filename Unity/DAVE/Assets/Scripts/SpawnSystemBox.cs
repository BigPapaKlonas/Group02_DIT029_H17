using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystemBox : MonoBehaviour {

    public GameObject systemBoxPrefab;
    public bool newSpawn;
    public string systemBoxName;
    public int size = 10;

    private int p = 0;
    // Use this for initialization
    void Start() {
        newSpawn = false;
    }
    void Update() {
        if (newSpawn) {
            SpawnSystem();
            newSpawn = false;
        }
    }
    private void SpawnSystem() {

        
        Vector3 position = new Vector3(0, size, p);
        GameObject box = (GameObject)Instantiate(
          systemBoxPrefab,
          position,
          this.transform.rotation
        );
        box.name = systemBoxName;
        // Changes the Z position
        p = p + 3;

        box.GetComponentInChildren<TextMesh>().text =
          systemBoxName + " : ";

        
    }
        
    
}
