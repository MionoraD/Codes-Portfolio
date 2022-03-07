using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Zooming in at winning character at the end of the level

public class EndLevelCamera : MonoBehaviour
{
    public static EndLevelCamera cam;

    [SerializeField] private float timeToReachTarget = 3f;
    [SerializeField] private float maxDistance = 10;
    private float t = 0;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    private bool moveCamera = false;

    private bool moveOn = false;
    // When the screen can go to the main menu
    public bool MovingOn
    {
        get { return moveOn; }
        private set { moveOn = value; }
    }

    void Awake()
    {
        cam = this;
    }

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (moveCamera)
        {
            if(t > timeToReachTarget)
            {
                moveCamera = false;
                MovingOn = true;
            }
            // Zoom camera on destination (character)
            else if (t < timeToReachTarget)
            {
                float dist = Vector3.Distance(targetPosition, transform.position);
                if(dist > maxDistance)
                    transform.position = Vector3.Lerp(startPosition, targetPosition, t);

                Vector3 pos = targetPosition - transform.position;
                var newRot = Quaternion.LookRotation(pos);
                transform.rotation = Quaternion.Lerp(transform.rotation, newRot, t);

                t += Time.deltaTime / timeToReachTarget;
            }
        }
    }

    // When a character has won
    public void SetDestination(Transform destination)
    {
        t = 0;
        startPosition = transform.position;
        targetPosition = destination.position;

        moveCamera = true;
    }

    // When another win condition
    public void NoMove()
    {
        MovingOn = true;
    }
}
