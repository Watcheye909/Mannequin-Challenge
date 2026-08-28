using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritOrb : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public GameObject playerHitMark;
    public GameObject mainPlayer;
    public Transform player;
    public Rigidbody rb;
    public PlayerCamera cam;
    //public ProjectileGun PG;
    //public EnemyAI EA;


    [Tooltip("Optional override. If empty, SpiritOrb will try to find the player by tag 'Player'.")]
    public string playerTag = "Player";

    [Header("Object Properties")]
    public GameObject impactEffect;
    public bool isThrown;


    //EXPLOSION PROPERTIES
    [Header("Explosion Properties")]
    public bool isExplosive;
    public float explosionRadius;
    public float explosionForce;
    public int explosionDamage;
    public float camShakeAmount;
    public GameObject explosionEffect;


    public bool hurtEnemy;
    public bool allowInvoke = true;


    void Start()
    {
        if (mainPlayer == null)
        {
            mainPlayer = GameObject.FindWithTag(playerTag);
        }

        if (mainPlayer != null)
        {
            rb = mainPlayer.GetComponent<Rigidbody>();
            if (rb == null)
            {
                Debug.LogWarning("SpiritOrb: Player object found but has no Rigidbody.");
            }
        }
        else
        {
            Debug.LogWarning("SpiritOrb: Could not find player object. Assign mainPlayer in the inspector or tag it as 'Player'.");
        }

        cam = GameObject.Find("Main Camera").GetComponent<PlayerCamera>();

        isExplosive = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(impactEffect != null)
        {
            GameObject effect = Instantiate(impactEffect, transform.position, transform.rotation);
            transform.LookAt(player);
            Destroy(effect, 0.9f);
            
        }

        if (collision.gameObject.layer == 9)
        {
            hurtEnemy = true;
            Invoke("resetDamage", 0.01f);
        }

        /* if the orb hits the ground or an enemy, it becomes explosive and explodes after a short delay
        if (isExplosive)
        {
            Explode();
            return;
        }
        */
    
        /*
        else
            Destroy(gameObject);
        */
    }
    


    /*
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
    */

    public void resetDamage()
    {
        hurtEnemy = false;
        Debug.Log("reset damage");
        Destroy(gameObject);
    }

    void Update()
    {
        /*
        if (hurtEnemy)
            animator.SetBool("Hit", true);

        if (!hurtEnemy)
            animator.SetBool("Hit", false);
        */


        if(Input.GetKeyDown(KeyCode.Mouse1) && isThrown)
        {
            Explode();
        }
    }


    public void Explode()
    {
        // spawn explosion effect (if assigned)
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // find all the objects that are inside the explosion range
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, explosionRadius);

        // loop through all of the found objects and apply damage and explosion force
        Rigidbody selfRb = GetComponent<Rigidbody>();
        for (int i = 0; i < objectsInRange.Length; i++)
        {
            Collider hitCollider = objectsInRange[i];
            Rigidbody hitRb = hitCollider.attachedRigidbody;
            Rigidbody playerRB = mainPlayer.GetComponent<Rigidbody>();


            if (hitRb == null || hitRb == selfRb || hitCollider.isTrigger)
                continue;
            

            /*
            Vector3 closestPoint = hitCollider.ClosestPoint(transform.position);
            float distance = Vector3.Distance(transform.position, closestPoint);
            if (distance > explosionRadius)
                continue;

            Vector3 forceDirection = (closestPoint - transform.position).normalized;
            if (forceDirection == Vector3.zero)
                forceDirection = Vector3.up;

            float distanceFactor = 1f - (distance / explosionRadius);
            hitRb.AddForceAtPosition((forceDirection + Vector3.up) * explosionForce * distanceFactor,
                closestPoint, ForceMode.Impulse);
            */
            
            if(hitRb != playerRB)
            {
                cam.DoShake(camShakeAmount/3, 0.5f);
                Debug.Log("small shake");
            }

            else
            {
                cam.DoShake(camShakeAmount, 0.5f);
                Debug.Log("full shake");
            }
            
            
            // custom explosionForce
            Vector3 objectPos = hitCollider.transform.position;

            // calculate force direction
            Vector3 forceDirection = (objectPos - transform.position).normalized;

            // apply force to object in range
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            hitRb.AddForceAtPosition(forceDirection * explosionForce + Vector3.up * explosionForce, 
                transform.position + new Vector3(0, -0.5f, 0), ForceMode.Impulse);
            

            Debug.Log("Kabooom " + hitCollider.name);
        }

        // destroy projectile with 0.1 seconds delay
        Invoke(nameof(DestroyProjectile), 0.1f);
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    // just graphics stuff
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
