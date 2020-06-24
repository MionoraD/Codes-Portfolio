using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkerStateDisplay : MonoBehaviour
{
    [SerializeField] private int index;
    GameManager gmc;
    PartBehavior behavior;

    [SerializeField] private Button btnBuild;
    [SerializeField] private List<GameObject> antList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        gmc = FindObjectOfType<GameManager>();
        behavior = gmc.getBehavior(index);

        btnBuild.onClick.AddListener(behavior.BuildPart);
    }

    // Update is called once per frame
    void Update()
    {
        if (behavior == null) return;

        int state = behavior.State;

        if (state == 0) btnBuild.gameObject.SetActive(true);
        else btnBuild.gameObject.SetActive(false);

        for (int i = 0; i < antList.Count; i++)
        {
            if ((state - 1) >= i) antList[i].SetActive(true);
            else antList[i].SetActive(false);
        }
    }
}
