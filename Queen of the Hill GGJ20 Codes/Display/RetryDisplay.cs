using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RetryDisplay : MonoBehaviour
{
    Button btnRetryGame;
    GameManager gmc;

    // Start is called before the first frame update
    void Start()
    {
        btnRetryGame = gameObject.GetComponent<Button>();
        btnRetryGame.onClick.AddListener(RetryGame);

        gmc = FindObjectOfType<GameManager>();
    }

    void RetryGame()
    {
        gmc.LoadLevel(gmc.getPlayerName());
    }
}
