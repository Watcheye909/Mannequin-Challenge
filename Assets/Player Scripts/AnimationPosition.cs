using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPosition : MonoBehaviour
{

    public ProjectileGun PG;
    public Transform AnimationPoint;

    // Update is called once per frame
    void Update()
    {
        transform.position = AnimationPoint.position;
    }
}
