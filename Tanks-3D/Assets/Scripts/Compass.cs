using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    public Transform player;
    public Transform compassObject;
    public Vector3 northDirection;

    void Update()
    {
        Vector3 playerDirection = player.forward;
        playerDirection.y = 0f;
        Quaternion playerRotation = Quaternion.LookRotation(playerDirection);

        Vector3 compassDirection = northDirection;
        compassDirection.y = 0f;
        Quaternion compassRotation = Quaternion.LookRotation(compassDirection);

        float angle = Quaternion.Angle(playerRotation, compassRotation);
        Vector3 cross = Vector3.Cross(playerRotation * Vector3.forward, compassRotation * Vector3.forward);
        if (cross.y < 0)
        {
            angle = -angle;
        }

        compassObject.rotation = Quaternion.Euler(0f, 0f, -angle);
    }
}
