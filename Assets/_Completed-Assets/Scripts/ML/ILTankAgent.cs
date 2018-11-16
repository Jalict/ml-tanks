using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class ILTankAgent : Agent {
	
	public RayPerception m_RayPerception;

	public Complete.SimpleTankHealth healthTankSelf;
	public Complete.SimpleTankMovement movementTankSelf;
	public Complete.SimpleTankShooting shootingtankSelf;

	public override void CollectObservations() {
		// Ray Perception
		var rayDistance = 12f;
		float[] rayAngles = { 0f, 45f, 90f, 135f, 180f, 110f, 70f };
		var detectableObjects = new[] { "projectile", "Player", "wall" };
		AddVectorObs(m_RayPerception.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
		AddVectorObs(m_RayPerception.Perceive(rayDistance, rayAngles, detectableObjects, 1.5f, 0f));
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
		} else if (forward == 2) {
			movementTankSelf.m_MovementInputValue = -1;
		}

		// Turn
		if (rotation == 1) {
			movementTankSelf.m_TurnInputValue = 1;
		} else if (rotation == 2) {
			movementTankSelf.m_TurnInputValue = -1;
		}

		// Shoot
		if (shoot)
			shootingtankSelf.Fire();
	}
}

