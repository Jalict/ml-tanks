using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellManager : MonoBehaviour {

	public List<Rigidbody> shellList = new List<Rigidbody>();

	public void Reset() {
		for (int i = 0; i < shellList.Count; i++) {
			Destroy(shellList[i].gameObject);
		}
	}

	public void AddToList(Rigidbody shell) {
		shellList.Add(shell);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
