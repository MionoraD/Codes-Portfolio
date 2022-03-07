using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("LevelCamera").transform;
    }

    private void Update()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("LevelCamera").transform;
            return;
        }

        Vector3 adjusted = target.position;
        adjusted.z -= 45;
        transform.LookAt(adjusted);
    }
}
