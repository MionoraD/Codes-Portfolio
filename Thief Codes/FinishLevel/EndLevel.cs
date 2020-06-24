using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    public static EndLevel end;

    private string chesspiece;
    private string blue = "Blue", red = "Red";
    [SerializeField] private Inventory inventory;

    [SerializeField] private List<GameObject> ui = new List<GameObject>();
    [SerializeField] private GameObject uiFinished;

    [SerializeField] private PlayerMovement player;

    [HideInInspector] public bool finishedLevel = false;
    [HideInInspector] public bool hasStarted = false;

    void Awake()
    {
        end = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        chesspiece = LevelManager.Level.ChessPieceName;
        uiFinished.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            if (CheckFinished())
                FinishLevel();
            else if (hasStarted)
                LevelManager.Level.GiveMessage("Not yet finished with level");
            else hasStarted = true;
        }
    }

    public bool CheckFinished()
    {
        bool finished = false;

        int totalRed = inventory.CountColorPieces(chesspiece, red);
        int haveRed = inventory.CountCurrentColorPieces(chesspiece, red);
        if (totalRed <= haveRed) finished = true;
        
        if (!finished) return finished;

        int totalBlue = inventory.CountColorPieces(chesspiece, blue);
        int haveBlue = inventory.CountCurrentColorPieces(chesspiece, blue);
        if (totalBlue <= haveBlue) finished = true;
        else finished = false;

        Debug.Log(finished);
        return finished;
    }

    public void FinishLevel()
    {
        finishedLevel = true;
        Debug.Log("Player hits end");
        LevelManager.Level.CanPlay = false;

        if (ui.Count > 0)
        {
            foreach(GameObject uiCanvas in ui)
            {
                uiCanvas.SetActive(false);
            }
        }
        uiFinished.SetActive(true);

        player.ResetPlayer();
    }

    public void ResetLevel()
    {
        finishedLevel = false;
        LevelManager.Level.ResetLevel();
        uiFinished.SetActive(false);
    }
}
