using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveObject : MonoBehaviour
{
    public bool isThrown = false;

    // EXPLOSION PROPERTIES
    public float explosionRadius = 5f;
    public float explosionForce = 10f;
    public int explosionDamage = 50;
    public GameObject explosionEffect;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && isThrown)
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

            if (hitRb == null || hitRb == selfRb || hitCollider.isTrigger)
                continue;

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