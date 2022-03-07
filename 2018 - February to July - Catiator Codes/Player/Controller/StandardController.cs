using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardController : MonoBehaviour
{
    // The player data
    public string name;
    public Color clr;

    // The menu in which the controller chooses their character
    private CharacterMenu menuCharacter;
    
    // Current chosen character and its the code that make it move
    [SerializeField] private GameObject currentCharacter;
    [SerializeField] private BasicMovement character;

    // The nr of the controller
    [SerializeField] private string controller;

    // The codes of the actions
    private const string strHorizontal = "Horizontal";
    private const string strVertical = "Vertical";
    private const string jump = "Jump";
    private const string basic = "Basic";
    private const string cc = "CC";
    private const string ultimate = "Ultimate";

    // The actual codes of the buttons (controller nr + action name)
    // Has to be set later because controller could receive a different controller nr
    private string controlHorizontal, controlVertical, controlJump;
    private string controlBasic, controlCC, controlUltimate;

    //rotation thing
    private const string strRightHorizontal = "Horizontal2";
    private const string strRightVertical = "Vertical2";
    private string controlRightHorizontal, controlRightVertical;
    private bool shootingUpdate;

    //CCattack modified controls
    private const string rightStick = "RightStick";
    private string controlRightStick;
    private float rotStickTapCount;
    private float rotStickTimeDelay;

    // In what state this controller is, what the control currently controls
    // Could all be false when game has NOT been started and character has been chosen
    private bool game, menu;

    private bool stickDownLast;

    // If characte has been chosen
    public bool canChoose
    {
        get; private set;
    }

    void Start()
    {
        canChoose = true;
        shootingUpdate = false;
    }

    void Update()
    {
        // If the game has started
        if (game)
        {
            // Find current direction of controls
            float horizontal = Input.GetAxis(controlHorizontal);
            float vertical = Input.GetAxis(controlVertical);

            // Set current direction into vector3
            Vector3 moveDirection = new Vector3(horizontal, 0.0f, vertical);

            //Rotation thing CC
            float rightHorizontal = Input.GetAxis(controlRightHorizontal);
            float rightVertical = Input.GetAxis(controlRightVertical);

            // Set rotation into Vector3
            Vector3 rotationDirection = new Vector3(rightHorizontal, 0.0f, rightVertical);

            // check if jump button is used
            bool jumping = false;
            if (Input.GetButtonDown(controlJump))
                jumping = true;

            // Move and rotate character
            character.MovingCharacter(moveDirection, jumping);
            character.RotateCharacter(rotationDirection);


            if(Math.Abs(rightHorizontal) + Math.Abs(rightVertical)>0.9f)
            {
                character.HoldBasic(true);
                shootingUpdate = true;
            }
            else
            {
                if(shootingUpdate)
                {
                    character.HoldBasic(false);
                    shootingUpdate = false;
                }
            }

            // Check if any buttons are used
            if (Input.GetButtonDown(controlBasic))
                character.HoldBasic(true);
            if (Input.GetButtonUp(controlBasic))
                character.HoldBasic(false);
            if (Input.GetButtonDown(controlRightStick))
            {
                Debug.Log("Right Stick clicked");
            }
            if (Input.GetButtonUp(controlRightStick))
            {
                rotStickTapCount += 1f;
                Debug.Log("Right Stick released");
            }
            if (Input.GetButtonDown(controlCC))
                character.UseCCAttack();
            if (Input.GetButtonDown(controlUltimate))
                character.UseUltimateAttack();

            if (rotStickTapCount > 0 && rotStickTimeDelay < 0.75f)
                rotStickTimeDelay += Time.deltaTime;

            if(rotStickTapCount > 1 && rotStickTimeDelay < 0.75f)
            {
                if(Math.Abs(rightHorizontal) + Math.Abs(rightVertical) > 0.8f)
                {
                    Debug.Log("CCAttack Used");
                    character.UseCCAttack();
                    rotStickTimeDelay = 0f;
                    rotStickTapCount = 0f;
                }
            }
            if(rotStickTimeDelay>0.75f)
            {
                rotStickTimeDelay = 0f;
                rotStickTapCount = 0f;
            }
        }

        // If the controller has to be in the menu state
        if (menu)
        {
            // If character has not been chosen yet
            if (canChoose)
            {
                // Check if the player uses the controller to go to the last or next character
                if (Input.GetAxis(controlHorizontal) < 0)
                {
                    if (!stickDownLast)
                        menuCharacter.LastItem();

                    stickDownLast = true;
                }
                else if (Input.GetAxis(controlHorizontal) > 0)
                {
                    if (!stickDownLast)
                        menuCharacter.NextItem();
                    stickDownLast = true;
                }
                else
                    stickDownLast = false;
            }

            // If the player uses the confirm button
            if (Input.GetButtonDown(controlJump))
            {
                Confirm();
            }
            // If the player uses the cancel button
            if (Input.GetButtonDown(controlCC))
            {
                Cancel();
            }
        }
    }

    // The player choses the current character in the menu
    public void Confirm()
    {
        canChoose = false;
        currentCharacter = menuCharacter.ChooseCharacter();
    }

    // The player cancels their character choice
    public void Cancel()
    {
        canChoose = true;
        menuCharacter.CancelCharacter();
    }

    // Setup the right input codes of the controller
    public void SetupButtons(bool pc, int controllerIndex)
    {
        if (pc)
            controller = "PC";
        else
            controller = "J" + controllerIndex;

        //Set Rotation stick
        controlRightHorizontal = controller + strRightHorizontal;
        controlRightVertical = controller + strRightVertical;
        controlRightStick = controller + rightStick;

        controlHorizontal = controller + strHorizontal;
        controlVertical = controller + strVertical;
        controlJump = controller + jump;
        controlBasic = controller + basic;
        controlCC = controller + cc;
        controlUltimate = controller + ultimate;

    }

    // Open the menu of the controller
    public void StartMenu(CharacterMenu charMenu)
    {
        menuCharacter = charMenu;
        menu = true;
        game = false;
    }

    // Start the game
    public GameObject StartGame(Transform parentTransform, Vector3 spawnPoint, Quaternion spawnRotation, SettingsLevel settings)
    {
        // Create the character at the right spawnlocation
        GameObject current = Instantiate(currentCharacter, spawnPoint, spawnRotation);
        current.transform.parent = parentTransform;
        character = current.GetComponent<BasicMovement>();

        // Set the name+character name
        settings.SetCharacter(name, currentCharacter.name);
        character.Settings(settings);

        // Set the right spawnpoint to the respawn location
        character.spawnPoint = spawnPoint;
        character.rotationPoint = spawnRotation;

        // Reset the menu
        if(menuCharacter != null)
        {
            name = menuCharacter.name;
            clr = menuCharacter.clr;
        }
        // Set the color of the character
        character.SetBasics(name, clr);

        // Set the right game state
        game = true;
        menu = false;

        // Return the current character that the controller controls
        return current;
    }

    // Stop the game
    public void StopGame()
    {
        character = null;
        menu = true;
        game = false;
    }
}
