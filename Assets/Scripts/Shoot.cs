using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] Transform shootpoint;
    
        
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            fireweapon();
        }
    }

    void fireweapon()
    {
        RaycastHit hitinfo;
        bool hit = Physics.Raycast(shootpoint.position, shootpoint.forward, out hitinfo);
        if (hit)
        {
            Debug.Log(hitinfo.collider.gameObject.name);

        }





    }
}
