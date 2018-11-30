using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoints : MonoBehaviour {

	public Transform[] spawnPoints;

	public Transform spawn1;
	public int spawnAngle1 = 0;

	public Transform spawn2;
	public int spawnAngle2 = 180;
	
	//Get all the spawnpoints in the children of this object
	void Start () {
		spawnPoints = new Transform[transform.childCount];

		for (int i = 0; i < transform.childCount;i++) {
			spawnPoints[i] = transform.GetChild(i).transform;
		}

        SetNewSpawnPoints();
	}

	//Get new random spawnpoints
	public void SetNewSpawnPoints() {

        List<Transform> spawnPointBuffer = new List<Transform>();
        spawnPointBuffer.AddRange(spawnPoints);

        spawn1 = spawnPoints[Random.Range(0, spawnPointBuffer.Count)];
        spawnPointBuffer.Remove(spawn1);
        spawn2 = spawnPoints[Random.Range(0, spawnPointBuffer.Count)];

		//Randomize angles, but always keep the tanks facing opposite directions
		int newAngle = Random.Range(0, 4) * 90;
		spawnAngle1 = 0 + newAngle;
		spawnAngle2 = 180 + newAngle;
	}
}
