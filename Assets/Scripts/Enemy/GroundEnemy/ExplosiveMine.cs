using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;

public class ExplosiveMine : MonoBehaviour
{
    public GroundChaser GC;
    public NavMeshAgent agent;
    public LayerMask isPlayer;



    public bool playerInDetectRange;
    public bool playerInExplosiveRange;
    public float detectRange;
    public float explosiveRange;
    public float slowSpeed;
    private float baseSpeed;

    // Start is called before the first frame update
    void Start()
    {
        agent = GC.GetComponent<NavMeshAgent>();
        baseSpeed = agent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        playerInDetectRange = Physics.CheckSphere(transform.position, detectRange, isPlayer);
        
        if(!playerInDetectRange) agent.speed = baseSpeed;
        if(playerInDetectRange) agent.speed = slowSpeed;
    }

/*
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.layer == 3)
        {
            agent.speed = 2;
        }
    }
*/

    private void Explosion()
    {
        playerInExplosiveRange = Physics.CheckSphere(transform.position, explosiveRange, isPlayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosiveRange);
    }
}
