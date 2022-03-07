using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private Image circle;

    [SerializeField] private Image classImage;
    [SerializeField] private Text classText;

    [SerializeField] private Sprite sword;
    [SerializeField] private Sprite bow;
    [SerializeField] private Sprite magic;
    [SerializeField] private Sprite knife;

    [SerializeField] private GameObject bellCotainer;
    [SerializeField] private List<GameObject> bells = new List<GameObject>();

    private PlayerManager pManager;
    private BasicMovement movement;

    public void SetPlayer(PlayerManager mPlayer)
    {
        pManager = mPlayer;
        movement = pManager.gameObject.GetComponent<BasicMovement>();

        // Set player color
        Color clr = movement.clr;
        background.color = clr;
        circle.color = clr;

        //Set player class
        string playerClass = movement.character;

        classText.text = playerClass;

        if (playerClass.Equals("Berserker"))
            classImage.sprite = sword;
        if (playerClass.Equals("Ranger"))
            classImage.sprite = bow;
        if (playerClass.Equals("Mage"))
            classImage.sprite = magic;
        if (playerClass.Equals("Rogue"))
            classImage.sprite = knife;

        classText.gameObject.SetActive(false);
        bellCotainer.SetActive(true);
    }

    public void SetPlayer(string playerClass, Color playerClr)
    {
        background.color = playerClr;
        circle.color = playerClr;

        classText.text = playerClass;

        if (playerClass.Equals("Berserker"))
            classImage.sprite = sword;
        if (playerClass.Equals("Ranger"))
            classImage.sprite = bow;
        if (playerClass.Equals("Wizard"))
            classImage.sprite = magic;
        if (playerClass.Equals("Rogue"))
            classImage.sprite = knife;
    }

    public void ShowBells()
    {
        classText.gameObject.SetActive(false);
        bellCotainer.SetActive(true);
    }

    void Update()
    {
        if (pManager == null) return;
        UpdateBells(pManager.HasKilled);
    }

    private void UpdateBells(int kills)
    {
        int nr = 1;
        foreach(GameObject bell in bells)
        {
            if (nr <= kills) bell.SetActive(true);
            else bell.SetActive(false);

            nr++;
        }
    }
}
