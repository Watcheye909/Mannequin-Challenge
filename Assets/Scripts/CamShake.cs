using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    [Header("Refrences")]
    public Transform cameraTransformation;

    [Header("Settings")]
    public float intensity;
    public float speed;

    [Header("state")]
    Vector3 _originalpos;
    Vector3 _TargetOffset;
    Vector3 _currentOffset;
    public bool _shake = false;

    private void Start()
    {
        _originalpos = cameraTransformation.localPosition;
    }

    public void ShakeCamera(float Duration)
    {
        if (_shake)
        {
            return;

        }
        StartCoroutine(ShakeCoroutine(Duration));

    }

    IEnumerator ShakeCoroutine(float Duration)
    {
        _shake = true;
        yield return new WaitForSeconds(Duration);
        _shake = false;
    }

    private void Update()
    {
        if (_shake)
        {

            if (Vector3.Distance(_currentOffset, _TargetOffset) < 0.001f)
            {
                _TargetOffset = Random.insideUnitSphere * intensity;

            }
            _currentOffset = Vector3.Lerp(_currentOffset, _TargetOffset, Time.deltaTime * speed);
            cameraTransformation.localPosition = _originalpos + _currentOffset;
        }
        else
        {

            cameraTransformation.localPosition = Vector3.Lerp(cameraTransformation.localPosition, _originalpos, Time.deltaTime * speed);
        }

    }
}


