using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFollows : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 2;

    private bool hasStarted = false;
    
    void Update()
    {
        if (!hasStarted && transform.parent.gameObject.activeSelf)
        {
            transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
            hasStarted = true;
        }

        Vector3 wayPointPos = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.position = Vector3.MoveTowards(transform.position, wayPointPos, speed * Time.deltaTime);


        //Rotation
        transform.rotation = Quaternion.Euler(0, target.eulerAngles.y, 0);
    }
}
