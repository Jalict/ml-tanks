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
    public Transform m_SpawnPoint;

    float lastOtherHealth;
    float lastSelfHealth;

    public void Start()
    {
        Setup();

        // Get Brain
        Brain brain = FindObjectOfType<Brain>();
        GiveBrain(brain);

        m_GameManager.roundStarted = true;
    }

    private void Setup()
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
        m_OtherHealth = m_OtherTankManager.m_Movement.gameObject.GetComponent<Complete.TankHealth>();
        m_SelfHealth = m_SelfTankManager.m_Movement.gameObject.GetComponent<Complete.TankHealth>();

        //SpawnPoint
        m_SpawnPoint = m_SelfTankManager.m_SpawnPoint;

        // Init values
        lastOtherHealth = m_OtherHealth.m_StartingHealth;
        lastSelfHealth = m_SelfHealth.m_StartingHealth;
    }

    public override void AgentReset()
    {
        StartCoroutine(GetReferences());
    }     
    
    IEnumerator GetReferences()
    {
        yield return new WaitUntil(
            () =>
            m_OtherHealth != null &&
            m_SelfHealth != null &&
            GetComponent<Complete.TankHealth>() != null
            );

        // Init values
        lastOtherHealth = m_OtherHealth.m_StartingHealth;
        lastSelfHealth = m_SelfHealth.m_StartingHealth;

        // Set Transform
        transform.position = m_SpawnPoint.position;
        transform.rotation = m_SpawnPoint.rotation;

        // Resets health
        gameObject.GetComponent<Complete.TankHealth>().enabled = false;
        gameObject.GetComponent<Complete.TankHealth>().enabled = true;
    }

    public override void CollectObservations()
    {
        // Ray Perception
        var rayDistance = 12f;
        float[] rayAngles = { 0f, 45f, 90f, 135f, 180f, 110f, 70f };
        var detectableObjects = new[] { "projectile", "Player", "wall" };
        AddVectorObs(m_RayPerception.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
        AddVectorObs(m_RayPerception.Perceive(rayDistance, rayAngles, detectableObjects, 1.5f, 0f));       
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        if (m_GameManager.roundStarted && m_GameManager)
        {
            Inputs(vectorAction);

            CalculateRewards();
        }
    }

    private void Inputs(float[] vectorAction)
    {
        // Get Movement Action
        int forward = (int)vectorAction[0];
        int rotation = (int)vectorAction[1];
        bool shoot = vectorAction[2] == 1;

        // Forward
        if (forward == 1)
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

    private void CalculateRewards()
    {
        // Other lost health or died
        if (m_OtherHealth.m_CurrentHealth < lastOtherHealth)
        {
            AddReward(1.0f);

            if (lastOtherHealth > 0 && m_OtherHealth.m_CurrentHealth <= 0)
            {
                AddReward(1.0f);
                Done();
            }
        }
        lastOtherHealth = m_OtherHealth.m_CurrentHealth;

        // Self lost health or died
        if (m_SelfHealth.m_CurrentHealth < lastSelfHealth)
        {
            AddReward(-1.0f);

            if (lastSelfHealth > 0 && m_SelfHealth.m_CurrentHealth <= 0)
            {
                AddReward(-1.0f);
            }
        }
        lastSelfHealth = m_SelfHealth.m_CurrentHealth;

        // Time penalty
        AddReward(-0.05f);
    }
}

