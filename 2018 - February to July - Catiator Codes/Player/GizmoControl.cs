using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoControl : MonoBehaviour
{
    [SerializeField] private GameObject gizmo;

    private void Update()
    {
        GameObject levelManager = GameObject.Find("LevelManager");
        if (levelManager == null)
        {
            gizmo.SetActive(false);
        }
        else
        {
            gizmo.SetActive(true);
        }
    }
}
