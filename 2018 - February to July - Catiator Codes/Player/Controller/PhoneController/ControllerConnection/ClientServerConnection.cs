using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class ClientServerConnection : MonoBehaviour
{
    private string name;
    private int client;
    private Color clr;
    private bool hasClient = false;

    public bool canChoose
    {
        get; private set;
    }
    [SerializeField] private CharacterMenu menuCharacter;
    [SerializeField] private GameObject currentCharacter;

    [SerializeField] private BasicMovement character;

    private Vector3 move = new Vector3(0, 0, 0);
    private bool jump = false, basic = false, cc = false, ultimate = false;
	private Vector3 aiming = new Vector3(0, 0, 0);

    private MenuPlayer menuPlayer;

    public void SetClientNr(int clientNr, Color color, CharacterMenu characterMenu, MenuPlayer playerMenu)
    {
        client = clientNr;
        clr = color;
        menuCharacter = characterMenu;
        hasClient = false;

        menuPlayer = playerMenu;
    }

    public bool Available(int clientNr)
    {
        if (client == clientNr)
            return !hasClient;
        else
            return false;
    }

    public Color FindColor()
    {
        return clr;
    }

    public string ColorString()
    {
        string color = "";

        color += "" + Mathf.Round(clr.r * 1000f);
        color += "|" + Mathf.Round(clr.g * 1000f);
        color += "|" + Mathf.Round(clr.b * 1000f);

        return color;
    }

    public void OpenConnection()
    {
        menuPlayer.ConnectPhone();
    }

    public void Confirm()
    {
        currentCharacter = menuCharacter.ChooseCharacter();
        canChoose = false;
    }

    public void ConfirmCharacter(int clientnr, string character)
    {
        if (client != clientnr) return;
        Debug.Log(character);
        currentCharacter = menuCharacter.ChooseCharacter(character);
        canChoose = false;
    }

    public void Cancel()
    {
        canChoose = true;
        menuCharacter.CancelCharacter();
    }

    public void CancelCharacter(int clientnr)
    {
        if (client != clientnr) return;
        canChoose = true;
        menuCharacter.CancelCharacter();
    }

    public GameObject StartGame(Transform parentTransform, Vector3 spawnPoint, Quaternion spawnRotation, SettingsLevel settings)
    {
        GameObject behaviour = GameObject.Find("Behaviour");

        GameObject current = Instantiate(currentCharacter, spawnPoint, spawnRotation);
        current.transform.parent = parentTransform;
        character = current.GetComponent<BasicMovement>();

        character.spawnPoint = spawnPoint;
        character.rotationPoint = spawnRotation;

        clr = menuCharacter.clr;
        name = menuCharacter.name;
        character.SetBasics(name, clr);
        Debug.Log("Start with name " + name);

        settings.SetCharacter(name, currentCharacter.name);
        character.Settings(settings);

        return current;
    }

    public void UseMove(int clientnr, string[] deltas)
    {
        if (client != clientnr) return;

        if (deltas[2].Equals("Move"))
        {
            string[] vector = deltas[3].Split('/');
            float x = float.Parse(vector[0]) / 1000;
            float z = float.Parse(vector[1]) / 1000;

            move = new Vector3(x, 0, z);

            if (deltas[4].Equals("True"))
                jump = true;
        }
        else if (deltas[2].Equals("Basic"))
        {
            if (deltas[3].Equals("True"))
                basic = true;
            else
                basic = false;
        }
        else if (deltas[2].Equals("CC"))
        {
            cc = true;
        }
        else if (deltas[2].Equals("Ultimate"))
        {
            ultimate = true;
        }
		else if (deltas[2].Equals("Aim"))
		{
			Debug.Log("Aiming");
			string[] vector = deltas[3].Split('/');
			float x = float.Parse(vector[0]) / 1000;
			float z = float.Parse(vector[1]) / 1000;

			aiming = new Vector3(x, 0, z);
		}
    }

    void Update()
    {
        if (character == null) return;

        character.MovingCharacter(move, jump);
		character.RotateCharacter(aiming);
		Debug.Log("aim" + aiming);
        jump = false;

        character.HoldBasic(basic);

        if (cc)
        {
            character.UseCCAttack();
            cc = false;
        }

        if (ultimate)
        {
            character.UseUltimateAttack();
            ultimate = false;
        }
    }
}
