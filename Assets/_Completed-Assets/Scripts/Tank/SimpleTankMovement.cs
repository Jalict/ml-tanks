﻿using UnityEngine;
using UnityEngine.SceneManagement;

namespace Complete
{
    public class SimpleTankMovement : MonoBehaviour
    {
        public int m_PlayerNumber = 1;              // Used to identify which tank belongs to which player.  This is set by this tank's manager.
        public float m_Speed = 12f;                 // How fast the tank moves forward and back.
        public float m_TurnSpeed = 180f;            // How fast the tank turns in degrees per second.
        public AudioSource m_MovementAudio;         // Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.
        public AudioClip m_EngineIdling;            // Audio to play when the tank isn't moving.
        public AudioClip m_EngineDriving;           // Audio to play when the tank is moving.
		public float m_PitchRange = 0.2f;           // The amount by which the pitch of the engine noises can vary.

        private string m_MovementAxisName;          // The name of the input axis for moving forward and back.
        private string m_TurnAxisName;              // The name of the input axis for turning.
        public Rigidbody m_Rigidbody;              // Reference used to move the tank.
        public float m_MovementInputValue;         // The current value of the movement input.
        public float m_TurnInputValue;             // The current value of the turn input.
        private float m_OriginalPitch;              // The pitch of the audio source at the start of the scene.

		//NEW SHIT
		public Color m_PlayerColor;                             // This is the color this tank will be tinted.
		public Spawnpoints spawnpoints;
		//public Transform m_SpawnPoint;                          // The position and direction the tank will have when it spawns.

        public string filename;


		private void Awake ()
        {
            m_Rigidbody = GetComponent<Rigidbody> ();


			//NEW SHIT
			
			// Get all of the renderers of the tank.
			MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
			
			// Go through all the renderers...
			for (int i = 0; i < renderers.Length; i++) {
				// ... set their material color to the color specific to this tank.
				renderers[i].material.color = m_PlayerColor;
			}
		}


		// NEW SHIT
		// Used at the start of each round to put the tank into it's default state.
		public void Reset() {
			//transform.position = m_SpawnPoint.position;
			//transform.rotation = m_SpawnPoint.rotation;
			if(m_PlayerNumber == 1) {
				transform.position = spawnpoints.spawn1.position;
				transform.rotation = Quaternion.Euler(0, spawnpoints.spawnAngle1, 0);
			}
			if(m_PlayerNumber == 2) {
				transform.position = spawnpoints.spawn2.position;
				transform.rotation = Quaternion.Euler(0, spawnpoints.spawnAngle2, 0);
			}

			// When the tank is turned off, set it to kinematic so it stops moving.
			m_Rigidbody.isKinematic = true;

			// When the tank is turned on, make sure it's not kinematic.
			m_Rigidbody.isKinematic = false;

			// Also reset the input values.
			m_MovementInputValue = 0f;
			m_TurnInputValue = 0f;

            Statestics.WriteDataPoint(m_PlayerNumber, StateType.AmountOfMovement, amountOfMovement);
		}


		private void Start ()
        {
            // The axes names are based on player number.
            m_MovementAxisName = "Vertical" + m_PlayerNumber;
            m_TurnAxisName = "Horizontal" + m_PlayerNumber;

            // Store the original pitch of the audio source.
            m_OriginalPitch = m_MovementAudio.pitch;
        }

        private Vector3 lastPosition;
        private float amountOfMovement;

        private void Update ()
        {
            // Store the value of both input axes.
            //if(Input.GetAxis(m_MovementAxisName) != 0)
            //	m_MovementInputValue = Input.GetAxis (m_MovementAxisName);
            //if(Input.GetAxis(m_TurnAxisName) != 0)
            //	m_TurnInputValue = Input.GetAxis (m_TurnAxisName);

            //Debug.Log("Move: " + m_MovementInputValue + ", Turn: " + m_TurnInputValue);

            amountOfMovement += Vector3.Distance(transform.position, lastPosition);
            lastPosition = transform.position;

            EngineAudio ();
        }


        private void EngineAudio ()
        {
            // If there is no input (the tank is stationary)...
            if (Mathf.Abs (m_MovementInputValue) < 0.1f && Mathf.Abs (m_TurnInputValue) < 0.1f)
            {
                // ... and if the audio source is currently playing the driving clip...
                if (m_MovementAudio.clip == m_EngineDriving)
                {
                    // ... change the clip to idling and play it.
                    m_MovementAudio.clip = m_EngineIdling;
                    m_MovementAudio.pitch = Random.Range (m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play ();
                }
            }
            else
            {
                // Otherwise if the tank is moving and if the idling clip is currently playing...
                if (m_MovementAudio.clip == m_EngineIdling)
                {
                    // ... change the clip to driving and play.
                    m_MovementAudio.clip = m_EngineDriving;
                    m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play();
                }
            }
        }


        private void FixedUpdate ()
        {
            // Adjust the rigidbodies position and orientation in FixedUpdate.
            Move ();
            Turn ();
        }


        private void Move ()
        {
            // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
            Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;

            // Apply this movement to the rigidbody's position.
            m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
        }


        private void Turn ()
        {
            // Determine the number of degrees to be turned based on the input, speed and time between frames.
            float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

            // Make this into a rotation in the y axis.
            Quaternion turnRotation = Quaternion.Euler (0f, turn, 0f);

            // Apply this rotation to the rigidbody's rotation.
            m_Rigidbody.MoveRotation (m_Rigidbody.rotation * turnRotation);
        }
    }
}