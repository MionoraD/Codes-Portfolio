using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundsDisplay : MonoBehaviour
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
        int rounds = Tracker._instance.RoundsSurvived() + 1;
        text.text = rounds.ToString();
    }
}
