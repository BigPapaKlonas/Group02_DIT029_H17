using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSDInit : MonoBehaviour {

	public GameObject player;
	public GameObject ssdSpawnerPrefab;
	public bool spawnSpawner;
	public string room;
	public int size;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (spawnSpawner)
		{
			SpawnSSDSpawner();
			spawnSpawner = false;
		}
	}

	public void SpawnSSDSpawner () {

		GameObject SSDGO = (GameObject)Instantiate(
                    ssdSpawnerPrefab,
                    player.transform.position,
                    player.transform.rotation
                );

		SSDSpawner spawner = SSDGO.GetComponent<SSDSpawner>();
		spawner.room = room;
		spawner.size = size;
		Debug.Log(room);

	}
}
