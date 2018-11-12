using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class TankAgent : Agent {

    public TankManager m_OtherTankManager;
    public TankManager m_SelfTankManager;
    public GameManager m_GameManager;
    public RayPerception m_RayPerception;

    public void Start()
    {
        // Get Ray Perception
        m_RayPerception = GetComponent<RayPerception>();

        // Get Game Manager
        m_GameManager = FindObjectOfType<GameManager>();

        // Get Tank Manangers
        int myPlayerNumber = GetComponent<TankMovement>().m_PlayerNumber;
        for(int i = 0; i < m_GameManager.m_Tanks.Length;i++)
        {
            TankManager tankManager = m_GameManager.m_Tanks[i];
            if (myPlayerNumber == tankManager.m_PlayerNumber)
                m_SelfTankManager = tankManager;
            else
                m_OtherTankManager = tankManager;
        }

        // Get Brain
        Brain b = FindObjectOfType<Brain>();

        Debug.Log(name + " found brain: " + b.name);

        GiveBrain(b);
    }

    public override void AgentReset()
    {

    }

    public override void CollectObservations()
    {
        var rayDistance = 12f;
        float[] rayAngles = { 0f, 45f, 90f, 135f, 180f, 110f, 70f };
        var detectableObjects = new[] { "projectile", "Player", "wall" };
        AddVectorObs(m_RayPerception.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
        AddVectorObs(m_RayPerception.Perceive(rayDistance, rayAngles, detectableObjects, 1.5f, 0f));
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        
    }
}

