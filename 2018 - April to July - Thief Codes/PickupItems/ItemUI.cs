using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUI : MonoBehaviour
{
    [SerializeField] private Transform target;

    void Update()
    {
        // Turn the UI to the camera
        transform.LookAt(target);
    }
}
