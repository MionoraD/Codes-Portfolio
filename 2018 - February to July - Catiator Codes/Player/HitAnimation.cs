using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAnimation : MonoBehaviour
{

    public Material[] mat;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().materials;
    }

    private void Update()
    {
        GameObject levelManager = GameObject.Find("LevelManager");
        if (levelManager == null)
        {
            EmissionOff();
        }
    }

    public void EmissionOn() 
    {
        for (int i = 0; i < mat.Length; i++)
        {
            mat[i].EnableKeyword ("_EMISSION");
        }
    }

    public void EmissionOff() 
    {
        for (int i = 0; i < mat.Length; i++)
        {
            mat[i].DisableKeyword ("_EMISSION");
        }
    }
}
