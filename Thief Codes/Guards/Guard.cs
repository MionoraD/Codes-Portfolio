using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    private CharacterController controller;

    [Header("For all guards")]
    [SerializeField] private Animator animator;
    private bool waitForAnimation = false;

    private float speed = 0;
    [SerializeField] private float walkingSpeed = 1;
    [SerializeField] private float runningSpeed = 3;
    [SerializeField] private float turnSpeed = 10;
    private bool turnedToTarget = false;

    private bool noticed = false;
    [Header("Catch collision")]
    [SerializeField] private float caughtDistance = 1;
    [SerializeField] private float lostDistance = 10;

    [SerializeField] private GameObject collision;
    [SerializeField] private RotateCollider colliderRotation;

    private Transform player;
    private PlayerMovement playerControl;

    [Header("For each guard")]
    [SerializeField] private bool move = false;
    private bool returnToPosition = false;
    private bool returnToRotation = false;

    private bool follow = false;
    public bool recognize = false;
    public GameObject watchOverItem;

    private int currentPosition = 0;
    [SerializeField] private List<Transform> pathPositions = new List<Transform>();

    void Start()
    {
        // Set guard on first position
        transform.position = pathPositions[0].position;

        // Find components gameObject
        controller = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (waitForAnimation) return;

        // If the guard has not noticed the player
        if (!noticed)
        {
            // Does the guard move when not following the player?
            if (move)
                Move(pathPositions[currentPosition]);
            // Go to the position (for after losing player)
            else
            {
                bool rotate = false;

                if (returnToPosition)
                    Move(pathPositions[0]);
                else if (returnToRotation)
                    TurnTowardsTarget(pathPositions[1]);
                else
                    rotate = true;

                colliderRotation.rotate = rotate;
            }
        }
        else
        {
            float distance = Vector3.Distance(player.position, transform.position);
            if(distance < caughtDistance)
            {
                if(playerControl != null) playerControl.PlayerCaught();
                ResetGuard(true);
            }
            else if (distance > lostDistance)
            {
                if (playerControl != null) playerControl.GuardLost(this);
                ResetGuard(false);
            }
            else
            {
                speed = runningSpeed;

                MoveTowardsTarget(player);
                TurnTowardsTarget(player);
            }
        }
    }

    private void Move(Transform position)
    {
        if (!turnedToTarget)
            TurnTowardsTarget(position);
        else
        {
            speed = walkingSpeed;
            MoveTowardsTarget(position);
        }
    }

    protected void MoveTowardsTarget(Transform target)
    {
        // Start walking animation
        if (speed == runningSpeed)
            animator.SetBool("Running", true);
        else
            animator.SetBool("Walking", true);

        Vector3 offset = target.position - transform.position;
        
        if (offset.magnitude > .1f)
        {
            offset = offset.normalized * speed;
            controller.Move(offset * Time.deltaTime);
        }
        else
        {
            currentPosition++;
            if (currentPosition == pathPositions.Count) currentPosition = 0;
            turnedToTarget = false;
            colliderRotation.rotate = false;

            returnToPosition = false;
            returnToRotation = true;

            animator.SetBool("Running", false);
            animator.SetBool("Walking", false);
        }
    }

    protected void TurnTowardsTarget(Transform target)
    {
        Vector3 targetDir = target.position - transform.position;

        float step = turnSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
        newDir.y = 0;
        Quaternion newRotation = Quaternion.LookRotation(newDir);

        if (transform.rotation.y < newRotation.y)
            animator.SetBool("TurnRight", true);
        else
            animator.SetBool("TurnLeft", true);

        transform.rotation = newRotation;

        Vector3 endDir = Vector3.RotateTowards(transform.forward, targetDir, 1, 0.0f);
        endDir.y = 0;

        if (endDir == newDir)
        {
            turnedToTarget = true;
            colliderRotation.rotate = true;
            returnToRotation = false;

            animator.SetBool("TurnLeft", false);
            animator.SetBool("TurnRight", false);
        }
    }

    public void NoticePlayer(Transform playerCaught)
    {
        noticed = true;
        collision.SetActive(false);

        animator.SetBool("Running", false);
        animator.SetBool("Walking", false);
        animator.SetBool("TurnLeft", false);
        animator.SetBool("TurnRight", false);

        StartCoroutine(PlayAnimationStartFind());

        player = playerCaught;
        playerControl = player.GetComponent<PlayerMovement>();
        if (playerControl != null) playerControl.GuardNoticed(this);
    }

    public void ResetGuard(bool foundPlayer)
    {
        noticed = false;

        animator.SetBool("Running", false);

        if (foundPlayer)
            StartCoroutine(PlayAnimationFind());
        else
            StartCoroutine(PlayAnimationDefeat());

        collision.SetActive(true);
        turnedToTarget = false;
        colliderRotation.rotate = false;

        returnToPosition = true;
        returnToRotation = true;
    }

    private IEnumerator PlayAnimationStartFind()
    {
        waitForAnimation = true;
        animator.SetTrigger("StartFind");

        yield return new WaitForSeconds(1);

        waitForAnimation = false;
    }

    private IEnumerator PlayAnimationDefeat()
    {
        waitForAnimation = true;
        animator.SetTrigger("Defeat");

        yield return new WaitForSeconds(3);

        waitForAnimation = false;
    }

    private IEnumerator PlayAnimationFind()
    {
        waitForAnimation = true;
        animator.SetTrigger("Find");

        yield return new WaitForSeconds(2);

        waitForAnimation = false;
    }
}
