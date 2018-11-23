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
		var detectableObjects = new[] { "projectile", "Player", "wall" };
		AddVectorObs(m_RayPerception.Perceive(rayDistance, rayAngles2, detectableObjects, 0f, 0f));

		//Personal movement information
		AddVectorObs(movementTankSelf.m_Rigidbody.velocity.x);
		AddVectorObs(movementTankSelf.m_Rigidbody.velocity.y);
		AddVectorObs(movementTankSelf.m_Rigidbody.velocity.z);
		AddVectorObs(movementTankSelf.m_Rigidbody.angularVelocity);

		//Opponent movement information
		AddVectorObs(movementTankOpponent.m_Rigidbody.velocity.x);
		AddVectorObs(movementTankOpponent.m_Rigidbody.velocity.y);
		AddVectorObs(movementTankOpponent.m_Rigidbody.velocity.z);
		AddVectorObs(movementTankOpponent.m_Rigidbody.angularVelocity);
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

