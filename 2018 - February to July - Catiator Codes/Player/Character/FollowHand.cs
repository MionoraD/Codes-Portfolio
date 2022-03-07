using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHand : MonoBehaviour
{
    [SerializeField] private Transform hand;

    void Update()
    {
        transform.position = hand.position;
        transform.eulerAngles = hand.eulerAngles;
    }
}
