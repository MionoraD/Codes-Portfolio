using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum inputMode { attack, repair, build }
public enum gameState {  menu, playing, pause, died }
public class GameManager : MonoBehaviour
{
    private string playerName = "";
    public string getPlayerName() { return playerName; }
    private gameState currentState = gameState.menu;

    public static GameManager _instance;
    [SerializeField]inputMode ModeSelector;
    
    [Range(0f, 2f)] [SerializeField] float gameSpeed=1f;
    [SerializeField] int Points, Coins, WorkerParts, SoldierParts;
    [SerializeField] int health = 3;
    private string endtxt;
    int timeActive;

    // Worker stuff
    [SerializeField] private int workerPartCost = 5;
    [SerializeField] private int workerbuildCost = 3;
    public int getWorkerBuildCost() { return workerbuildCost; }
    [SerializeField] private int workerrepairCost = 1;
    public int getWorkerRepairCost() { return workerrepairCost; }

    // Solder stuff
    [SerializeField] private int soldierPartCost = 0;
    [SerializeField] private int soldierBuildCost = 0;
    public int getSoldierBuildCost() { return soldierBuildCost; }
    [SerializeField] private int soldierRepairCost = 0;
    public int getSoldierRepairCost() { return soldierRepairCost; }

    // To find behavior for UI
    private List<PartBehavior> wallbehaviorCodes;
    public PartBehavior getBehavior(int index) { return wallbehaviorCodes[index]; }

    [SerializeField] private List<Round> rounds = new List<Round>();
    public List<Round> getRoundsList() { return rounds; }

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        timeActive = Mathf.RoundToInt(Time.timeSinceLevelLoad);
        SceneManager.LoadScene(1, LoadSceneMode.Additive);  // Load Menu
        currentState = gameState.menu;
    }

    public gameState getGameState()
    {
        return currentState;
    }
    public void setGameState(gameState newState)    
    {
        currentState = newState;
    }
    // Loading the level
    public void LoadLevel(string player)
    {
        if (player.Equals("Insert your name..."))
            playerName = "Anonymous";
        else 
            playerName = player;

        Debug.Log(playerName + " starts game");
        StartCoroutine(LoadingLevel(5));
    }

    IEnumerator LoadingLevel(int level)
    {
        Tracker._instance.StartGame();

        SceneManager.LoadScene(level, LoadSceneMode.Additive);
        yield return new WaitForSeconds(0.5f);

        // To communicate the walls to the UI
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        wallbehaviorCodes = new List<PartBehavior>();
        for(int i=0; i<walls.Length; i++)
        {
            PartBehavior behavior = walls[i].GetComponent<PartBehavior>();
            if (behavior != null) wallbehaviorCodes.Add(behavior);
        }

        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(4, LoadSceneMode.Additive);
        currentState = gameState.playing;

        Scene scene = SceneManager.GetSceneByBuildIndex(1);
        if(scene.isLoaded) SceneManager.UnloadSceneAsync(1);

        scene = SceneManager.GetSceneByBuildIndex(2);
        if (scene.isLoaded) SceneManager.UnloadSceneAsync(2);
    }

    // Pause menu stuff
    public void PauseGame()
    {
        if (currentState == gameState.pause) return;
        currentState = gameState.pause;
        SceneManager.LoadScene(3, LoadSceneMode.Additive);
    }

    public void UnloadPauseMenu()
    {
        currentState = gameState.playing;
        SceneManager.UnloadSceneAsync(3);
    }

    public void EndLevelPauseMenu()
    {
        UnloadPauseMenu();
        EndLevel("You needed a break!!!");
    }

    // End level stuff

    public void EndLevel(string endText)
    {
        endtxt = endText;
        StartCoroutine(LoadEndGame(5));
    }

    IEnumerator LoadEndGame(int level)
    {
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
        currentState = gameState.died;
        yield return new WaitForSeconds(0.2f);
        SceneManager.UnloadSceneAsync(4);
        SceneManager.UnloadSceneAsync(level);
    }

    public void OpenMainMenu()
    {
        Scene scene = SceneManager.GetSceneByBuildIndex(2);
        if(scene.isLoaded) SceneManager.UnloadSceneAsync(2);

        scene = SceneManager.GetSceneByBuildIndex(6);
        if (scene.isLoaded) SceneManager.UnloadSceneAsync(6);

        StartCoroutine("LoadMenu");
    }

    IEnumerator LoadMenu()
    {
        yield return new WaitForSeconds(0.2f);
        currentState = gameState.menu;
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }

    public void LoadHighScoreScreen()
    {
        SceneManager.LoadScene(6, LoadSceneMode.Additive);

        Scene scene = SceneManager.GetSceneByBuildIndex(1);
        if (scene.isLoaded) SceneManager.UnloadSceneAsync(1);

        scene = SceneManager.GetSceneByBuildIndex(2);
        if (scene.isLoaded) SceneManager.UnloadSceneAsync(2);

        currentState = gameState.menu;
    }

    // input mode
    public inputMode getMode()
    {
        return ModeSelector;
    }
    // Point stuff
    public int getPoints()
    {
        return Points;
    }
    public void addPoints(int pointsToAdd)
    {
        Points += pointsToAdd;
    }
    // Coin stuff
    public int getCoins()
    {
        return Coins;
    }
    public void addCoin(int value)
    {
        Tracker._instance.CollectCoin();
        Coins += value;
    }
    public void removeCoin(int value)
    {
        Coins -= value;
    }
    // Worker Part stuff
    public int getWorkerParts()
    {
        return WorkerParts;
    }
    public void addWorkerPart()
    {
        WorkerParts++;
    }
    public void collectWorkerPart()
    {
        Tracker._instance.ReturnPart();
        addWorkerPart();
    }
    // Soldier Part stuff
    public int getSoldierParts()
    {
        return SoldierParts;
    }
    public void addSoldierParts(int value)
    {
        SoldierParts += value;
    }
    // Health stuff
    public int getHealth()
    {
        return health;
    }
    public void addHealth(int value)
    {
        health += value;
    }
    public void removeHealth(int value)
    {
        health -= value;
        if (health <= 0) EndLevel("You died!!!");
    }
    // End text for end scene
    public string getEndText()
    {
        return endtxt;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == gameState.playing)
        {
            if (Input.GetKey(KeyCode.R))
            {
                ModeSelector = inputMode.repair;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                ModeSelector = inputMode.build;
            }
            else
            {
                ModeSelector = inputMode.attack;
            }

            if (Input.GetKey(KeyCode.Escape))
            {
                PauseGame();
            }
        }

        if (currentState == gameState.pause) Time.timeScale = 0;
        else Time.timeScale = 1;
    }


    // Pay/buy worker stuff
    public void BuyWorkerPart()
    {
        if (Coins >= workerPartCost)
        {
            addWorkerPart();
            removeCoin(workerPartCost);
        }
        else
        {
            Debug.Log("Not enough coins!");
        }
    }

    public bool PayWorkerCost(int cost)
    {
        if (cost <= WorkerParts)
        {
            WorkerParts -= cost;
            return true;
        }
        Debug.Log("Not enough parts!");
        return false;
    }

    // Pay/buy solder stuff
    public void BuySoldierPart()
    {
        Debug.Log("Cannot buy solders yet!!!");
        return;

        if (Coins >= soldierPartCost)
        {
            addWorkerPart();
            removeCoin(soldierPartCost);
        }
        else
        {
            Debug.Log("Not enough coins!");
        }
    }
    public bool PaySolderCost(int cost)
    {
        if (cost <= SoldierParts)
        {
            WorkerParts -= cost;
            return true;
        }
        return false;
    }
}
