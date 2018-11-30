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
		spawnPoints = GetComponentsInChildren<Transform>();

		if(spawnPoints.Length > 2) {
			spawn1 = spawnPoints[0];
			spawn2 = spawnPoints[1];
		}
	}

	//Get new random spawnpoints
	public void SetNewSpawnPoints() {
		//Find two new random spawns from our list
		int rand1 = Random.Range(0, spawnPoints.Length);
		int rand2 = Random.Range(0, spawnPoints.Length);
		while(rand1 == rand2) {
			rand2 = Random.Range(0, spawnPoints.Length);
		}
		spawn1 = spawnPoints[rand1];
		spawn2 = spawnPoints[rand2];

		//Randomize angles, but always keep the tanks facing opposite directions
		int newAngle = Random.Range(0, 4) * 90;
		spawnAngle1 = 0 + newAngle;
		spawnAngle2 = 180 + newAngle;
	}
}
