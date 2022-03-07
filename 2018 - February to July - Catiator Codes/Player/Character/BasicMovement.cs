using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicMovement : MonoBehaviour
{
    // Move left/right/forward/backward
    // Jump
    // Basic attack
    // cc attack
    // Passive ability
    // Ultimate attack

    // Name of the character
    public string player, character;

    //State Control Stun,Death
    public bool canMove = true;
    private bool isDead = false;
    [HideInInspector] public bool attack = false;

    // The movement speed of the current character
    public float movementSpeed = 6.0f;
    private float moveSpeed;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    // The direction of the movement
    private Vector3 moveDirection = Vector3.zero;

    // The CharacterController of the GameObject
    private CharacterController controller;

    // Variables needed to dash (used to be a jump)
    private bool jumping;
    private float dashTimer;
    [SerializeField] private float dashTime = 1f;
    [SerializeField] Transform transformDashDirection;
    private Vector3 dashDirection = Vector3.zero;
    private float timer = 0, endTime = 0.5f;

    private Vector3 lastdirection = Vector3.zero;

    // Variables needed for the attacks
    private bool holdBasic = false;
    [SerializeField] private float basicTime = 2.5f;
    private float basicTimer;
    
    [HideInInspector] public bool useCC;
    public float ccTime = 3.0f;
    [HideInInspector] public float ccTimer;

    [HideInInspector] public bool useUltimate;
    public float ultimateTime = 5.0f;
    [HideInInspector] public float ultimateTimer;

    // The weapon(s) that the character uses
    [SerializeField] private Weapon weapon;
    [SerializeField] private CCAttack ccAttack;

    // Variables that can change through pickups
    public float movementMultiplyer;
    public float dmgMultiplyer;

    // The spawnpoint of the character
    public Vector3 spawnPoint = new Vector3(0, 2.5f, 0);
    public Quaternion rotationPoint = Quaternion.identity;

    // The basic settings to see which character belongs to which player
    public Text name;
    public Color clr;
    [SerializeField] private Renderer rendering;

    // The animator of the model and the variables needed for that
    [SerializeField] private Animator animator;
    public Animator CharacterAnimator
    {
        get { return animator; }
        private set { animator = value; }
    }
    [SerializeField] private float timeAnimationBasic = 2f;
    [SerializeField] private float timeAnimationCC = 3f;
    [SerializeField] private float timeAnimationUltimate = 2f;

    //Audio
    private AudioManager audioManager;

    // Dash feedback
    [SerializeField] private GameObject dashFeedback;
	[SerializeField] private GameObject rangerAttack;

    //Turn to camera endgame
    [HideInInspector] public bool endgame = false;

    // How fast does the character move?
    [SerializeField] private float attackTime = 1f;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }
    
    void Start()
    {
        // Setup the moment that the character is spawned
        controller = gameObject.GetComponent<CharacterController>();
        basicTimer = basicTime;
        canMove = true;

        // Find the weapon of the character, and set the right character
        weapon.SetPlayerManager(gameObject.GetComponent<PlayerManager>());
        gameObject.GetComponent<PlayerManager>().SetAnimator(animator);

        // Set the character to the game animations
        if(animator != null)
            animator.SetBool("Game", true);
    }

    void Update()
    {
        // If the game has ended, turn the character towards the chamera
        if (endgame) RotateCharacter(Vector3.back);

        // Check if the player has died
        PlayerManager manager = gameObject.GetComponent<PlayerManager>();
        isDead = manager.death;

        // If the player cannot move, or has died then do not go further
        if (isDead || !canMove) return;

        //Alternative timer to solve cooldown bug
        if(basicTimer<basicTime)
        {
            basicTimer += Time.deltaTime;
        }

        // If the basic attack button has been used then use that attack
        if (holdBasic)
        {
            if (basicTimer >= basicTime)
            {
                weapon.UseWeaponBasic(animator);

                // animator.SetTrigger("Basic");
                StartCoroutine(Attack(1f));

                basicTimer = 0;
            } 
        }

        // Timer of cc attack
        if (ccTimer < ccTime)
        {
            ccTimer += Time.deltaTime;
            useCC = false;
        }
        else
        {
            useCC = true;
        }
        
        // Timer of ultimate attack
        if (ultimateTimer < ultimateTime)
        {
            ultimateTimer += Time.deltaTime;
            useUltimate = false;
        }
        else
        {
            useUltimate = true;
        }

        // If dmg is changed
        weapon.ChangeMultiplayer(dmgMultiplyer);

        // Dashing the character if the button is used
        if (jumping)
        {
            //Dash Audio
            audioManager.Play("Character_Dash");

            controller.Move(dashDirection * Time.deltaTime * jumpSpeed);
            RotateCharacter(moveDirection);

            if (transform.position == dashDirection | timer > endTime)
            {
                if (animator != null)
                    animator.SetBool("Dashing", false);
                dashFeedback.SetActive(false);
                dashTimer = 0;
                jumping = false;
            }

            timer += Time.deltaTime;
        }
        else
        {
            dashTimer += Time.deltaTime;
        }
    }

    // Move player
    public void MovingCharacter(Vector3 direction, bool jump)
    {
        if(controller == null)
        {
            Debug.Log("Why?");
            return;
        }
        if (isDead | jumping | !canMove | attack) return;

        if (controller.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes

            moveDirection = direction;

            moveSpeed = movementSpeed;
            if (movementMultiplyer != 0)
                moveSpeed = moveSpeed * movementMultiplyer;

            // moveDirection = transform.TransformDirection(moveDirection);
            moveDirection = moveDirection * moveSpeed;

            if (jump)
            {
                jump = false;
                // moveDirection = moveDirection * jumpSpeed;
                // moveDirection.y = jumpSpeed;
                if(dashTimer >= dashTime)
                {
                    Jump();
                    return;
                }
            }
        }

        // Apply gravity
        moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);

        //Commented it to imlement new rotation system
        //RotateCharacter(moveDirection);

        // Move the controller
        controller.Move(moveDirection * Time.deltaTime);

        // Animation
        if (animator != null)
        {
            if (moveDirection.x == 0 && moveDirection.z == 0)
                animator.SetBool("Running", false);
            else
                animator.SetBool("Running", true);
        }
    }

    // Dash controls
    private void Jump()
    {
        if (animator != null)
            animator.SetBool("Dashing", true);
        if (moveDirection == Vector3.zero)
        {
            dashDirection = transform.forward;
        }
        else
        {
            dashDirection = moveDirection.normalized;
        }
        
        dashDirection.y = 0;
        timer = 0;

        dashFeedback.SetActive(true);
        jumping = true;
    }

    // Return to spawn point when player has died
    public void MoveToSpawnPoint()
    {
        moveDirection = new Vector3(0, 0, 0);
        controller.Move(moveDirection * Time.deltaTime);

        transform.position = spawnPoint;
        transform.rotation = rotationPoint;
    }

    // Set the current use of the basic attack button
    public void HoldBasic(bool hold)
    {
        if (isDead | jumping | !canMove)
        {
            holdBasic = false;
            return;
        }

        holdBasic = hold;
        //if (!hold) basicTimer = basicTime;
    }

    // Set cc attack
    public void UseCCAttack()
    {
        if (isDead | jumping | !canMove) return;

        if (useCC)
        {
            // animator.SetTrigger("CC");
			ccAttack.UseWeaponCC();
			StartCoroutine(Attack(1f));
            ccTimer = 0;
        }
    }

    // Set ultimate attack
    public void UseUltimateAttack()
    {
        if (isDead | jumping | !canMove) return;

        if (useUltimate)
        {
            // weapon.UseWeaponUltimate();
            // animator.SetTrigger("Ultimate");
            StartCoroutine(Attack(2f));
            ultimateTimer = 0;
        }
    }

    // Rotating the character
    public void RotateCharacter(Vector3 direction)
    {
        Vector3 rotation = new Vector3(transform.eulerAngles.x, Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg, transform.eulerAngles.z);
        
        if (direction.x == 0 && direction.z == 0)
        {
            rotation.y = transform.rotation.eulerAngles.y;
        }
        else
        {
            transform.eulerAngles = rotation;
        }

    }

    // Set the settings of the level
    public void Settings(SettingsLevel settings)
    {
        movementSpeed = settings.moveSpeed;
        jumpSpeed = settings.jumpSpeed;
        gravity = settings.gravity;

		if(rangerAttack != null)
		{
			rangerAttack.transform.localPosition = new Vector3(rangerAttack.transform.localPosition.x, rangerAttack.transform.localPosition.y, settings.distanceRanger);
		}

        player = settings.player;
        character = settings.character;
    }

    // Start the character controller
    public void SetBasics(string _name, Color _clr)
    {
        name.text = _name;
        name.color = _clr;

        clr = _clr;
        player = _name;

        rendering.materials[2].color = clr;
    }

    // Timer of basic attack
    private IEnumerator Attack(float seconds)
    {
        attack = true;
        yield return new WaitForSeconds(attackTime);
        attack = false;
    }
}
