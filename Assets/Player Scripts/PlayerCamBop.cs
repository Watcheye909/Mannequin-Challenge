using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamBop : MonoBehaviour
{
    [SerializeField] private bool enable = true;

    [SerializeField, Range(0, 1f)] private float amplitude = 0.015f;
    [SerializeField, Range(0, 30)] private float frequency = 10.0f;

    [SerializeField] private Transform camera = null;
    [SerializeField] private Transform cameraHolder = null;

    private float toggleSpeed = 3.0f;
    private Vector3 startPos;
    public PlayerMovement controller;
    public Rigidbody rb;

    private void Awake()
    {
        //rb = GetComponent<Rigidbody>();
        //controller = GetComponent<PlayerMovement>();
        startPos = camera.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.grounded)
        {
            ResetPosition();
            enable = true;
        } 
            
        if (!controller.grounded) 
        {
            
            enable = false;
        }
        if (!enable) return;
        ResetPosition();
        CheckMotion();
        camera.LookAt(FocusTarget());
    }



    private void PlayMotion(Vector3 motion)
    {
        camera.localPosition += motion;
    }

    private void CheckMotion()
    {
        if (controller.moveSpeed < 0) return;
        if (rb.velocity.magnitude < 1f) return;
        if (!controller.grounded) return;

        PlayMotion(FootStepMotion());
    }

    private Vector3 FootStepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * frequency) * amplitude;
        pos.x += Mathf.Cos(Time.time * frequency / 2) * amplitude;
        return pos;
    }

    private void ResetPosition()
    {
        if (camera.localPosition == startPos) return;
        camera.localPosition = Vector3.Lerp(camera.localPosition, startPos, 1 * Time.deltaTime);
    }

    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + cameraHolder.localPosition.y, transform.position.z);
        pos += cameraHolder.forward * 15.0f;
        return pos;
    }

}
