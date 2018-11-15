using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Complete
{
    public class ILGameManager : MonoBehaviour {

        public CameraControl m_CameraControl;			 // Reference to the CameraControl script for control during different phases.
        public ILTankManager[] m_Tanks;					 // A collection of managers for enabling and disabling different aspects of the tanks.

        private ILTankManager m_RoundWinner;			 // Reference to the winner of the current round.  Used to make an announcement of who won.

        private void Start() {

            SetCameraTargets();
        }

		public void Update() {
			if (Input.GetKeyDown(KeyCode.R)) {
				QuickReset();
			}
		}

		private void SetCameraTargets() {
            // Create a collection of transforms the same size as the number of tanks.
            Transform[] targets = new Transform[m_Tanks.Length];

            // For each of these transforms...
            for (int i = 0; i < targets.Length; i++)
            {
                // ... set it to the appropriate tank transform.
                targets[i] = m_Tanks[i].m_Instance.transform;
            }

            // These are the targets the camera should follow.
            m_CameraControl.m_Targets = targets;
        }

        // This function is used to turn all the tanks back on and reset their positions and properties.
        private void ResetAllTanks()
        {
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                m_Tanks[i].Reset();
            }
        }

        // Quick reset for Machine Learning
        public void QuickReset()
        {
            ResetAllTanks();

            m_CameraControl.SetStartPositionAndSize();

            EnableTankControl();
        }

        private void EnableTankControl()
        {
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                m_Tanks[i].EnableControl();
            }
        }

        private void DisableTankControl()
        {
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                m_Tanks[i].DisableControl();
            }
        }
    }
}