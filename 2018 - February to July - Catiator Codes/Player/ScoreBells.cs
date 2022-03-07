using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBells : MonoBehaviour
{
    [SerializeField] private List<GameObject> bells = new List<GameObject>();

    [SerializeField] private List<Image> backgrounds = new List<Image>();

    [SerializeField] private Text positionText;

    [SerializeField] private Transform winnerPos;
    [SerializeField] private Transform loserPos;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject bell in bells)
        {
            bell.SetActive(false);
        }
    }

    public void SetBells(int bellsOn)
    {
        if (bells.Count <= 0) return;

        int nr = 1;
        foreach (GameObject bell in bells)
        {
            if (bellsOn >= nr) bell.SetActive(true);
            nr++;
        }
    }

    public void SetBackground(Color clr)
    {
        if (backgrounds.Count <= 0) return;

        foreach(Image bg in backgrounds)
        {
            bg.color = clr;
        }
    }

    public void SetPosition(int nr)
    {
        positionText.text = "" + nr;
    }

    public void SetPlayer(bool won, GameObject character)
    {
        Transform parent = loserPos;
        if (won) parent = winnerPos;

        character.transform.position = parent.position;
        character.transform.rotation = parent.rotation;
        character.transform.parent = parent;
    }
}
