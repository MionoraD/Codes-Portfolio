using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int levelScene;
    [SerializeField] private GameObject playerload;
    [SerializeField] private Transform parentCharacters;
    [SerializeField] private List<GameObject> characters;
    [SerializeField] private Transform parentSpawnPointCharacters;
    [SerializeField] private PickUpManager pickups;

    [SerializeField] private GameObject prefabPlayerScore;
    [SerializeField] private Transform playerBoardUI;

    [Header("Settings level")]
    private SettingsLevel settings;
    [SerializeField] private float moveSpeed, jumpSpeed, gravity;
    [SerializeField] private int killsNeeded;
	[SerializeField] private int distanceRangerAttack;

    private bool endLevel;

    [SerializeField] private float showMessageSeconds = 5.0f;
    [SerializeField] private GameObject message;


    [Header("For testing")]
    [SerializeField] private bool EndingLevel = false;

    
    // Start is called before the first frame update
    void Start()
    {
        playerload = GameObject.Find("PlayerLoader");
        parentCharacters = transform.Find("Characters");
        settings = new SettingsLevel(moveSpeed, jumpSpeed, gravity, distanceRangerAttack);

        if(playerload != null)
        {
            foreach(Transform child in playerBoardUI)
            {
                Destroy(child.gameObject);
            }

            int i = 0;
            foreach (Transform child in playerload.transform)
            {
                Vector3 spawnPoint = new Vector3(0, 2.5f, 0);
                Quaternion rotationPoint = Quaternion.identity;

                Transform transformSpawnPoint = parentSpawnPointCharacters.GetChild(i);
                if (transformSpawnPoint != null)
                {
                    spawnPoint = transformSpawnPoint.position;
                    rotationPoint = transformSpawnPoint.rotation;
                }

                StandardController stController = child.GetComponent<StandardController>();
                if (stController != null)
                {
                    GameObject character = stController.StartGame(parentCharacters, spawnPoint, rotationPoint, settings);

                    GameObject scoreUI = Instantiate(prefabPlayerScore, playerBoardUI);

                    PlayerManager pManager = character.GetComponent<PlayerManager>();
                    pManager.killsNeeded = killsNeeded;

                    PlayerUI uiPlayer = scoreUI.GetComponent<PlayerUI>();
                    uiPlayer.SetPlayer(pManager);
                    uiPlayer.ShowBells();
                }

                ClientServerConnection phoneConnection = child.GetComponent<ClientServerConnection>();
                if (phoneConnection != null)
                {
                    GameObject character = phoneConnection.StartGame(parentCharacters, spawnPoint, rotationPoint, settings);

                    GameObject scoreUI = Instantiate(prefabPlayerScore, playerBoardUI);

                    PlayerManager pManager = character.GetComponent<PlayerManager>();
                    pManager.killsNeeded = killsNeeded;

                    PlayerUI uiPlayer = scoreUI.GetComponent<PlayerUI>();
                    uiPlayer.SetPlayer(pManager);
                    uiPlayer.ShowBells();
                }

                i++;
            }
        }

        // Switch menu clients
        Server.StartGame();

        // Show message
        StartCoroutine(ShowMessage());

        pickups.SearchForPlayers();

        if (FadeInOut.screen != null)
            FadeInOut.screen.FadeIn();
    }

    private IEnumerator ShowMessage()
    {
        // message.SetActive(true);
        yield return new WaitForSeconds(showMessageSeconds);
        message.SetActive(false);
    }

    public GameObject FindCharacter(int nr)
    {
        return characters[nr];
    }

    void Update()
    {
        if (EndingLevel)
        {
            EndLevel();
            EndingLevel = false;
        }
    }

    public void QuitLevel()
    {
        if (FadeInOut.screen != null)
        {
            StartCoroutine(LevelQuit());
        }
    }

    private IEnumerator LevelQuit()
    {
        FadeInOut.screen.FadeOut();

        yield return new WaitForSeconds(FadeInOut.screen.SetTime);

        GameObject sceneManager = GameObject.Find("SceneManager");
        if (sceneManager != null)
        {
            ManagingScenes mScenes = sceneManager.GetComponent<ManagingScenes>();
            mScenes.Endlevel(levelScene);
        }
    }

    private bool endingLevel = false;
    public void EndLevel()
    {
        if(FadeInOut.screen != null && !endingLevel)
        {
            StartCoroutine(LevelEnd());
            endingLevel = true;
        } 
    }

    private IEnumerator LevelEnd()
    {
        GameObject sceneManager = GameObject.Find("SceneManager");
        if (sceneManager != null)
        {
            ManagingScenes mScenes = sceneManager.GetComponent<ManagingScenes>();
            mScenes.FindScores(parentCharacters);
        }

        while (!EndLevelCamera.cam.MovingOn)
        {
            yield return null;
        }

        FadeInOut.screen.FadeOut();

        yield return new WaitForSeconds(FadeInOut.screen.SetTime);
        
        if (sceneManager != null)
        {
            ManagingScenes mScenes = sceneManager.GetComponent<ManagingScenes>();
            mScenes.Endlevel(levelScene);
        }
    }
}

public class SettingsLevel
{
    public float moveSpeed, jumpSpeed, gravity;
    public string player, character;
	public float distanceRanger;

    public SettingsLevel(float mSpeed, float jSpeed, float gSpeed, int rangerdistance)
    {
        moveSpeed = mSpeed;
        jumpSpeed = jSpeed;
        gravity = gSpeed;
		distanceRanger = rangerdistance;
    }

    public void SetCharacter(string _player, string _character)
    {
        player = _player;
        character = _character;
    }
}