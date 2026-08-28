using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ViewDirectionSystem : MonoBehaviour
{
    public GameObject spirit;
    public GroundChaser GC;
    public Rigidbody spiritBody;
    public UnityEngine.AI.NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.layer == 9)
        {
            GC.spotted = true;
        }
    }
    void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.layer == 9)
        {
            GC.spotted = false;
        }
    }
}
