using UnityEngine;
using UnityEngine.UI;

namespace Complete
{
    public class SimpleTankShooting : MonoBehaviour
    {
        public int m_PlayerNumber = 1;              // Used to identify the different players.
        public Rigidbody m_Shell;                   // Prefab of the shell.
        public Transform m_FireTransform;           // A child of the tank where the shells are spawned.
        public AudioSource m_ShootingAudio;         // Reference to the audio source used to play the shooting audio. NB: different to the movement audio source.
        public AudioClip m_ChargingClip;            // Audio that plays when each shot is charging up.
        public AudioClip m_FireClip;                // Audio that plays when each shot is fired.
        public float m_MinLaunchForce = 15f;        // The force given to the shell if the fire button is not held.


        private string m_FireButton;                // The input axis that is used for launching shells.
        private float m_CurrentLaunchForce;         // The force that will be given to the shell when the fire button is released.
        private bool m_Fired;                       // Whether or not the shell has been launched with this button press.

		private float shootCooldown = 0.5f;
		private float shootCooldownTimer = 0.0f;
		private bool shotOnCooldown = false;

		public void Reset() {
			// When the tank is turned on, reset the launch force and the UI
			m_CurrentLaunchForce = m_MinLaunchForce;
		}

		private void Start ()
        {
            // The fire axis is based on the player number.
            m_FireButton = "Fire" + m_PlayerNumber;
        }

		private void Update() {
			if (shotOnCooldown) {
				shootCooldownTimer += Time.deltaTime;
				if (shootCooldownTimer > shootCooldown) {
					shotOnCooldown = false;
					shootCooldownTimer = 0.0f;
				}
			}
		}

		public bool Fire (float launchForce)
        {
			if (!shotOnCooldown) {

				//Put the shot on cooldown
				shotOnCooldown = true;

				// Create an instance of the shell and store a reference to it's rigidbody.
				Rigidbody shellInstance =
					Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

				// Set the shell's velocity to the launch force in the fire position's forward direction.
				shellInstance.velocity = launchForce * m_FireTransform.forward;

				// Change the clip to the firing clip and play it.
				m_ShootingAudio.clip = m_FireClip;
				m_ShootingAudio.Play();

				// Reset the launch force.  This is a precaution in case of missing button events.
				m_CurrentLaunchForce = m_MinLaunchForce;
				return true;
			}
			return false;
        }
    }
}