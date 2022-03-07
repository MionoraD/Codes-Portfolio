using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Level;
    private bool play = false;
    [SerializeField] private PlayerMovement player;
    public bool CanPlay
    {
        get { return play; }
        set
        {
            play = value;
            if (!play) player.StopAnimations();
        }
    }

    [SerializeField] private Inventory inventory;

    [SerializeField] private string piece;
    public string ChessPieceName
    {
        get { return piece; }
    }

    [SerializeField] private GameObject redPiece;
    public GameObject Red
    {
        get { return redPiece; }
        private set { redPiece = value; }
    }
    [HideInInspector] public string totalRed;

    [SerializeField] private GameObject bluePiece;
    public GameObject Blue
    {
        get { return bluePiece; }
        private set { bluePiece = value; }
    }
    [HideInInspector] public string totalBlue;

    private bool counted = false;
    public bool HasCounted
    {
        get { return counted; }
        private set { counted = value; }
    }

    [Header("Level map")]
    [SerializeField] private string mapName;
    [SerializeField] private GameObject levelMapUI;

    [Header("Red piece")]
    [SerializeField] private Transform locationRedUI;
    [SerializeField] private Text currentRedPieces;
    [SerializeField] private Text totalRedPieces;

    [Header("Blue piece")]
    [SerializeField] private Transform locationBlueUI;
    [SerializeField] private Text currentBluePieces;
    [SerializeField] private Text totalBluePieces;

    [Header("Next location")]
    [SerializeField] private GameObject nextLocationUI;
    private bool hasFoundNextLocation = false;
    [SerializeField] private Text locationText;

    [Header("Road Control")]
    [SerializeField] private Road road;

    [Header("Assignment")]
    [SerializeField] private GameObject assignmentUI;

    [Header("Reset level")]
    [SerializeField] private Transform parentItems;

    [Header("End level")]
    [SerializeField] private EndLevel end;

    [Header("Message")]
    [SerializeField] private Message message;

    void Awake()
    {
        Level = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(Red, locationRedUI);
        Instantiate(Blue, locationBlueUI);
        levelMapUI.SetActive(false);
        nextLocationUI.SetActive(false);

        UpdateRoad();
        HasCounted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!HasCounted && inventory.FilledPieces)
        {
            totalRed = "" + inventory.CountColorPieces(piece, "Red");
            totalRedPieces.text = totalRed;
            currentRedPieces.text = "0";

            totalBlue = "" + inventory.CountColorPieces(piece, "Blue");
            totalBluePieces.text = totalBlue;
            currentBluePieces.text = "0";

            HasCounted = true;
        }

        if (!assignmentUI.activeSelf && !CanPlay && !end.finishedLevel) assignmentUI.SetActive(true);

        if (assignmentUI.activeSelf || end.finishedLevel) levelMapUI.SetActive(false);
    }

    public void UpdateUI()
    {
        currentRedPieces.text = "" + inventory.CountCurrentColorPieces(piece, "Red");
        currentBluePieces.text = "" + inventory.CountCurrentColorPieces(piece, "Blue");

        if (inventory.HasFoundMap(mapName)) levelMapUI.SetActive(true);

        if (end.CheckFinished()) nextLocationUI.SetActive(true);
    }

    public void UpdateRoad()
    {
        StartCoroutine(UpdatingRoad());
    }
    private IEnumerator UpdatingRoad()
    {
        yield return new WaitForSeconds(.1f);
        road.UpdateRoad();
    }

    public void ResetLevel()
    {
        foreach(Transform child in parentItems)
        {
            ItemControl control = child.GetComponent<ItemControl>();
            if(control != null)
            {
                child.gameObject.SetActive(true);
                control.ResetItem();
            }

            end.hasStarted = false;
        }

        CanPlay = false;
    }

    public void GiveMessage(string msg)
    {
        message.ShowMessage(msg);
    }
}
