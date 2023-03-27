using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    public Transform playerTransform;
    private Vector3 _direction;

    void Update()
    {
        _direction.z = playerTransform.eulerAngles.y;
        transform.localEulerAngles = _direction;
    }
}








