using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ModeDisplay : MonoBehaviour
{
    GameManager gmc;
    Text text;

    [SerializeField] GameObject iconattack;
    [SerializeField] GameObject iconrepair;
    [SerializeField] GameObject iconbuild;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        gmc = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        string mode = gmc.getMode().ToString();

        iconattack.SetActive(false);
        iconrepair.SetActive(false);
        iconbuild.SetActive(false);

        if (mode.Equals("repair")) iconrepair.SetActive(true);
        if (mode.Equals("attack")) iconattack.SetActive(true);
        if (mode.Equals("build")) iconbuild.SetActive(true);

        text.text = mode;
    }
}
