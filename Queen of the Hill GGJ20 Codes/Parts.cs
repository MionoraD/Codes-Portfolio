using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parts : MonoBehaviour
{
    [SerializeField] int value;
    [Range(1f, 10f)] float timebeforedie = 3f;
    GameManager gmc;
    // Start is called before the first frame update
    void Start()
    {
        gmc = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        timebeforedie -= Time.deltaTime;
        if (0 > timebeforedie) Destroy(this.gameObject);
    }

    private void OnMouseDown()
    {
        gmc.collectWorkerPart();
        Destroy(this.gameObject);
    }
}
