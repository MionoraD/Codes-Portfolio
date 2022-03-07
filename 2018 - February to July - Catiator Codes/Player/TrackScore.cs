using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackScore : MonoBehaviour
{
    [SerializeField] private Text name;

    private PlayerManager pManager;
    private BasicMovement movement;

    [SerializeField] private Text killed;
    [SerializeField] private Text died;

    [SerializeField] private GameObject ccUI;
    [SerializeField] private GameObject ultimateUI;

    [SerializeField] private Image iconCircle;
    [SerializeField] private List<Image> colorCircles;

    [SerializeField] private Sprite ranger;
    [SerializeField] private Sprite berserker;

    [SerializeField] private Image cooldownCC;
    [SerializeField] private Image cooldownUltimate;

    // Update is called once per frame
    void Update()
    {
        if (pManager == null) return;
        killed.text = "K:" + pManager.HasKilled;
        died.text = "D:" + pManager.HasDied;

        if(movement == null) return;

        // ccUI.SetActive(movement.useCC);
        // ultimateUI.SetActive(movement.useUltimate);

        // Cooldown cc
        float calculatecc = movement.ccTimer / movement.ccTime;
        if (calculatecc <= 0) calculatecc = 0;
        else if (calculatecc >= 1) calculatecc = 1;

        cooldownCC.fillAmount = calculatecc;

        // cooldown Ultimate
        float calculateultimate = movement.ultimateTimer / movement.ultimateTime;
        if (calculateultimate <= 0) calculateultimate = 0;
        else if (calculateultimate >= 1) calculateultimate = 1;

        cooldownUltimate.fillAmount = calculateultimate;
    }

    public void SetPlayer(PlayerManager mPlayer)
    {
        pManager = mPlayer;

        Image img = gameObject.GetComponent<Image>();

        movement = mPlayer.gameObject.GetComponent<BasicMovement>();
        img.color = movement.clr;

        ccUI.SetActive(false);
        ultimateUI.SetActive(false);

        foreach(Image circle in colorCircles)
        {
            circle.color = movement.clr;
        }

        if (movement.character.Contains("Ranger"))
        {
            iconCircle.sprite = ranger;
            name.text = "Ranger";
        }
        else if (movement.character.Contains("Berserker"))
        {
            iconCircle.sprite = berserker;
            name.text = "Berserker";
        }
    }
}
