using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager SpawnEnemyManager;
     
    [SerializeField] private Transform moveEnemyIntoDirection;
    [SerializeField] private Transform enemySpawnPositions;
    private Transform[] spawnPositionsArray;
    private List<Transform> spawnPositions;
    [SerializeField] private Transform enemySpawnParent;

    private List<GameObject> enemyPool;
    private int currentEnemyIndex = 0;
    [SerializeField] private GameObject prefabBasicEnemy;

    private GameManager gmc;
    [SerializeField] private List<Round> rounds;
    private int currentRoundNr = 0;

    private float timeBeforeNextEnemy = 1;

    private bool roundstarted = false;
    
    // Start is called before the first frame update
    void Start()
    {
        gmc = FindObjectOfType<GameManager>();
        rounds = gmc.getRoundsList();

        SpawnEnemyManager = this;
        enemyPool = new List<GameObject>();

        // Grab spawn positions (remove parent transform)
        spawnPositionsArray = enemySpawnPositions.GetComponentsInChildren<Transform>();
        spawnPositions = new List<Transform>();
        for(int i=1; i<spawnPositionsArray.Length; i++)
        {
            spawnPositions.Add(spawnPositionsArray[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentEnemyIndex > 0)
        {
            SpawnEnemy();
        }
        else
        {
            BetweenRounds();
        }
    }

    // Spawning enemies over time
    private void SpawnEnemy()
    {
        // Timer
        timeBeforeNextEnemy -= Time.deltaTime;
        if (timeBeforeNextEnemy < 0)
        {
            // Grab next enemy from pool
            GameObject enemy = enemyPool[currentEnemyIndex - 1];

            // Put enemy in play
            enemy.SetActive(true);
            EnemyBehaviour behaviour = enemy.GetComponent<EnemyBehaviour>();
            if (behaviour != null) behaviour.StartMoving();

            // Set next timer time
            timeBeforeNextEnemy = Random.Range(2.5f, 5f);
            currentEnemyIndex--;
        }

        roundstarted = false;
    }

    // Remove last round and add next round
    private void BetweenRounds()
    {
        // When the next round has not started yet
        if (!roundstarted)
        {
            // Check if last round has ended
            bool roundEnded = true;
            foreach (GameObject enemy in enemyPool)
            {
                if (enemy.activeSelf)
                {
                    roundEnded = false;
                    return;
                }
            }

            // When the last round has ended
            if (roundEnded)
            {
                StartCoroutine("StartRound");
            }
        }
    }

    // Setup the next round
    IEnumerator StartRound()
    {
        Tracker._instance.SurvivedRound();
        roundstarted = true;

        // Find the current round
        if (currentRoundNr >= rounds.Count) currentRoundNr = 0;
        Round currentRound = rounds[currentRoundNr];

        // Clean up last round
        foreach(GameObject child in enemyPool)
        {
           // no, don't destroy items, disable them (MAD)
            Destroy(child);
        }
        enemyPool.Clear();

        // Wait before building new round
        float randomtime = Random.Range(0.1f, 1/rounds[currentRoundNr].roundSpeed);
        yield return new WaitForSeconds(randomtime);

        // Spawn the basic enemies of this round
        for (int i = 0; i < currentRound.basicEnemies; i++)
        {
            CreateEnemy(prefabBasicEnemy);
        }

        ShuffleEnemyList();
        currentEnemyIndex = enemyPool.Count;

        // Prepare next round
        currentRoundNr++;
    }

    private void CreateEnemy(GameObject prefab)
    {
        GameObject newBasicEnemy = Instantiate(prefab, enemySpawnParent);

        // Random spawn position
        int randomEnemyPosition = Random.Range(0, spawnPositions.Count);
        newBasicEnemy.transform.position = spawnPositions[randomEnemyPosition].position;

        // Random return position
        randomEnemyPosition = Random.Range(0, spawnPositions.Count);
        Transform returnPosition = spawnPositions[randomEnemyPosition];

        // Setup enemy spawn and return positions
        EnemyBehaviour enemyScript = newBasicEnemy.GetComponent<EnemyBehaviour>();
        float randomSpeed = Random.Range(0.5f, rounds[currentRoundNr].enemySpeed);
        if (enemyScript != null) enemyScript.SetTarget(moveEnemyIntoDirection, returnPosition,randomSpeed);

        enemyPool.Add(newBasicEnemy);
        newBasicEnemy.SetActive(false);
    }

    // Set enemies in random order
    private void ShuffleEnemyList()
    {
        for (int i = 0; i < enemyPool.Count; i++)
        {
            GameObject temp = enemyPool[i];
            int randomIndex = Random.Range(i, enemyPool.Count);
            enemyPool[i] = enemyPool[randomIndex];
            enemyPool[randomIndex] = temp;
        }
    }
}


// A round of enemies
[System.Serializable]
public class Round
{
    public string roundName;
    public int basicEnemies;
    public float roundSpeed;
    public float enemySpeed;
    [HideInInspector] public int total;

    public Round(string name, int id)
    {
        roundName = name;
        basicEnemies = id;

        total = basicEnemies;
    }
}
