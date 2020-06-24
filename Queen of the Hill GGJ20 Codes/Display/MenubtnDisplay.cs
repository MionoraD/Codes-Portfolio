using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenubtnDisplay : MonoBehaviour
{
    Button btnMenu;
    GameManager gmc;

    // Start is called before the first frame update
    void Start()
    {
        btnMenu = gameObject.GetComponent<Button>();
        btnMenu.onClick.AddListener(OpenMenu);

        gmc = FindObjectOfType<GameManager>();
    }

    void OpenMenu()
    {
        gmc.OpenMainMenu();
    }
}
