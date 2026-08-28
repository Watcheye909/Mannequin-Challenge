using System.Collections;
using System.Collections.Generic;
using cakeslice;
using UnityEngine;
using UnityEngine.Analytics;

public class ObjectInteract : MonoBehaviour
{

    //GRAB PROPERTIES
    [Header("Crosshair")]
    public Animator animator;
    public GameObject weapon = null;
    public GameObject crosshair2;

    [Header("Grab Properties")]
    [SerializeField] private LayerMask PickupMask;
    [SerializeField] private Camera PlayerCamera;
    [SerializeField] private Transform PickupTarget;
    [Space]
    [SerializeField] private float PickupRange;
    //the object that the player is currently grabbing
    public Rigidbody CurrentObject;
    //public Rigidbody foundObject;
    Vector3 DirectionToPoint;

    public bool isGrabbing;
    

    [Header("References")]
    //OBJECT REFERENCES
    //public GameObject slotMenu;
    
    //SCRIPT REFERENCES
    public SlotButtons SB;
    public UpgradeMenu UM;
    public Outline OL;
    private PowerUps power;
    private SpiritOrb SO;


    [Header("Throw Properties")]
    //THROW PROPERTIES
    public float throwForce;
    public int ThrowDamage;

    [Header("Explosive Object Properties")]
    public float explosiveForce;
    public int ExplosiveDamage;




    // Start is called before the first frame update
    void Start()
    {
        //crosshair2.SetActive(false);
        animator.SetBool("Grabbable", false);
        animator.SetBool("Grabbed", false);

        UM = GameObject.Find("Menu").GetComponent<UpgradeMenu>();
        SB = GameObject.Find("Menu").GetComponent<SlotButtons>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (CurrentObject != null && !CurrentObject.gameObject.activeInHierarchy)
        {
            CurrentObject = null;
            isGrabbing = false;
            animator.SetBool("Grabbed", false);
            animator.SetBool("Grabbable", false);
        }

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


        //PLAYER INPUTS

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(CurrentObject == null)
                return;
            
            

            if(CurrentObject.gameObject.CompareTag("SpiritOrb"))
            {
                SO = CurrentObject.GetComponent<SpiritOrb>();
                SO.isExplosive = true;
                ThrowObject();
                SO.isThrown = true;
            }
            else if(CurrentObject)
            {
                ThrowObject();
                SO.isThrown = true;
            }
        }
        /*
        if(Input.GetKey(KeyCode.Mouse1))
        {
            SO = CurrentObject.GetComponent<SpiritOrb>();
            SO.Explode();
        }
        */

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (CurrentObject)
            {
                CurrentObject.useGravity = true;
                CurrentObject = null;
                animator.SetBool("Grabbed", false);
                isGrabbing = false;

                
                //[OUTLINE]
                OL = CurrentObject.GetComponent<Outline>();
                OL.eraseRenderer = true;

                //weapon.SetActive(true);

                return;
            }
            //Ray CameraRay = PlayerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            if (Physics.Raycast(CameraRay, out RaycastHit HitInfo, PickupRange, PickupMask))
            {
                // PICKING UP AN OBJECT
                //OL = CurrentObject.GetComponent<Outline>();
                CurrentObject = HitInfo.rigidbody;
                CurrentObject.useGravity = false;
                //crosshair2.SetActive(false);
                animator.SetBool("Grabbed", true);
                isGrabbing = true;
                
                
                power = CurrentObject.GetComponent<PowerUps>();
                SB.SetPendingPower(power);
                
                if(power == null)
                    return;

                if(power.dashUpgrade || power.floatUpgrade)
                {
                    UM.MenuOpen();
                    // The upgrade will be applied once the player selects a slot.
                }
                
                else if(power.doubleJumpUpgrade)
                {
                    power.gainDoubleJump();
                    CurrentObject = null;
                    animator.SetBool("Grabbed", false);
                }



                else if(power.highjumpUpgrade)
                {
                    power.gainHighJump();
                    CurrentObject = null;
                    animator.SetBool("Grabbed", false);
                }
                else if(power.sprintUpgrade)
                {
                    power.gainSpeedBoost();
                    CurrentObject = null;
                    animator.SetBool("Grabbed", false);
                }

                //[OUTLINE]
                OL = CurrentObject.GetComponent<Outline>();
                if(OL != null)
                    OL.eraseRenderer = true;
                //weapon.SetActive(false);
            }
        }
    }

    void FixedUpdate()
    {
        if (CurrentObject)
        {
            DirectionToPoint = PickupTarget.position - CurrentObject.position;
            float DistanceToPoint = DirectionToPoint.magnitude;

            CurrentObject.velocity = DirectionToPoint * 12f * DistanceToPoint; 
        }
    }

    public void ThrowObject()
    {
        // Add explosive ability to thrown objects (except SpiritOrb which has its own)
        if (!CurrentObject.gameObject.CompareTag("SpiritOrb"))
        {
            ExplosiveObject explosive = CurrentObject.gameObject.AddComponent<ExplosiveObject>();
            explosive.explosionForce = explosiveForce;
            explosive.explosionDamage = ExplosiveDamage;
            explosive.isThrown = true;
        }

        CurrentObject.AddForce(PickupTarget.transform.forward.normalized * throwForce, ForceMode.Impulse);
        //PickupTarget.transform.forward = DirectionToPoint.normalized;
        CurrentObject.useGravity = true;
        CurrentObject = null;
        animator.SetBool("Grabbed", false);
        isGrabbing = false;
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
