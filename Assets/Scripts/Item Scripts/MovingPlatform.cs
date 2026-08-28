using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public Transform pointC;
    public Transform pointD;
    public bool Loop;
    public float speed = 2f;
    public bool canBoost = false;
    private Vector3 lastPosition;
    private Vector3 platformVelocity;
    public Vector3 boostVelocity;
    private List<Transform> waypoints;
    public int currentWaypointIndex = 0;
    public Rigidbody playerRb;
    private bool isPlayerOnPlatform;
    private Vector3 platformDisplacement;
    void Start()
    {
        lastPosition = transform.position;
        waypoints = new List<Transform>();
        if (pointA != null) waypoints.Add(pointA);
        if (pointB != null) waypoints.Add(pointB);
        if (pointC != null) waypoints.Add(pointC);
        if (pointD != null) waypoints.Add(pointD);
    }
    void FixedUpdate()
    {
        if (waypoints.Count == 0) return;

        // Calculate displacement before moving
        Vector3 positionBeforeMove = transform.position;

        Transform target = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.fixedDeltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.01f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Count)
            {
                if (Loop)
                    currentWaypointIndex = 0;
                else
                    currentWaypointIndex = waypoints.Count - 1; // Stay at last point
            }
        }

        // Calculate displacement and velocity
        platformDisplacement = transform.position - positionBeforeMove;
        platformVelocity = platformDisplacement / Time.fixedDeltaTime;

        // Apply platform displacement to player if on platform
        if (isPlayerOnPlatform && playerRb != null)
        {
            playerRb.position += platformDisplacement;
        }

        boostVelocity = platformVelocity;
        boostVelocity.x *= 5;
        boostVelocity.z *= 5;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerRb = other.gameObject.GetComponent<Rigidbody>();
            isPlayerOnPlatform = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerRb.AddForce(platformVelocity, ForceMode.VelocityChange); // Apply boost velocity to player
            isPlayerOnPlatform = false;
            playerRb = null;
        }
    }

/*
    private void ResetBoost()
    {
        isPlayerOnPlatform = false;
        canBoost = false;
        playerRb = null;
    }

    private void applyBoost()
    {
        if (canBoost && playerRb != null)
        {
            playerRb.AddForce(boostVelocity, ForceMode.VelocityChange);
            Invoke(nameof(ResetBoost), 1f); // Reset boost after a short delay
        }
    }
*/
    public Vector3 GetVelocity() => platformVelocity;
}
