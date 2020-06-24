using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartBehavior : MonoBehaviour
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

    private int state = 3;
    public int State
    {
        get { return state; }
        private set {
            state = value;
            UpdateState();
        }
    }

    [SerializeField] private Sprite sprTrenchFull;
    [SerializeField] private Sprite sprTrenchBroken;
    [SerializeField] private SpriteRenderer sprRenderer;
    [SerializeField] private List<GameObject> antList;

    private GameManager gmc;

    // Start is called before the first frame update
    void Start()
    {
        wallcollider = gameObject.GetComponent<Collider>();
        UpdateState();

        gmc = FindObjectOfType<GameManager>();

        buildcost = gmc.getWorkerBuildCost();
        repaircost = gmc.getWorkerRepairCost();
    }

    // Update is called once per frame
    void Update()
    {
        if(repairing)
        {
            Building(repairTimer);
        }
        if (building)
        {
            Building(buildTimer);
        }
    }

    private void UpdateState()
    {
        if (State == 0) sprRenderer.sprite = sprTrenchBroken;
        else sprRenderer.sprite = sprTrenchFull;

        for (int i = 0; i < antList.Count; i++)
        {
            if ((State - 1) >= i) antList[i].SetActive(true);
            else antList[i].SetActive(false);
        }
    }

    // Enemy steals part
    public bool StealPart()
    {
        if (State == 0) return false;
        State--;

        return true;
    }

    // Start repairing
    public void RepairPart()
    {
        if (State == 0 || State == 3) return; // If wall needs to be rebuild

        wallcollider.isTrigger = false;

        // Check if able to repair
        if (gmc.PayWorkerCost(repaircost))
        {
            _timer = 0;
            repairing = true;
            laststate = State;
            AudioSource.PlayClipAtPoint(Repairclip, transform.position);
        }
    }

    // For buying or recollecting part
    public void BuildPart()
    {
        if (State != 0) return; // If wall needs to be repaired

        wallcollider.isTrigger = false;

        // Check if able to repair
        if (gmc.PayWorkerCost(buildcost))
        {
            _timer = 0;
            building = true;
            laststate = State;
        }
    }

    // Buy wall
    float inbetweentimer = .25f;
    float timebetween = 0;
    int laststate = 0;
    public void Building(float timer)
    {
        _timer += Time.deltaTime;
        timebetween += Time.deltaTime;
        if (_timer > timer)
        {
            if (repairing)
            {
                Tracker._instance.RepairWall();
                State = laststate + 1;
            }
            else
            {
                Tracker._instance.BuildWall();
                State = 3;
            }
            
            wallcollider.isTrigger = true;
            repairing = false;
            building = false;
            timebetween = 0;
        }
        else if (timebetween > inbetweentimer)
        {
            if(repairing)
            {
                if(State == laststate) State++;
                else State = laststate;
            }

            if (building)
            {
                if (State == 0) State = 1;
                else if (State == 1) State = 2;
                else if (State == 2)
                {
                    if (laststate < 2 && (timer - _timer) > (timer / 2)) State = 1;
                    else State = 3;
                }
                else State = 2;
            }

            timebetween = 0;
        }
    }

    // Capture mouse
    private void OnMouseDown()
    {
        if (gmc.getMode() == inputMode.repair)
        {
            RepairPart();
        }
        if(gmc.getMode() == inputMode.build)
        {
            BuildPart();
        }
    }
}
