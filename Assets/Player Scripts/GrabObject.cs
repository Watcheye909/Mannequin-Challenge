using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class GrabObject : MonoBehaviour
{
    
    public Transform objTransform, cameraTrans;
    public bool interactable;
    public bool holding;
    public Rigidbody rb;
    public float throwAmount;
    Vector3 md;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            interactable = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            if(holding == false)
            {
                interactable = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(interactable == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                objTransform.parent = cameraTrans;
                rb.useGravity = false;
                holding = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                objTransform.parent = null;
                rb.useGravity = true;
                holding = false;
            }
            if(holding == true)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    objTransform.parent = null;
                    rb.useGravity = true;
                    rb.velocity = new Vector3 (cameraTrans.forward = throwAmount = Time.deltaTime);
                    holding = false;
                }
            }
        }
    }
}
*/