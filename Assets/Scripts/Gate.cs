using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{

    public Animator animator;
    public GameObject target1;
    public GameObject target2;
    public GameObject target3;

    Target targetCheck1;
    Target targetCheck2;
    Target targetCheck3;

    void Start()
    {
        targetCheck1 = target1.GetComponent<Target>();
        targetCheck2 = target2.GetComponent<Target>();
        targetCheck3 = target3.GetComponent<Target>();
        animator.SetBool("Open", false);
        animator.SetBool("Close", true);
    }
    // Update is called once per frame
    void Update()
    {
        if (targetCheck1.hit && target2 == null && target3 == null)
        {
            animator.SetBool("Open", true);
            animator.SetBool("Close", false);
        }

        else if (targetCheck1.hit && targetCheck2.hit && targetCheck3.hit)
        {
            animator.SetBool("Open", true);
            animator.SetBool("Close", false);
        }
        
        //Destroy(gameObject);

    }

}
