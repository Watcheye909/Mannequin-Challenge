using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCamera : MonoBehaviour
{

    public int FOV; //never changes

    public float sensX;
    public float sensY;

    public Transform orientation;
    public Transform camHolder;

    float xRotation;
    float yRotation;

    // Start is called before the first frame update
    private void Start()
    {
        DOTween.SetTweensCapacity(500,20);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        //LETS THE PLAYER ROTATION BE CHANGED DEPENDING ON THE ROTATION THEY HAVE IN THE EDITOR
        if (orientation != null)
        {
            yRotation = orientation.eulerAngles.y;
        }
        else
        {
            yRotation = transform.eulerAngles.y;
        }

        if (camHolder != null)
        {
            xRotation = camHolder.localEulerAngles.x;
        }
        else
        {
            xRotation = transform.localEulerAngles.x;
        }
        //SPECIFIC COMMAND ENDS HERE
    }

    // Update is called once per frame
    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);


        camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void DoFov(float endValue)
    {
        GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
    }

    public void DoTilt(float zTilt)
    {
        transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);
    }

    public void DoLean(float xTilt)
    {
        transform.DOLocalRotate(new Vector3(xTilt, 0, 0), 0.25f);
    }

    public void DoShake(float shakeAmt, float length)
    {
        DOTween.Kill(transform, false);
        transform.DOShakePosition(length, new Vector3(shakeAmt, shakeAmt, 0), vibrato: 10, randomness: 0.5f, snapping: false, fadeOut: true);
    }
}
