using MLAgents;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Complete
{
	public class SimpleGameManager : MonoBehaviour {

		public CameraControl m_CameraControl;       // Reference to the CameraControl script for control during different phases.

        private TankComponents[] tanks;


		private void Start() {
            FindTanks();
			SetCameraTargets();
			ResetGame();
		}

        private void FindTanks()
        {
            // Find Tanks with 'player' tag
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            // Make TankComponents array length of found players
            tanks = new TankComponents[players.Length];

            // Assign components to struct array
            for(int i = 0; i < players.Length;i++)
            {
                tanks[i] = new TankComponents
                {
                    health = players[i].GetComponent<SimpleTankHealth>(),
                    movement = players[i].GetComponent<SimpleTankMovement>(),
                    shooting = players[i].GetComponent<SimpleTankShooting>(),
                    ray = players[i].GetComponent<RayPerception>()
                };
            }
        }

        private void SetCameraTargets() {
			// Create a collection of transforms the same size as the number of tanks.
			Transform[] targets = new Transform[tanks.Length];

			// Put the tank transforms into the array
            for(int i = 0; i < targets.Length;i++)
            {
                targets[i] = tanks[i].movement.transform;
            }

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
            CleanUpLevel();
			m_CameraControl.SetStartPositionAndSize();
		}

        private void CleanUpLevel()
        {
            // Delete all projectiles
            GameObject[] objs = GameObject.FindGameObjectsWithTag("projectile");
            for(int i = objs.Length - 1; i > 0;i--)
            {
                Destroy(objs[i]);
            }
        }

        private bool OneTankLeft() {
            int alive = 0;
            foreach(TankComponents coms in tanks) {
                if (!coms.health.m_Dead)
                    alive++;
            }

            return alive == 1;
		}


		// This function is used to turn all the tanks back on and reset their positions and properties.
		private void ResetAllTanks() { 
            foreach(TankComponents coms in tanks) {
                coms.health.Reset();
                coms.movement.Reset();
                coms.shooting.Reset();
            }
		}
	}

    public struct TankComponents
    {
        public SimpleTankHealth health;
        public SimpleTankMovement movement;
        public SimpleTankShooting shooting;
        public RayPerception ray;

        public TankComponents(SimpleTankHealth health, SimpleTankMovement movement, SimpleTankShooting shooting, RayPerception ray)
        {
            this.health = health;
            this.movement = movement;
            this.shooting = shooting;
            this.ray = ray;
        }
    }
}