using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    private GameManager gmc;

    [SerializeField] private Button moveon, endgame;

    // Start is called before the first frame update
    void Start()
    {
        gmc = FindObjectOfType<GameManager>();
        moveon.onClick.AddListener(gmc.UnloadPauseMenu);
        endgame.onClick.AddListener(gmc.EndLevelPauseMenu);
    }
}
