using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSDInit : MonoBehaviour {

	public GameObject player;
	public GameObject ssdSpawnerPrefab;
	public bool spawnSpawner;
	public string room;
	public int size;
	public int offset;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SpawnSSDSpawner (float offset, string SSDroom) {

		Vector3 pos = new Vector3(offset + 15, 10, offset + 15);

		GameObject SSDGO = (GameObject)Instantiate(
                    ssdSpawnerPrefab,
                    pos,
                    player.transform.rotation
                );

		SSDSpawner spawner = SSDGO.GetComponent<SSDSpawner>();
		spawner.room = SSDroom;
		spawner.size = size;
		Debug.Log(SSDroom);

	}
}
