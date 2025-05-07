using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System.IO.Abstractions;

public class JumpAgent : Agent
{
    // [SerializeField]
    // private Transform targetPosition;

    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private GameObject coinPrefab;
    private ObstacleMovement obstacle;

    public float jumpForce = 5f;
    private Rigidbody rb;
    //private bool jumped_over = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //obstacle.jumpAgent = this; // Link obstacle met agent
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnEpisodeBegin()
    {
        Debug.Log("Episode gestart!");

        InitializeEnvironment();
    }

    private void InitializeEnvironment(){
        transform.localPosition = new Vector3(0, 0.55f, 0);
        transform.localRotation = Quaternion.identity;
        rb.linearVelocity = Vector3.zero;

        if (obstacle != null)
        {
            Debug.Log($"Destroying obstacle: {obstacle.gameObject.name}");
            Destroy(obstacle.gameObject);
        }

        GameObject randomPrefab = (Random.value < 0.5f) ? obstaclePrefab : coinPrefab; // if < 0.5f true spawn obstacle else spawn coin

        GameObject newObstacleGO = Instantiate(randomPrefab, new Vector3(0f, 0.25f, 9.75f), Quaternion.identity);
        obstacle = newObstacleGO.GetComponent<ObstacleMovement>();
        obstacle.jumpAgent = this;
        obstacle.SetRandomSpeed(3f, 8f);

        //jumped_over = false;
    }

    public void Success()
    {
        SetReward(1f);
        EndEpisode();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Player hit obstacle");

            SetReward(-1f);
            EndEpisode();

            // GameObject[] objects = GameObject.FindGameObjectsWithTag("Enemy");
            // foreach(GameObject obj in objects)
            // {
            //     Destroy(obj);
            // } // Meer voor als er meerdere obstacles zijn.
        }
        else if (other.CompareTag("Coin"))
        {
            Debug.Log("Player hit coin");

            SetReward(1f);
            EndEpisode();
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    { 
        sensor.AddObservation(transform.localPosition);
    }


    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        float jumpAction = actionBuffers.ContinuousActions[0];

        if (jumpAction > 0.5f && Mathf.Abs(rb.linearVelocity.y) < 0.01f) // Alleen springen als bijna geen verticale snelheid / Geen double jump
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

    }    

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //Debug.Log("Heuristic mode actief!");

        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetKey(KeyCode.Space) ? 1f : 0f; // if space pressed true = 1f else 0f
    }

}



