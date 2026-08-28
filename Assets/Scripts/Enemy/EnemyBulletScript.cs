using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    public ProjectileGun PG;
    public EnemyAI EA;

    void OnCollisionEnter(Collision collision)
    {
            Destroy(gameObject);
    }

    /*
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
    */

}
