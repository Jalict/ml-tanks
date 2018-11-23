using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MLAgents;
using System;
using System.Collections.Generic;

namespace Complete
{
	public class SimpleGameManager : MonoBehaviour {

		public List<Rigidbody> shellList = new List<Rigidbody>();

		public CameraControl m_CameraControl;       // Reference to the CameraControl script for control during different phases.

		public Agent agentTank1;
		public SimpleTankHealth healthTank1;
		public SimpleTankMovement movementTank1;
		public SimpleTankShooting shootingtank1;

		public Agent agentTank2;
		public SimpleTankHealth healthTank2;
		public SimpleTankMovement movementTank2;
		public SimpleTankShooting shootingtank2;


		private void Start() {
			SetCameraTargets();
			ResetGame();
		}

		public void AddToShellList(Rigidbody shell) {
			shellList.Add(shell);
		}

		private void SetCameraTargets() {
			// Create a collection of transforms the same size as the number of tanks.
			Transform[] targets = new Transform[2];

			// Put the tank transforms into the array
			targets[0] = healthTank1.transform;
			targets[1] = healthTank2.transform;

			// These are the targets the camera should follow.
			m_CameraControl.m_Targets = targets;
		}

		private void Update() {
			if (OneTankLeft()) {
				agentTank1.Done();
				agentTank2.Done();
				ResetGame();
			}
		}

		private void ResetGame() {
            CleanShells();
			ResetAllTanks();
			m_CameraControl.SetStartPositionAndSize();
		}

        private void CleanShells() {
			
			for (int i = 0; i < shellList.Count; i++) {
				Rigidbody shell = shellList[i];

				//Remove the shells from the list
				shellList.Remove(shell);

				//Destroy the shells that aren't already destroyed
				Destroy(shell.gameObject);
			}
		}

        private bool OneTankLeft() {
			if (healthTank1.m_Dead || healthTank2.m_Dead) {
				return true;
			}
			return false;
		}


		// This function is used to turn all the tanks back on and reset their positions and properties.
		private void ResetAllTanks() {
			healthTank1.Reset();
			movementTank1.Reset();
			shootingtank1.Reset();

			healthTank2.Reset();
			movementTank2.Reset();
			shootingtank2.Reset();
		}
	}

    public struct TankComponents
    {
        public SimpleTankHealth m_Health;
        public SimpleTankMovement m_Movement;
        public SimpleTankShooting m_Shooting;
        public RayPerception m_RayPerception;
    }
}