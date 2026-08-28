using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    public LayerMask Bullet;
    public bool hit;

    void Start()
    {
        hit = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10)
        {
            hit = true;
            Destroy(gameObject, 3f);
        }

    }
}
