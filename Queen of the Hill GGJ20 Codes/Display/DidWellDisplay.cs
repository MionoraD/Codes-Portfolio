using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DidWellDisplay : MonoBehaviour
{
    Text text;
    GameManager gmc;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        gmc = FindObjectOfType<GameManager>();

        text.text = "You did well " + gmc.getPlayerName() + "!";
    }
}
