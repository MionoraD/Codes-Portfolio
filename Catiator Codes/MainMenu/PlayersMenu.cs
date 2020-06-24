using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayersMenu : MonoBehaviour
{
    private Transform loadPlayersTo;
    [HideInInspector] public List<MenuPlayer> players = new List<MenuPlayer>();
    [SerializeField] List<GameObject> menuPlayers = new List<GameObject>();

    private int controllersinuse = 0;
	private bool[] inusecontrollers = new bool[5];

    // Start is called before the first frame update
    void Start()
    {
        GameObject loadPlayers = GameObject.Find("PlayerLoader");
        if (loadPlayers == null) loadPlayersTo = this.transform;
        else
        {
            loadPlayersTo = loadPlayers.transform;
            foreach (Transform child in loadPlayersTo)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        foreach (MenuPlayer player in players)
        {
            player.loadControllerTo = loadPlayersTo;
        }

		for (int i = 0; i < inusecontrollers.Length; i++)
		{
			inusecontrollers[i] = false;
		}
	}

    public int AddController()
    {
		for (int i = 1; i < inusecontrollers.Length-1; i++)
		{
			if (!inusecontrollers[i])
			{
				inusecontrollers[i] = true;
				return i;
			}
		}

		return 5;
    }
	public bool AddController(int nr)
	{
		if (!inusecontrollers[nr])
		{
			inusecontrollers[nr] = true;
			return true;
		}
		else
		{
			return false;
		}
	}

    public bool CanStartGame()
    {
        foreach (MenuPlayer player in players)
        {
            if (!player.CheckChosen()) return false;
        }
        return true;
    }

    public void UpdateNavigation(bool start, MenuPlayer player)
    {
        MenuButtonNavigation navigation = gameObject.GetComponent<MenuButtonNavigation>();
        if(navigation != null)
        {
            navigation.UpdateNavigation(start, player);
        }
    }
}

