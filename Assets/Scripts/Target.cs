using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{

    public Animator animator;
    public LayerMask Bullet;
    public bool hit;

    void Start()
    {
        hit = false;
        animator.SetBool("Hit", false);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10)
        {
            hit = true;
            animator.SetBool("Hit", true);
        }
            
    }
}
