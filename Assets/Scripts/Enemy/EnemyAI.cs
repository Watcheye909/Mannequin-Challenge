using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    public Animator animator;
    public Collider Sphere;
    public NavMeshAgent agent;
    public GameObject mainPlayer;
    public Transform player;
    public Transform attackPoint;
    public LayerMask whatIsGround, whatIsPlayer, Bullet;

    public GameObject playerHitMark;

    //behaviour
    public bool spotted;

    //Enemy States
    public int health;
    public int damage;
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    public bool hurt;
    public bool hit; //USED FOR THE GATE ONLY

    //Attack Stats
    public float timeBetweenShooting, spread, reloadTime, fireRate;
    public int magazineSize;

    int bulletsLeft, bulletsShot;

    public bool readyToShoot, reloading;

    //bullet force
    public float shootForce, upwardForce;

    //bullets
    public GameObject bullet;
    public ProjectileGun PG;
    public ObjectInteract OI;

    //Bug Fixing
    public bool allowInvoke = true;

    private void Awake()
    {
        hit = false;
        player = GameObject.Find("Player").transform;
        mainPlayer = GameObject.Find("Player");
        OI = mainPlayer.GetComponent<ObjectInteract>();
        agent = GetComponent<NavMeshAgent>();
        bulletsLeft = magazineSize;
        readyToShoot = true;
        spotted = false;
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (playerInAttackRange && playerInSightRange || spotted) AttackPlayer();

        //EVERYTHING INVOLVED WITH THE HITMARKER IS NOW SETUP IN THE BulletScript.cs
        if (hurt)
        {
            spotted = true;
            //animator.SetBool("Hit", true);
        }
        //if (!hurt)
            //animator.SetBool("Hit", false);
        
    }


    private void AttackPlayer()
    {
        //this line causes the enemy to not move if they were following the player before
        //agent.SetDestination(transform.position);
        transform.LookAt(player);
        spotted = true;
        //shooting
        if (readyToShoot && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;

            Shoot();
        }

        //reload automatically if ammo runs out
        if (readyToShoot && !reloading && bulletsLeft == 0)
        {
            Reload();
        }
    }





    //...GUN BEHAVIOUR...//








    
    private void Shoot()
    {

        readyToShoot = false;

        //find the exact hit position using a raycast
        Ray ray = new Ray(attackPoint.position, attackPoint.forward);
        RaycastHit hit;

        //check if ray hits something
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75); //just a point far away from the player

        //calculate direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        //calculate spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //calculate new direction with spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        //Instantiate bullet/projectile
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);

        //rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithSpread.normalized;

        //add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(attackPoint.transform.up * upwardForce, ForceMode.Impulse);

        //Instantiate muzzle flash, if you have one
        //if (muzzleFlash != null)
        //    Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }
    }

    private void ResetShot()
    {
        //allow shooting and invoking again
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

    public void takeDamage(int damage)
    {
        health -= damage;
        hurt = true;
        Debug.Log("Hit Enemy");
        if (health <= 0) 
        {
            Invoke("resetDamage", 0.05f);
            Debug.Log("Enemy Got Clapped!!!");
            Invoke(nameof(DestroyEnemy), 0.1f);
        }
        //Invoke("resetDamage", 0.1f);
    }

    //THIS IS NOW DONE IN THE BulletScript.cs
    public void resetDamage()
    {
        hurt = false;
        Debug.Log("reset damage");
    }
    
    private void DestroyEnemy()
    {
        hit = true;
        //resetDamage();
        //Invoke("resetDamage", 0.1f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 11)
        {
            takeDamage(OI.ThrowDamage);
        }
    }
}
