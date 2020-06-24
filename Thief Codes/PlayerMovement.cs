using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    [SerializeField] private Transform startPosition;

    [SerializeField] private float speed = 10;
    [SerializeField] private float crouchSpeed = 5;
    [SerializeField] private float sprintingSpeed = 20;

    [SerializeField] private float turnSpeed = 90;
    [SerializeField] private float jumpSpeed = 8;
    [SerializeField] private float gravity = 9.8f;
    private float vSpeed = 0;

    [SerializeField] private Animator animCharacter;
    private bool crouching = false;
    private bool sprinting = false;
    public bool Sprint
    {
        get { return sprinting; }
        private set { sprinting = value; }
    }

    private ItemControl item = null;

    [Header("Guard alarm")]
    [SerializeField] private FollowGameObject cameraC;
    [SerializeField] private Transform mainCameraPosition;
    [SerializeField] private Transform alarmCameraPosition;

    private List<Guard> guards = new List<Guard>();
    [SerializeField] private GameObject guardsUI;
    [SerializeField] private Text guardsUITextTotal;
    [SerializeField] private Text guardsUITextDistance;

    [HideInInspector] public bool suspicious = false;
    
    private float movetimer = 0;
    [SerializeField] private float waitBeforeMove = 1;

    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        guardsUI.SetActive(false);
        cameraC.ChangeFollowObject(false, mainCameraPosition);
    }

    public void Update()
    {
        if (!LevelManager.Level.CanPlay) return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Rotate character
        transform.Rotate(0, horizontal * turnSpeed * Time.deltaTime, 0);

        // Set speed
        float moveSpeed = speed;

        // Start Crouching
        if (Input.GetKeyDown("space"))
        {
            crouching = !crouching;
            animCharacter.SetBool("Crouch", crouching);
        }

        // Crouching
        if (crouching)
            moveSpeed = crouchSpeed;
        
        // Sprinting if not crouching
        if (!crouching && Input.GetKey(KeyCode.LeftShift))
        {
            Sprint = true;
            animCharacter.SetBool("Sprinting", true);
            moveSpeed = sprintingSpeed;
        }
        else
        {
            Sprint = false;
            animCharacter.SetBool("Sprinting", false);
        }

        // Calculate movement
        Vector3 vel = transform.forward * vertical * moveSpeed;
        if (controller.isGrounded)
        {
            vSpeed = 0;
        }
        vSpeed -= gravity * Time.deltaTime;
        vel.y = vSpeed;

        // Move character
        controller.Move(vel * Time.deltaTime);

        // Walking animation
        if (vertical == 0)
            animCharacter.SetBool("Walking", false);
        else
            animCharacter.SetBool("Walking", true);

        // If player is near an item
        if(item != null)
        {
            if (Input.GetButtonDown("Pickup"))
            {
                // Use item when player uses the right input
                item.ItemAction();
                item = null;
            }
        }
        else if (Input.GetButtonDown("ThrowRock"))
        {
            Debug.Log("Throw rock");
        }

        if (Input.GetButtonDown("OpenInventory"))
        {
            Debug.Log("Open inventory");
        }

        if(guards.Count > 0)
        {
            guardsUITextTotal.text = "Guards " + guards.Count;
            
            bool filled = false;
            float lowestDistance = 0;
            foreach (Guard grd in guards)
            {
                float distance = Vector3.Distance(grd.transform.position, transform.position);
                if (filled)
                {
                    if (distance < lowestDistance) lowestDistance = distance;
                }
                else
                {
                    lowestDistance = distance;
                    filled = true;
                }
            }
            string txtDistance = "Distance " + Mathf.Round(lowestDistance);
            guardsUITextDistance.text = txtDistance;
        }

        if (crouching || Sprint) suspicious = true;
        else suspicious = false;
    }

    public void AddItem(ItemControl newItem)
    {
        item = newItem;
    }

    public void RemoveItem(ItemControl removeItem)
    {
        if (item == removeItem)
        {
            item = null;
        }
    }

    public void GuardNoticed(Guard guard)
    {
        guards.Add(guard);
        LevelManager.Level.GiveMessage("A guard noticed you!");

        if (guards.Count <= 1)
        {
            cameraC.ChangeFollowObject(true, alarmCameraPosition);
            guardsUI.SetActive(true);
        }
    }
    public void GuardLost(Guard guard)
    {
        guards.Remove(guard);

        if (guards.Count <= 0)
        {
            LevelManager.Level.GiveMessage("Lost all guards");
            cameraC.ChangeFollowObject(false, mainCameraPosition);
            guardsUI.SetActive(false);
        }
    }

    public void PlayerCaught()
    {
        ResetPlayer();
        LevelManager.Level.ResetLevel();
    }

    public void ResetPlayer()
    {
        guardsUI.SetActive(false);

        if (startPosition != null)
        {
            transform.position = startPosition.position;
            transform.rotation = startPosition.rotation;
        }

        cameraC.ResetCamera();

        foreach (Guard grd in guards)
        {
            grd.ResetGuard(true);
        }
        guards = new List<Guard>();

        cameraC.ChangeFollowObject(false, mainCameraPosition);
    }

    public void StopAnimations()
    {
        animCharacter.SetBool("Walking", false);
        animCharacter.SetBool("Sprinting", false);

        crouching = false;
        animCharacter.SetBool("Crouch", crouching);
    }
}
