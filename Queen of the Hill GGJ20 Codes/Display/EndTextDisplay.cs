using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTextDisplay : MonoBehaviour
{
    Text text;
    GameManager gmc;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        gmc = FindObjectOfType<GameManager>();

        text.text = gmc.getEndText();
    }
}
