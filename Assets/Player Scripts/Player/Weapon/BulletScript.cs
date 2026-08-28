using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public Animator animator;
    public GameObject playerHitMark;
    public ProjectileGun PG;
    public EnemyAI EA;

    public GameObject impactEffect;
    public Transform player;

    public bool hurtEnemy;
    public bool allowInvoke = true;

    void OnCollisionEnter(Collision collision)
    {
        GameObject effect = Instantiate(impactEffect, transform.position, transform.rotation);
        transform.LookAt(player);
        Destroy(effect, 0.9f);

        if (collision.gameObject.layer == 9)
        {
            hurtEnemy = true;
            Invoke("resetDamage", 0.01f);
        }

        else
            Destroy(gameObject);
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
        if (hurtEnemy)
            animator.SetBool("Hit", true);

        if (!hurtEnemy)
            animator.SetBool("Hit", false);
    }
}
