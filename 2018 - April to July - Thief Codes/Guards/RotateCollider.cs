using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCollider : MonoBehaviour
{
    // The target marker.

    // Angular speed in radians per sec.
    [SerializeField] private int degrees = 45;
    [HideInInspector] public bool rotate = true;

    void Update()
    {
        if (rotate)
        {
            float angle = Mathf.Sin(Time.time) * degrees;
            transform.localRotation = Quaternion.AngleAxis(angle, Vector3.up);
        }
        else
        {
            transform.localRotation = Quaternion.AngleAxis(0, Vector3.up);
        }
    }
}
