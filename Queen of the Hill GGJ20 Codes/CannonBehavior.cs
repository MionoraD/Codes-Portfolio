using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBehavior : MonoBehaviour
{
    private Collider wallcollider;
    [SerializeField] AudioClip Repairclip;
    private bool repairing = false;
    [SerializeField] private float repairTimer = 2f;
    private bool building = false;
    [SerializeField] private float buildTimer = 4f;
    private float _timer = 0;

    private int buildcost = 0;
    private int repaircost = 0;

    private int state = 0;
    private int stringstate = 0;
    public int getPartState() { return state; }

    private GameManager gmc;

    [SerializeField] private List<GameObject> cannonPartList = new List<GameObject>();
    [SerializeField] private List<GameObject> cannonStringList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        wallcollider = gameObject.GetComponent<Collider>();
        wallcollider.isTrigger = false;
        UpdateState();

        gmc = FindObjectOfType<GameManager>();

        buildcost = gmc.getWorkerBuildCost();
        repaircost = gmc.getWorkerRepairCost();
    }

    // Update is called once per frame
    void Update()
    {
        if (repairing)
        {
            Building(repairTimer);
        }
        if (building)
        {
            Building(buildTimer);
        }

        if(state == 3)
        {
            Shooting();
        }
    }

    private float _shootTimer = 0;
    [SerializeField] private float timebetweenshots = 8;
    private void Shooting()
    {
        _shootTimer += Time.deltaTime;
        if (_shootTimer > timebetweenshots)
        {
            GameObject enemy = FindClosestEnemy();
            if (enemy != null)
            {
                EnemyBehaviour behavior = enemy.GetComponent<EnemyBehaviour>();
                if (behavior != null) behavior.EnemyDies();
            }
            _shootTimer = 0;
        }
    }

    private void UpdateState()
    {
        for (int i = 0; i < cannonPartList.Count; i++)
        {
            if ((state - 1) >= i) cannonPartList[i].SetActive(true);
            else cannonPartList[i].SetActive(false);
        }

        for(int i=0; i< cannonStringList.Count; i++)
        {
            if (stringstate == i) cannonStringList[i].SetActive(true);
            else cannonStringList[i].SetActive(false);
        }
    }

    // Enemy steals part
    public bool StealPart()
    {
        if (state == 0) return false;
        state--;

        UpdateState();

        return true;
    }

    // Start repairing
    public void RepairPart()
    {
        if (state == 0 || state == 3) return; // If wall needs to be rebuild

        wallcollider.isTrigger = false;

        // Check if able to repair
        if (gmc.PayWorkerCost(repaircost))
        {
            _timer = 0;
            repairing = true;
            AudioSource.PlayClipAtPoint(Repairclip, transform.position);
        }
    }

    // For buying or recollecting part
    public void BuildPart()
    {
        if (state != 0) return; // If wall needs to be repaired

        wallcollider.isTrigger = false;

        // Check if able to repair
        if (gmc.PayWorkerCost(buildcost))
        {
            _timer = 0;
            building = true;
        }
    }

    // Buy wall
    public void Building(float timer)
    {
        _timer += Time.deltaTime;
        if (_timer > timer)
        {
            if (repairing) Tracker._instance.RepairGun();
            else Tracker._instance.BuildGun();
            state = 3;
            UpdateState();
            wallcollider.isTrigger = true;
            repairing = false;
            building = false;
        }
    }

    // Capture mouse
    private void OnMouseDown()
    {
        if (gmc.getMode() == inputMode.repair)
        {
            RepairPart();
        }
        if (gmc.getMode() == inputMode.build)
        {
            BuildPart();
        }
    }

    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
}
