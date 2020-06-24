using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A fieldblock is a position that the camera may look at
// This code checks if there are any players in the fieldblock

public class FieldBlock : MonoBehaviour
{
    [SerializeField] private int _id;
    [SerializeField] private Vector3 cameraPosition;

    // How many players are in the field?
    // Is used by ControlCamera to see if there are players in the field
    [HideInInspector] public int crowd;

    // Start is called before the first frame update
    void Start()
    {
        crowd = 0;
    }

    // If a player enters the field
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;
        crowd++;
    }

    // If a player exits the field
    private void OnTriggerExit(Collider other)
    {
        if(other.tag != "Player") return;
        crowd--;
    }
}
