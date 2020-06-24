using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierDisplay : MonoBehaviour
{
    GameManager gmc;
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        gmc = FindObjectOfType<GameManager>();
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = gmc.getSoldierParts().ToString();
    }
}
