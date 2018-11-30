using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using System;
using Complete;

public class RLTankAgent : Agent {

    public SimpleGameManager m_GameManager;

    public TankComponents m_Self;
    public TankComponents m_Other;

    float lastOtherHealth;
    float lastSelfHealth;

    float previousDistanceToOther;

    public void Start()
    {
        Setup();
    }

    private void Setup()
    {
        m_GameManager = FindObjectOfType<SimpleGameManager>();

        m_Self = new TankComponents
        {
            m_Health = GetComponent<SimpleTankHealth>(),
            m_Movement = GetComponent<SimpleTankMovement>(),
            m_Shooting = GetComponent<SimpleTankShooting>(),
            m_RayPerception = GetComponent<RayPerception>()
        };

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            if (GameObject.ReferenceEquals(players[i], this.gameObject))
                continue;
            else
            {
                m_Other = new TankComponents
                {
                    m_Health = players[i].GetComponent<SimpleTankHealth>(),
                    m_Movement = players[i].GetComponent<SimpleTankMovement>(),
                    m_Shooting = players[i].GetComponent<SimpleTankShooting>(),
                    m_RayPerception = players[i].GetComponent<RayPerception>()
                };
            }
        }

        // Init values
        lastOtherHealth = m_Other.m_Health.m_StartingHealth;
        lastSelfHealth = m_Self.m_Health.m_StartingHealth;
    }

    public override void AgentReset()
    {
        m_Other.m_Health.Reset();
        m_Other.m_Movement.Reset();
        m_Other.m_Shooting.Reset();

        m_Self.m_Health.Reset();
        m_Self.m_Movement.Reset();
        m_Self.m_Shooting.Reset();
    }

    public List<float> rays;

    public override void CollectObservations()
    {
        // Ray Perception
        //var rayDistance = 50f;
        //float[] rayAngles = { 0f, 45f, 90f, 135f, 180f, 110f, 70f, -180 };
        //var detectableObjects = new[] { "Player", "wall" };
        //rays = m_Self.m_RayPerception.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f);
        //AddVectorObs(rays);

        Vector3 normalizedPosSelf = SimpleGameManager.GetNormalizedPosition(transform);
        AddVectorObs(normalizedPosSelf.x);
        AddVectorObs(normalizedPosSelf.z);

        Vector3 normalizedPosOther = SimpleGameManager.GetNormalizedPosition(m_Other.m_Movement.transform);
        AddVectorObs(normalizedPosSelf.x);
        AddVectorObs(normalizedPosSelf.z);

        // Distance to other
        float distance = Vector3.Distance(normalizedPosSelf, normalizedPosOther);
        AddVectorObs(distance);

        // Rotation
        float rot = transform.rotation.eulerAngles.y;
        float normalizedRot = ((rot % 360) / 360);

        AddVectorObs(normalizedRot);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        if (m_GameManager)
        {
            // Get Movement Action
            int forward = (int)vectorAction[0];
            int rotation = (int)vectorAction[1];
            int shoot = (int)vectorAction[2];

            Inputs(forward, rotation, shoot);

            CalculateRewards(forward, rotation, shoot);

            if (m_Self.m_Health.m_Dead)
                Done();
        }
    }

    private void Inputs(int forward, int rotation, int shoot)
    {
        // Forward
        if (forward == 1)
        {
            m_Self.m_Movement.m_MovementInputValue = 1;
        }
        else if (forward == 2)
        {
            m_Self.m_Movement.m_MovementInputValue = -1;
        }
        else
        {
            m_Self.m_Movement.m_MovementInputValue = 0;
        }

        // Turn
        if (rotation == 1)
        {
            m_Self.m_Movement.m_TurnInputValue = 1;
        }
        else if (rotation == 2)
        {
            m_Self.m_Movement.m_TurnInputValue = -1;
        }
        else
        {
            m_Self.m_Movement.m_TurnInputValue = 0;
        }

        switch(shoot)
        {
            default:
                break;
            case 1:
                m_Self.m_Shooting.Fire(15);
                break;
            case 2:
                m_Self.m_Shooting.Fire(22.5f);
                break;
            case 3:
                m_Self.m_Shooting.Fire(30);
                break;

        }
    }

    public float K;

    private void CalculateRewards(int forward, int rotation, int shoot)
    {
        // Check if close to something
        //var rayDistance = 50f;
        //float[] rayAngles = { 0f, 45f, 90f, 135f, 180f, 110f, 70f, -180 };
        //var detectableObjects = new[] { "Player", "wall" };
        //m_Self.m_RayPerception.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f);

        float distanceToOther = Vector3.Distance(transform.position, m_Other.m_Movement.transform.position);
        float angleToOther = (Vector3.Angle(transform.position, m_Other.m_Movement.transform.position) % 360) / 360;

        if (previousDistanceToOther-distanceToOther > 0.1)
        {
            AddReward(0.2f);
            if (distanceToOther > 0.05f)
                AddReward(0.2f);
        }
        else
            AddReward(-0.05f);

        previousDistanceToOther = distanceToOther;

        // Other lost health or died
        if (m_Other.m_Health.m_CurrentHealth < lastOtherHealth)
        {
            AddReward(0.5f);

            Debug.Log(name + ": OTHER LOST HEALTH");

            if (lastOtherHealth > 0 && m_Other.m_Health.m_CurrentHealth <= 0 || m_Other.m_Health.m_Dead)
            {
                AddReward(1.0f);
            }
        }
        lastOtherHealth = m_Other.m_Health.m_CurrentHealth;

        // Self lost health or died
        if (m_Self.m_Health.m_CurrentHealth < lastSelfHealth)
        {
            AddReward(-0.8f);

            Debug.Log(name + ": I LOST HEALTH");

            if (lastSelfHealth > 0 && m_Self.m_Health.m_CurrentHealth <= 0 || m_Self.m_Health.m_Dead)
            {
                AddReward(-1.0f);
                Done();
            }
        }
        lastSelfHealth = m_Self.m_Health.m_CurrentHealth;

        // Time penalty
        AddReward(-0.05f);
    }
}

