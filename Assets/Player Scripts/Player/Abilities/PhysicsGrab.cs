using System.Collections;
using System.Collections.Generic;
using cakeslice;
using UnityEngine;

public class PhysicsGrab : MonoBehaviour
{

    public Animator animator;
    public GameObject weapon = null;
    public GameObject crosshair2;
    [SerializeField] private LayerMask PickupMask;
    [SerializeField] private Camera PlayerCamera;
    [SerializeField] private Transform PickupTarget;
    [Space]
    [SerializeField] private float PickupRange;
    public Rigidbody foundObject;
    private Rigidbody CurrentObject;
    public Outline OL;

    public bool isGrabbing;

    // Start is called before the first frame update
    void Start()
    {
        //crosshair2.SetActive(false);
        animator.SetBool("Grabbable", false);
        animator.SetBool("Grabbed", false);

    }
    
    // Update is called once per frame
    void Update()
    {
        
        if(isGrabbing)
            weapon.SetActive(false);
        else
            weapon.SetActive(true);


        Ray CameraRay = PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(CameraRay, out RaycastHit objHit, PickupRange, PickupMask))
        {
            //[OUTLINE]
            //foundObject = objHit.rigidbody;
            //OL = foundObject.GetComponent<Outline>();
            //OL.eraseRenderer = false; //Keep the outline active

            crosshair2.SetActive(true);
            animator.SetBool("Grabbable", true);
            if (CurrentObject) // [IF THE PLAYER IS CURRENTLY GRABBING THE BLOCK CHANGE THE CROSSHAIR TO THE GRABBED ICON]
            {
                //[OUTLINE]
                //foundObject = CurrentObject;
                //OL.eraseRenderer = true;   


                animator.SetBool("Grabbable", false);
                animator.SetBool("Grabbed", true);
            }
        }
        else
        {
            //weapon.SetActive(true);
            animator.SetBool("Grabbable", false);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (CurrentObject)
            {
                //[OUTLINE]
                OL = CurrentObject.GetComponent<Outline>();
                OL.eraseRenderer = false;

                CurrentObject.useGravity = true;
                CurrentObject = null;
                animator.SetBool("Grabbed", false);
                isGrabbing = false;

                //weapon.SetActive(true);

                return;
            }
            //Ray CameraRay = PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            if (Physics.Raycast(CameraRay, out RaycastHit HitInfo, PickupRange, PickupMask))
            {
                //OL = CurrentObject.GetComponent<Outline>();
                CurrentObject = HitInfo.rigidbody;
                CurrentObject.useGravity = false;
                //crosshair2.SetActive(false);
                animator.SetBool("Grabbed", true);
                isGrabbing = true;

                //[OUTLINE]
                OL = CurrentObject.GetComponent<Outline>();
                OL.eraseRenderer = true;
                //weapon.SetActive(false);
            }
        }
    }

    void FixedUpdate()
    {
        if (CurrentObject)
        {
            Vector3 DirectionToPoint = PickupTarget.position - CurrentObject.position;
            float DistanceToPoint = DirectionToPoint.magnitude;

            CurrentObject.velocity = DirectionToPoint * 12f * DistanceToPoint; 
        }
    }

//if the player makes contact with the grabbed item they let go of it
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8)
        {
            if (collision.rigidbody == CurrentObject)
            {
                //[OUTLINE]
                OL = CurrentObject.GetComponent<Outline>();
                OL.eraseRenderer = false;

                CurrentObject.useGravity = true;
                CurrentObject = null;
                animator.SetBool("Grabbed", false);
                isGrabbing = false;
                //weapon.SetActive(true);

            }
        }
        

    }
}
