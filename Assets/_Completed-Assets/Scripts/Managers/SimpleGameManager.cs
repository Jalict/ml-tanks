using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Complete
{
	public class SimpleGameManager : MonoBehaviour {

		public CameraControl m_CameraControl;       // Reference to the CameraControl script for control during different phases.

		public SimpleTankHealth healthTank1;
		public SimpleTankMovement movementTank1;
		public SimpleTankShooting shootingtank1;

		public SimpleTankHealth healthTank2;
		public SimpleTankMovement movementTank2;
		public SimpleTankShooting shootingtank2;


		private void Start() {
			SetCameraTargets();
			ResetGame();
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
				ResetGame();
			}
		}

		private void ResetGame() {
			ResetAllTanks();
			m_CameraControl.SetStartPositionAndSize();
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
}