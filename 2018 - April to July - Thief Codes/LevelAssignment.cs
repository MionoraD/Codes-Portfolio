using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelAssignment : MonoBehaviour
{
    private bool hasAssignment = false;

    [SerializeField] private LevelManager level;

    [SerializeField] private Text blueTotal;
    [SerializeField] private Transform locationBlue;

    [SerializeField] private Text redTotal;
    [SerializeField] private Transform locationRed;

    [SerializeField] private GameObject levelUI;
    [SerializeField] private GameObject assignmentUI;

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            level.CanPlay = false;
            levelUI.SetActive(false);
        }

        if (level.HasCounted && !hasAssignment)
        {
            blueTotal.text = level.totalBlue;
            Instantiate(level.Blue, locationBlue);

            redTotal.text = level.totalRed;
            Instantiate(level.Red, locationRed);

            hasAssignment = true;
        }
    }

    public void PlayGame()
    {
        levelUI.SetActive(true);
        level.CanPlay = true;
        assignmentUI.SetActive(false);
    }
}
