using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    public Animator animator;
    public float minblinktime, maxblinktime;
    bool looping = true;
    private void Start()
    {
        StartCoroutine(Blinking());



    }
    IEnumerator Blinking()
    {
        
        while (looping == true)
        {
            yield return new WaitForSeconds(Random.Range(minblinktime, maxblinktime));
            animator.SetTrigger("Blinked");

        }

    }


}
