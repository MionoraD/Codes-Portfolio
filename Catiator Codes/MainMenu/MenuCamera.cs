using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    private Transform currentPosition;
    private float speed = 1.5f;

    void Start()
    {
        currentPosition = transform;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, currentPosition.position, Time.deltaTime * speed);
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, currentPosition.eulerAngles, Time.deltaTime * speed);
    }

    public void GoToScreen(Transform screen)
    {
        currentPosition = screen;
    }

    public void ScreenSwitch(Transform screen)
    {
        transform.position = screen.position;
        transform.eulerAngles = screen.eulerAngles;
    }
}
