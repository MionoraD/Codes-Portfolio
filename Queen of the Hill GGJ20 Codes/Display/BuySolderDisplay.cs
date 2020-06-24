using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuySolderDisplay : MonoBehaviour
{
    Button btnBuyPart;
    GameManager gmc;

    // Start is called before the first frame update
    void Start()
    {
        btnBuyPart = gameObject.GetComponent<Button>();
        btnBuyPart.onClick.AddListener(BuySoldierPart);

        gmc = FindObjectOfType<GameManager>();
    }

    void BuySoldierPart()
    {
        gmc.BuySoldierPart();
    }
}
