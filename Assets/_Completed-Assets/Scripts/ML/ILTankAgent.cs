using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class ILTankAgent : Agent {
	
	public RayPerception m_RayPerception;

	public Complete.SimpleTankHealth healthTankSelf;
	public Complete.SimpleTankMovement movementTankSelf;
	public Complete.SimpleTankShooting shootingtankSelf;

	public Complete.SimpleTankMovement movementTankOpponent;

	public override void CollectObservations() {
		// Ray Perception
		var rayDistance = 20f;
		float[] rayAngles = { 0f, 45f, 90f, 135f, 180f, 110f, 70f };
		float[] rayAngles2 = { 0f, 30f, 60f, 90f, 120f, 150f, 180f, 210f, 240f, 270f, 300f, 330f, 360f};
		float[] rayAngles3 = { 0f, 20f, 40f, 60f, 80f, 100f, 120f, 140f, 160f, 180f, 200f, 220, 240f, 260f, 280f, 300f, 320f, 340f};
		float[] rayAngles4 = { 0f, 45f, 90f, 135f, 180f, 225f, 270f, 315f };
		var detectableObjects = new[] { "projectile", "Player", "wall" };
		AddVectorObs(m_RayPerception.Perceive(rayDistance, rayAngles4, detectableObjects, 0f, 0f));

		/*
		//Personal position information
		AddVectorObs(movementTankSelf.m_Rigidbody.position.x);
		AddVectorObs(movementTankSelf.m_Rigidbody.position.y);
		AddVectorObs(movementTankSelf.m_Rigidbody.position.z);

		//Opponent position information
		AddVectorObs(movementTankOpponent.m_Rigidbody.position.x);
		AddVectorObs(movementTankOpponent.m_Rigidbody.position.y);
		AddVectorObs(movementTankOpponent.m_Rigidbody.position.z);
		*/
	}

	public override void AgentAction(float[] vectorAction, string textAction) {
		Inputs(vectorAction);
	}

	private void Inputs(float[] vectorAction) {
		// Get Movement Action
		int forward = (int)vectorAction[0];
		int rotation = (int)vectorAction[1];
		bool shoot = vectorAction[2] == 1;

		// Forward
		if (forward == 1) {
			movementTankSelf.m_MovementInputValue = 1;
			AddReward(0.1f);
		} else if (forward == 2) {
			movementTankSelf.m_MovementInputValue = -1;
			AddReward(0.1f);
		} else if(forward == 0) {
			movementTankSelf.m_MovementInputValue = 0;
		}

		// Turn
		if (rotation == 1) {
			movementTankSelf.m_TurnInputValue = -1;
			AddReward(0.1f);
		} else if (rotation == 2) {
			movementTankSelf.m_TurnInputValue = 1;
			AddReward(0.1f);
		} else if(rotation == 0) {
			movementTankSelf.m_TurnInputValue = 0;
		}

		// Shoot
		if (shoot) {
			shootingtankSelf.Fire(shootingtankSelf.m_MinLaunchForce);
			AddReward(0.1f);
		}
	}
}

