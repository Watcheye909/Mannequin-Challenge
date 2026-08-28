using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class NewBullet : MonoBehaviour
{
    public Animator animator;
    public GameObject impactEffect;
    public Transform player;
    //public Camera fpsCam;
    //public Transform attackPoint;
    public ProjectileGun PG;

    Rigidbody rb;


    public float speed;
    public bool hurtEnemy;
    public bool allowInvoke = true;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hurtEnemy)
            animator.SetBool("Hit", true);

        if (!hurtEnemy)
            animator.SetBool("Hit", false);
        //shoot();
        //rb.MovePosition(speed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        shoot();
    }

    void shoot()
    {
        /*

        //find the exact hit position using a raycast
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        //check if ray hits something
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75); //just a point far away from the player

        
        //calculate direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        */
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);
        direction.Normalize();
        Vector3 velocity = direction * speed;
        rb.MovePosition(rb.position + velocity * Time.deltaTime);
        
    }
}
