using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMannequin : MonoBehaviour
{
    public Transform player;
    public Camera playerCam;
    public bool spotted;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //DETECTS THE BOUNDS OF THE PLAYER CAMERA
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerCam);

        //CHECKS IF THE MANNEQUIN IS IN VIEW
        if(GeometryUtility.TestPlanesAABB(planes, this.gameObject.GetComponentInChildren<Renderer>().bounds))
            spotted = true;
        else if(!GeometryUtility.TestPlanesAABB(planes, this.gameObject.GetComponentInChildren<Renderer>().bounds))
            spotted = false;


        if(!spotted)
        transform.LookAt(player);
    }
}
