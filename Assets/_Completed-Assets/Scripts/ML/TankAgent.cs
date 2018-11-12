using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class TankAgent : Agent {

    public Complete.TankHealth m_OtherHealth;
    public Complete.TankHealth m_SelfHealth;
    public Complete.TankManager m_OtherTankManager;
    public Complete.TankManager m_SelfTankManager;
    public Complete.GameManager m_GameManager;
    public RayPerception m_RayPerception;

    public void Start()
    {
        // Get Ray Perception
        m_RayPerception = GetComponent<RayPerception>();

        // Get Game Manager
        m_GameManager = FindObjectOfType<Complete.GameManager>();

        // Get Tank Manangers
        int myPlayerNumber = GetComponent<Complete.TankMovement>().m_PlayerNumber;
        for (int i = 0; i < m_GameManager.m_Tanks.Length; i++)
        {
            Complete.TankManager tankManager = m_GameManager.m_Tanks[i];
            if (myPlayerNumber == tankManager.m_PlayerNumber)
                m_SelfTankManager = tankManager;
            else
                m_OtherTankManager = tankManager;
        }

        // Health
        m_OtherHealth = m_OtherTankManager.m_Movement.GetComponent<Complete.TankHealth>();
        m_OtherHealth = m_SelfTankManager.m_Movement.GetComponent<Complete.TankHealth>();

        // Get Brain
        Brain[] brains = FindObjectsOfType<Brain>();
        for(int i = 0; i < brains.Length;i++)
        {
            if (brains[i].gameObject.name.Contains(m_SelfTankManager.m_PlayerNumber.ToString()))
                GiveBrain(brains[i]);
        }
    }

    public override void AgentReset()
    {

    }

    float lastOtherHealth;
    float lastSelfHealth;
        

    public override void CollectObservations()
    {
        // Ray Perception
        var rayDistance = 12f;
        float[] rayAngles = { 0f, 45f, 90f, 135f, 180f, 110f, 70f };
        var detectableObjects = new[] { "projectile", "Player", "wall" };
        AddVectorObs(m_RayPerception.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
        AddVectorObs(m_RayPerception.Perceive(rayDistance, rayAngles, detectableObjects, 1.5f, 0f));

        // Other lost health or died
        if(lastOtherHealth < m_OtherHealth.m_CurrentHealth)
        {
            AddReward(1.0f);

            if (m_OtherHealth.m_Dead)
                AddReward(1.0f);
        }
        lastOtherHealth = m_OtherHealth.m_CurrentHealth;

        // Self lost health or died
        if (lastSelfHealth < m_SelfHealth.m_CurrentHealth)
        {
            AddReward(-1.0f);

            if (m_SelfHealth.m_Dead)
                AddReward(-1.0f);
        }
        lastSelfHealth = m_SelfHealth.m_CurrentHealth;

        // Time penalty
        AddReward(-0.05f);
        
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        // Get Movement Action
        int forward = (int)vectorAction[0];
        int rotation = (int)vectorAction[1];
        bool shoot = vectorAction[2] == 1; 

        // Forward
        if(forward == 1)
        {
            m_SelfTankManager.m_Movement.m_MovementInputValue = 1;
        }
        else if (forward == 2)
        {
            m_SelfTankManager.m_Movement.m_MovementInputValue = -1;
        }

        // Turn
        if (rotation == 1)
        {
            m_SelfTankManager.m_Movement.m_TurnInputValue = 1;
        }
        else if (rotation == 2)
        {
            m_SelfTankManager.m_Movement.m_TurnInputValue = -1;
        }

        // Shoot
        if (shoot)
            m_SelfTankManager.m_Shooting.Fire();
    }
}

