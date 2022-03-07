using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowGameObject : MonoBehaviour
{
    [SerializeField] private Transform target;
    private float speed;
    [SerializeField] private float walkSpeed = 1;
    [SerializeField] private float sprintSpeed = 5;
    [SerializeField] private float betweenCameras = 8;
    private bool switching = false;
    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;

    [SerializeField] private PlayerMovement player;
    private Transform lookingAt;
    [SerializeField] private Transform transformCamera;
    private bool guards;

    private Vector3 startPosition;
    private Quaternion startRotation;

    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        lookingAt = player.transform;

        transform.position = target.position;
        transform.rotation = target.rotation;

        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = new Vector3(lookingAt.position.x, transformCamera.position.y, lookingAt.position.z);
        if (guards) targetPosition.y = lookingAt.position.y;
        transformCamera.LookAt(targetPosition);

        if (!LevelManager.Level.CanPlay) return;

        Vector3 offset = target.position - transform.position;
        if (offset.magnitude > .1f)
        {
            if (!switching)
            {
                speed = walkSpeed;
                if (player.Sprint) speed = sprintSpeed;

                float dist = Vector3.Distance(target.position, transform.position);
                if (dist > 2f) speed += 1;
            }

            offset = offset.normalized * speed;
            controller.Move(offset * Time.deltaTime);
        }
    }

    public void ChangeFollowObject(bool guardsfollow, Transform followObject)
    {
        guards = guardsfollow;
        target = followObject;

        StartCoroutine(SwitchingTarget());
    }

    public IEnumerator SwitchingTarget()
    {
        speed = betweenCameras;
        switching = true;
        yield return new WaitForSeconds(1f);
        switching = false;
    }

    public void ResetCamera()
    {
        transform.position = startPosition;
    }
}
