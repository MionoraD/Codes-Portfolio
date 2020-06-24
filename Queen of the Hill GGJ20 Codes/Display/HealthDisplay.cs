using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    Text text;
    GameManager gmc;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        gmc = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = gmc.getHealth().ToString();
    }
}
