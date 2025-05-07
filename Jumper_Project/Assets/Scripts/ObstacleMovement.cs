using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System.IO.Abstractions;

public class ObstacleMovement : MonoBehaviour
{
    public float speed = 5f;
    public JumpAgent jumpAgent;

    // void Update()
    // {
    //     transform.Translate(Vector3.back * speed * Time.deltaTime);

    // }

    public void Update()
    {
        transform.position += Vector3.back * speed * Time.deltaTime;
        if (transform.position.z <= -5f)
        {   
            Debug.Log("Obstacle reached wall"); 
            if (jumpAgent != null)
            {
                jumpAgent.Success();
            }
        }
    }

    // void OnTriggerEnter(Collider other)
    // {
    //     // if (other.CompareTag("Wall"))
    //     // {
    //     //     Debug.Log("Obstacle hit wall");

    //     //     speed = 0f;

    //     //     if (jumpAgent != null)
    //     //     {
    //     //         jumpAgent.Success();
    //     //     }
    //     // }
    //     if (other.CompareTag("Player"))
    //     {
    //         Debug.Log("Obstacle hit player");

    //         speed = 0f;
    //     }
    // }

    public void SetRandomSpeed(float minSpeed, float maxSpeed)
    {
        speed = Random.Range(minSpeed, maxSpeed);
    }

    // public void ResetPosition(Vector3 startPosition)
    // {
    //     Debug.Log($"Resetting obstacle to {startPosition}");
    //     transform.localPosition = startPosition;
    // }

}
