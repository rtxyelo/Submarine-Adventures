using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;


/// <summary>
/// Represents the behavior of an enemy agent for Machine Learning (ML) movement type. (NO ACTUAL!)
/// </summary>
public class AgentEnemyBehaviour : Agent
{
    // Movement speeds
    private float m_LateralSpeed = 0.05f;
    private float m_ForwardSpeed = 0.15f;

    [SerializeField]
    private PlayerSpawner _playerSpawner;

    [HideInInspector]
    public Rigidbody agentRb;

    private Vector3 m_initialEnemyPosition;

    /// <summary>
    /// Initializes the enemy agent.
    /// </summary>
    public override void Initialize()
    {
        agentRb = GetComponent<Rigidbody>();
        agentRb.maxAngularVelocity = 50;
        agentRb.freezeRotation = true;
        m_initialEnemyPosition = transform.position;
    }

    /// <summary>
    /// Moves the agent based on the received action.
    /// </summary>
    /// <param name="act">Action containing discrete values for movement.</param>
    public void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var forwardAxis = act[0];
        var rightAxis = act[1];
        var rotateAxis = act[2];

        switch (forwardAxis)
        {
            case 1:
                dirToGo = transform.forward * m_ForwardSpeed;
                break;
            case 2:
                dirToGo = transform.forward * -m_ForwardSpeed;
                break;
        }

        switch (rightAxis)
        {
            case 1:
                dirToGo = transform.right * m_LateralSpeed;
                break;
            case 2:
                dirToGo = transform.right * -m_LateralSpeed;
                break;
        }

        switch (rotateAxis)
        {
            case 1:
                rotateDir = transform.up * -1f;
                break;
            case 2:
                rotateDir = transform.up * 1f;
                break;
        }

        transform.Rotate(rotateDir, Time.deltaTime * 30f);
        agentRb.AddForce(dirToGo, ForceMode.VelocityChange);
    }

    /// <summary>
    /// Receives actions and calls MoveAgent to perform the corresponding movement.
    /// </summary>
    /// <param name="actionBuffers">ActionBuffers containing the actions to be performed.</param>
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        MoveAgent(actionBuffers.DiscreteActions);
    }

    /// <summary>
    /// Provides a heuristic for manual control of the agent.
    /// </summary>
    /// <param name="actionsOut">ActionBuffers for setting discrete actions based on user input.</param>
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;

        // Forward
        if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = 2;
        }

        // Rotate
        if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[2] = 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[2] = 2;
        }

        // Right
        if (Input.GetKey(KeyCode.E))
        {
            discreteActionsOut[1] = 1;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            discreteActionsOut[1] = 2;
        }
    }

    /// <summary>
    /// Called when a new episode begins. Resets the agent's state.
    /// </summary>
    public override void OnEpisodeBegin()
    {
        SetReward(0f);
        _playerSpawner.RespawnPlayer();
        transform.position = m_initialEnemyPosition;
    }

    /// <summary>
    /// Handles collision events with other objects.
    /// </summary>
    /// <param name="collision">Collision data.</param>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SetReward(1f);
            EndEpisode();
            Debug.Log("Bite!");
        }

        if (collision.gameObject.CompareTag("Rock"))
        {
            AddReward(-0.005f);
            Debug.Log("Collide with rock!");
        }

        if (collision.gameObject.CompareTag("AgentTrainWalls"))
        {
            AddReward(-0.003f);
            Debug.Log("Collide with wall!!!");
        }
    }

    /// <summary>
    /// Performs actions during the FixedUpdate phase.
    /// </summary>
    void FixedUpdate()
    {
        AddReward(-0.0001f);
    }
}
