using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
	[SerializeField] private PlayersMenu menu;
	private List<MenuPlayer> players = new List<MenuPlayer>();

	// Start is called before the first frame update
	void Start()
    {
		players = menu.players;
    }

    // Update is called once per frame
    void Update()
    {
		MenuPlayer playerMenu = null;
		foreach(MenuPlayer player in players)
		{
			if (!player.hasCurrentControl)
			{
				playerMenu = player;
				return;
			}
		}
		
		if (playerMenu == null) return;
		Debug.Log("There is a spot available");

		if (Input.GetButton("J1Basic"))
		{
			Debug.Log("Hit Joystick J1");
			playerMenu.StartPlayer();
			playerMenu.UseStandardController(1);
		}
    }
}
