using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPlayer : MonoBehaviour
{
    [HideInInspector] public Transform loadControllerTo;
    [SerializeField] private PlayersMenu parentCode;
    [SerializeField] private Image backgroundImage;

    [SerializeField] private int id;
    [SerializeField] private string playerName;
    [SerializeField] private bool startOpened;
    [SerializeField] private Color clr;

    private bool started, hasController, hasCharacter;

    [Header("Start Player")]
    [SerializeField] private GameObject playerMenuStart;
    [SerializeField] private Button startPlayer;

    [Header("Controller Player")]
    [SerializeField] private GameObject playerMenuController;
    [SerializeField] private Button controllerPc, controllerStandard, controllerPhone, controllerStop;
    [SerializeField] private GameObject prefabStandardController, prefabPhoneConnection;

	[HideInInspector] public bool hasCurrentControl = false;
    private GameObject currentControl;
    private bool phonecontrol;
    private StandardController stController;
    private ClientServerConnection phoneConnection = null;

    [Header("Load Phone connection")]
    [SerializeField] private GameObject playerMenuLoadConnection;
    [SerializeField] private GameObject ipAddressOutput;
    [SerializeField] private Button donotConnect;

    [Header("Character Selection")]
    [SerializeField] private GameObject playerMenuCharacters;
    [SerializeField] private CharacterMenu characterClassMenu;
    [SerializeField] private Button stopController;
    [SerializeField] private Button btnConfirm, btnCancel;

    // Start is called before the first frame update
    void Start()
    {
        // Force players to start
        if (startOpened)
        {
            controllerStop.gameObject.SetActive(false);
            StartPlayer();
        }
        else
        {
            startPlayer.onClick.AddListener(StartPlayer);
            controllerStop.onClick.AddListener(ReturnToStart);
        }

        // Only player 1 can use the pc
        if(id != 0)
            controllerPc.gameObject.SetActive(false);
        else
            controllerPc.onClick.AddListener(delegate { UseStandardController(true); });

        // Use other controllers
        controllerStandard.onClick.AddListener(delegate { UseStandardController(false); });
        controllerPhone.onClick.AddListener(UsePhoneController);

        // Set color in character menu
        clr = backgroundImage.color;
        characterClassMenu.SetColor(clr);

        //Set IpAddress
        if (ipAddressOutput != null)
        {
            Text ipAddress = ipAddressOutput.GetComponent<Text>();
            ipAddress.text = Server.LocalIPAddress();
        }

        // Return to controllers choice
        donotConnect.onClick.AddListener(ReturnToController);
        stopController.onClick.AddListener(ReturnToController);

        // Confirm/Cancel chosen character
        btnConfirm.onClick.AddListener(ConfirmCharacter);
        btnCancel.onClick.AddListener(CancelCharacter);

        parentCode.UpdateNavigation(true, this);
    }

    // Start the player
    public void StartPlayer()
    {
        playerMenuStart.SetActive(false);
        playerMenuController.SetActive(true);
        started = true;
        parentCode.UpdateNavigation(false, this);
    }

    // Close the player
    public void ReturnToStart()
    {
        playerMenuController.SetActive(false);
        playerMenuStart.SetActive(true);
        started = false;
        parentCode.UpdateNavigation(false, this);
    }

    // Start using standard controls
    public void UseStandardController(bool pc)
    {
        int controllerNr = 0;
        if (!pc) controllerNr = parentCode.AddController();

		StandardController(pc, controllerNr);
    }

	public void UseStandardController(int nr)
	{
		if (parentCode.AddController(nr))
		{
			StandardController(false, nr);
		}
	}

	public void StandardController(bool pc, int controllerNr)
	{
		currentControl = Instantiate(prefabStandardController, loadControllerTo);
		currentControl.name = id + " Standard";

		hasCurrentControl = true;

		stController = currentControl.GetComponent<StandardController>();
		stController.SetupButtons(pc, controllerNr);
		stController.StartMenu(characterClassMenu);

		playerMenuController.SetActive(false);
		OpenPlayerSelection();
	}


    // Start using phone controls
    public void UsePhoneController()
    {
        currentControl = Instantiate(prefabPhoneConnection, loadControllerTo);
        currentControl.name = id + " Connection";

		hasCurrentControl = true;

		phoneConnection = currentControl.GetComponent<ClientServerConnection>();
        if (phoneConnection != null)
        {
            characterClassMenu.PhoneCharacter();
            phoneConnection.SetClientNr(id, clr, characterClassMenu, this);
        }

        playerMenuController.SetActive(false);
        playerMenuLoadConnection.SetActive(true);
    }

    // Connect phone controls
    public void ConnectPhone()
    {
        phonecontrol = true;

        playerMenuLoadConnection.SetActive(false);
        OpenPlayerSelection();
    }

    // Go to player selection
    public void OpenPlayerSelection()
    {
        backgroundImage.enabled = false;
        playerMenuCharacters.SetActive(true);
        parentCode.UpdateNavigation(false, this);
    }

    // Return to control selection
    public void ReturnToController()
    {
        stController = null;
        phoneConnection = null;
        phonecontrol = false;

        Destroy(currentControl);
        currentControl = null;

		hasCurrentControl = false;

		backgroundImage.enabled = true;

        playerMenuLoadConnection.SetActive(false);
        playerMenuCharacters.SetActive(false);
        playerMenuController.SetActive(true);

        parentCode.UpdateNavigation(false, this);
    }

    // Choose character
    public void ConfirmCharacter()
    {
        if (stController != null) stController.Confirm();
        if (phoneConnection != null) phoneConnection.Confirm();
        parentCode.UpdateNavigation(false, this);
    }

    // Cancel chosen character
    public void CancelCharacter()
    {
        if (stController != null) stController.Cancel();
        if (phoneConnection != null) phoneConnection.Cancel();
        parentCode.UpdateNavigation(false, this);
    }

    // Check if there is a chosen character
    public bool CheckChosen()
    {
        bool chosen = false;

        if (started)
        {
            if (stController != null) chosen = !stController.canChoose;
            if (phoneConnection != null) chosen = !phoneConnection.canChoose;
        }
        else
            chosen = true;
        return chosen;
    }

    // Find state menu
    public List<Button> FindState()
    {
        List<Button> buttons = new List<Button>();

        if (started)
        {
            if (hasCurrentControl)
            {
                return null;
            }
            else
            {
                // If the player has no controller yet
                if (id == 0) buttons.Add(controllerPc);
                buttons.Add(controllerStandard);
                buttons.Add(controllerPhone);
                if(!startOpened) buttons.Add(controllerStop);
            }
        }
        else
        {
            // If the player has not started yet
            buttons.Add(startPlayer);
        }

        return buttons;
    }

    public CharacterMenu FindCharacterMenu()
    {
        return characterClassMenu;
    }
}
