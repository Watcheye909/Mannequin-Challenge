using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGate : MonoBehaviour
{

    public Animator animator;
    public GameObject target4;

    EnemyAI targetCheck4;

    void Start()
    {
        targetCheck4 = target4.GetComponent<EnemyAI>();
        animator.SetBool("Open", true);
        animator.SetBool("Close", false);
    }
    // Update is called once per frame
    void Update()
    {
        if (targetCheck4.spotted)
        {
            animator.SetBool("Open", false);
            animator.SetBool("Close", true);
        }
        /*
        //--> Enemy triggering gate <--
        if (!targetCheck4.hit)
        {
            animator.SetBool("Open", false);
            animator.SetBool("Close", true);
        }
        */
        if (targetCheck4.hit)
        {
            animator.SetBool("Open", true);
            animator.SetBool("Close", false);
        }
    }

}
