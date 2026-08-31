using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroundChaser : MonoBehaviour
{

    [Header("SETUP")]
    public SpiritGun SG;
    public NavMeshAgent agent;
    public LayerMask whatIsGround, isPlayer;
    public Transform player;
    public Rigidbody rb;
    public PlayerMovement PM;
    public Camera playerCam;
    
    public int bounceStrength;
    public float mainJumpForce;
    public float moveCooldown;
    float moveTime;

    public bool playerInSightRange;
    public bool playerInAttackRange;
    public bool attacking;
    public bool spotted;
    public bool angry;
    bool hit;
    bool hurt;
    bool isBouncing;
    public bool explosive;

    [Header("Patrol")]
    public Vector3 startPoint;
    public Vector3 walkPoint;
    //public Vector3 lastWalkPoint; used to make each movement a certain distance
    public bool walkPointSet;
    public float walkPointRange;

    [Header("STATS")]

    public int health;
    public float damage;
    public float knockbackForce;
    public float sightRange;
    public float attackRange;

    // Start is called before the first frame update
    void Start()
    {
        //locating enemy origin point
        startPoint = transform.position;

        //finding components attatched to the player
        agent = GetComponent<NavMeshAgent>();
        PM = player.GetComponent<PlayerMovement>();
        rb = player.GetComponent<Rigidbody>();
        
        mainJumpForce = PM.jumpForce; 

        //Setting bools to false
        playerInSightRange = false;
        playerInAttackRange = false;
        isBouncing = false;

        angry = false;
        attacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        //DETECTS THE BOUNDS OF THE PLAYER CAMERA
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerCam);

        //CHECKS IF THE MANNEQUIN IS IN VIEW
        if(GeometryUtility.TestPlanesAABB(planes, this.gameObject.GetComponentInChildren<Renderer>().bounds))
            spotted = true;
        else if(!GeometryUtility.TestPlanesAABB(planes, this.gameObject.GetComponentInChildren<Renderer>().bounds))
            spotted = false;


        if(!angry)
        {
            if(spotted)
                agent.isStopped = true;
            else if(!spotted)
                agent.isStopped = false;
        }

        else if(angry)
        {
            agent.speed = 13;
            agent.isStopped = false;
        }




        //DETECTS WHEN THE PLAYER IS IN THE RANGE OF THE MANNEQUIN TO CHASE
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, isPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, isPlayer);

        if(!playerInSightRange) Roam();
        if(playerInSightRange) Chase();
        if(playerInAttackRange) Attack();


        if(isBouncing)
            takeDamage(health);

        //when the player is under a certain amount of health the enemy dies
        if (health <= 0) 
        {
            Invoke("resetDamage", 0.05f);
            Debug.Log("Enemy Got Clapped!!!");
            Invoke(nameof(DestroyEnemy), 0.1f);
        }

    }

    private void Roam()
    {

        if(!walkPointSet)
        {
            SearchWalkPoint();
        } 

        if(walkPointSet)
        {
            agent.SetDestination(walkPoint);
            moveTime -= Time.deltaTime;
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 0.2f || moveTime <= 0f)
        {
            walkPointSet = false;
            //lastWalkPoint = walkPoint;
        }
    }

    void Attack()
    {
        if(!angry)
        {
            attacking = false;
        }
            //SG.bulletsLeft = 0;

        if(angry)
        {
            //transform.LookAt(player);
            attacking = true;
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        /*
        if(randomZ - lastWalkPoint.z < 2)
            randomZ = Random.Range(-walkPointRange, walkPointRange);

        if(randomX - lastWalkPoint.x < 2)
            randomX = Random.Range(-walkPointRange, walkPointRange);
        */
        
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if(walkPoint.x > startPoint.x + walkPointRange || walkPoint.z > startPoint.z + walkPointRange)
        {
            walkPoint = startPoint;
        }

        if (Physics.Raycast(walkPoint, -transform.up, whatIsGround))
        {
            walkPointSet = true;
            moveTime = moveCooldown;
        }
    }

    private void Chase()
    {
        agent.SetDestination(player.position);
    }

    public void takeDamage(int damage)
    {
        health -= damage;
        hurt = true;
        Debug.Log("Hit Enemy");
        
        //Invoke("resetDamage", 0.1f);
    }

    private void DestroyEnemy()
    {
        hit = true;
        //resetDamage();
        //Invoke("resetDamage", 0.1f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.layer == 3)
        {
            //playerInSightRange = true;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            PM.jumpForce = bounceStrength;
            PM.Jump();
            PM.jumpForce = mainJumpForce;
            //takeDamage(PG.damage);
            isBouncing = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.layer == 3)
        {
            isBouncing  = false;
        }
    }
    

    private void OnCollisionEnter(Collision collision)
    {
        /* [HANDLED IN TriggerEnter INSTEAD]
        if(collision.gameObject.layer == 3)
        {
            //playerInSightRange = true;
            PM.jumpForce = bounceStrength;
            PM.Jump();
            PM.jumpForce = mainJumpForce;
        }
        */

        if (collision.gameObject.layer == 10)
        {
            takeDamage(SG.damage);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        /*
        if(collision.gameObject.layer == 3)
        {
            playerInSightRange = false;
        }
        */
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
