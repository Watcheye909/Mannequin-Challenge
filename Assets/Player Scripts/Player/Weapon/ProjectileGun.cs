using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProjectileGun : MonoBehaviour
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

    int bulletsLeft, bulletsShot;

    //bools
    public bool shooting, readyToShoot, reloading;
    public bool bulletReady;

    //reference
    public Camera fpsCam;
    public Transform attackPoint;

    //Graphics
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;

    //bug fixing 
    public bool allowInvoke = true;

    private void Awake()
    {
        //check magazine is full
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();

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
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        //allows the shooting animation to play again while already shooting
        if (shooting && readyToShoot) animator.SetBool("ShotBullet", true);
        else animator.SetBool("ShotBullet", false);


        
        //shooting
        if(readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;
            bulletReady = true;
            //Shoot();
        }
        


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
    }

    private void Shoot()
    {
        //shooting animation
        animator.SetBool("Shooting", true);
        
        readyToShoot = false;
        

        //find the exact hit position using a raycast
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        //check if ray hits something
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75); //just a point far away from the player

        //calculate direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        //calculate spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //calculate new direction with spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        //Instantiate bullet/projectile
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);

        //rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithSpread.normalized;


        Rigidbody rb = currentBullet.GetComponent<Rigidbody>();

        //add forces to bullet
        //Vector3 velocity=directionWithSpread.normalized*shootForce;
        //rb.MovePosition(rb.position+(velocity * Time.fixedDeltaTime));

        rb.AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        rb.AddForce(fpsCam.transform.up *upwardForce, ForceMode.Impulse);

        //

        //Instantiate muzzle flash, if you have one
        if (muzzleFlash != null)
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity); 

        bulletsLeft--;
        bulletsShot++;

        if(allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

        //if more than one bulletsPerTap repeat shoot function
        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke("Shoot", fireRate);

        bulletReady = false;
    }

    private void ResetShot()
    {
        //allow shooting and invoking again
        animator.SetBool("Shooting", false);
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
        animator.SetBool("Reloading", true);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
        animator.SetBool("Reloading", false);
    }
}
