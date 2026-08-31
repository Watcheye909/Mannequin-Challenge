using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpiritGun : MonoBehaviour
{
    //animation
    public Animator animator;
    //bullet
    public GameObject bullet;

    //bullet force
    public float shootForce, upwardForce;

    //gun stats
    public float timeBetweenShooting, spread, reloadTime, fireRate;
    public int magazineSize, bulletsPerTap;
    public int damage;
    public bool allowButtonHold;

    public int bulletsLeft;

    //bools
    public bool shooting, readyToShoot, reloading;
    public bool bulletReady;

    //reference
    public Camera fpsCam;
    public Transform attackPoint;
    public LayerMask EnemyLayer;
    public GroundChaser groundChaser;

    //audio
    public AudioSource gunShotAudio;
    public AudioSource reloadAudio;
    public AudioSource chargedAudio;

    //Graphics
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;

    //bug fixing 
    public bool allowInvoke = true;

    [Header("Debug")]
    public bool showRaycastDebug = true;
    public Color raycastColor = Color.red;
    public float raycastDistance = 10f;

    private Vector3 debugRayStart;
    private Vector3 debugRayEnd;

    private void Awake()
    {
        //check magazine is full
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
        if(bulletsLeft > 0 && !reloading && !readyToShoot)
        ResetShot();

        //set ammo display, if it exists
        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
    }
    
    private void FixedUpdate()
    {
        if(bulletReady == true)
            Shoot();
    }

    private void MyInput()
    {
        //check if allowed to hold down button and take corresponding input
        //if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        shooting = Input.GetKeyDown(KeyCode.Mouse0);

        //allows the shooting animation to play again while already shooting
        //if (shooting && readyToShoot) animator.SetBool("ShotBullet", true);
        //else animator.SetBool("ShotBullet", false);


        
        //shooting
        if(readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletReady = true;
            //Shoot();
        }
        


    /*
        //reloading
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
        {
            Reload(); 
        }
        //reload automatically if ammo runs out
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0)
        {
            Reload();
        }

    */
    }




    private void Shoot()
    {
        //shooting animation
        //animator.SetBool("Shooting", true);

        readyToShoot = false;

        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        debugRayStart = attackPoint.position;
        debugRayEnd = ray.GetPoint(raycastDistance);

        if (Physics.Raycast(ray, out hit, raycastDistance, EnemyLayer))
        {
            //GroundChaser groundChaser = hit.collider.GetComponentInParent<GroundChaser>();
            groundChaser = hit.collider.GetComponentInParent<GroundChaser>();
            Debug.Log("Current guy:" + groundChaser);
            debugRayEnd = hit.point;

            if (groundChaser != null)
                groundChaser.takeDamage(damage);
        }


        gunShotAudio.enabled = true;

        //Instantiate muzzle flash, if you have one
        if (muzzleFlash != null)
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletsLeft--;

        if (allowInvoke)
        {
            Invoke("Reload", timeBetweenShooting);
            //Invoke("ResetShot", timeBetweenShooting); OLD
            allowInvoke = false;
        }


    /*
        //if more than one bulletsPerTap repeat shoot function
        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke("Shoot", fireRate);
    */

        bulletReady = false;
    }

    private void ResetShot()
    {
        //allow shooting and invoking again
        //animator.SetBool("Shooting", false);
        readyToShoot = true;
        allowInvoke = true;
    }

    private void OnDrawGizmos()
    {
        if (!showRaycastDebug || fpsCam == null || attackPoint == null)
            return;

        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 end = ray.GetPoint(raycastDistance);

        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, EnemyLayer))
            end = hit.point;

        Gizmos.color = raycastColor;
        Gizmos.DrawLine(attackPoint.position, end);
        Gizmos.DrawSphere(end, 0.05f);
    }

    private void Reload()
    {
        reloading = true;
        gunShotAudio.enabled = false;
        reloadAudio.enabled = true;
        Invoke("ReloadFinished", reloadTime);
        //animator.SetBool("Reloading", true);
    }

    private void ReloadFinished()
    {
        chargedAudio.enabled = false;
        bulletsLeft = magazineSize;
        reloading = false;
        reloadAudio.enabled = false;
        chargedAudio.enabled = true;
        //animator.SetBool("Reloading", false);
    }
}
