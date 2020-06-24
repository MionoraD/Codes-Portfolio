using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyWorkerDisplay : MonoBehaviour
{
    Button btnBuyPart;
    GameManager gmc;

    // Start is called before the first frame update
    void Start()
    {
        btnBuyPart = gameObject.GetComponent<Button>();
        btnBuyPart.onClick.AddListener(BuyWorkerPart);

        gmc = FindObjectOfType<GameManager>();
    }

    void BuyWorkerPart()
    {
        gmc.BuyWorkerPart();
    }
}
