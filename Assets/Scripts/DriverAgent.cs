using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.Rendering.Universal;
using UnityEditor;
using UnityEngine;

public class DriverAgent : Agent
{
    [SerializeField] PrometeoCarController carController;
    [SerializeField] Rigidbody rigidBody;
    [SerializeField] Transform target;
    [SerializeField] MeshRenderer indicator;

    public override void Initialize()
    {
        carController = GetComponent<PrometeoCarController>();
    }
    public override void OnEpisodeBegin()
    {

        carController.ResetSteeringAngle();
        carController.RecoverTraction();
        carController.ThrottleOff();

        transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        transform.localPosition = Vector3.zero;
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;

        ResetTarget();
    }
    // Reset Target Position
    public void ResetTarget()
    {
        target.localPosition = new Vector3(Random.Range(-30, 30), 0, Random.Range(-30, 30));
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        var bearing = Vector3.SignedAngle(transform.forward, target.localPosition - transform.localPosition, Vector3.up) / 180f;
        bearing = 1 - Mathf.Abs(bearing);
        sensor.AddObservation(bearing);
        sensor.AddObservation(carController.carSpeed / carController.maxSpeed);
        sensor.AddObservation(carController.frontLeftCollider.steerAngle / carController.maxSteeringAngle);
    }
    // Use Discrete Actions to control the car, Methods to control the car are
    // GoForward, GoBackward, TurnLeft, TurnRight, HandBrake, RecoverTraction
    // Use multiple Discrete Actions to use actions that dont interfere with each other
    public override void OnActionReceived(ActionBuffers actions)
    {
        var action = actions.DiscreteActions;

        switch (action[0])
        {
            case 0:
                carController.ThrottleOff();
                break;
            case 1:
                carController.GoForward();
                break;
            case 2:
                carController.GoReverse();
                break;
        }
        switch (action[1])
        {
            case 0:
                carController.ResetSteeringAngle();
                break;
            case 1:
                carController.TurnLeft();
                break;
            case 2:
                carController.TurnRight();
                break;
        }
        AddReward(-0.001f);
        AddReward(0.00001f * carController.carSpeed / carController.maxSpeed);
        // punish for losing traction
        if (carController.isDrifting)
        {
            AddReward(-0.01f);
        }
        // punish for quick turns using the angular velocity
        AddReward(-0.001f * rigidBody.angularVelocity.magnitude);
        // Reward for moving in the target direction using Rigidbody velocity
        /*AddReward(Vector3.Dot(rigidBody.velocity.normalized, (target.localPosition - transform.localPosition).normalized) / 1000);


        var bearing = Vector3.SignedAngle(transform.forward, target.localPosition - transform.localPosition, Vector3.up) / 180f;
        bearing = 1 - Mathf.Abs(bearing);
        AddReward(bearing / 50000);*/
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            // Set the indicator to a light green color
            indicator.material.color = new Color(0, 50, 0);
            AddReward(10f);
            ResetTarget();
            //EndEpisode();
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            // Set the indicator to a light red color
            indicator.material.color = new Color(50, 0, 0);
            SetReward(-2f);
            EndEpisode();
        }
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actions = actionsOut.DiscreteActions;
        actions[0] = 0;
        if (Input.GetKey(KeyCode.W))
        {
            actions[0] = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            actions[0] = 2;
        }
        actions[1] = 0;
        if (Input.GetKey(KeyCode.A))
        {
            actions[1] = 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            actions[1] = 2;
        }
    }
}
