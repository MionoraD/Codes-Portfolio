using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This shows any health change to the player

public class DamageUI : MonoBehaviour
{
    [SerializeField] private Text dmgText;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 resetPosition;
    private bool move = false;
    public float speed = 0.1F;

    void Start()
    {
        dmgText.gameObject.SetActive(false);
        move = false;
    }

    void Update()
    {
        // If the ui needs to move
        if (move)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, endPosition, Time.deltaTime * speed);

            // If the ui reached its endpoint
            if(transform.localPosition.y >= resetPosition.y)
            {
                // Remove ui from view
                dmgText.gameObject.SetActive(false);
                transform.localPosition = startPosition;
                move = false;
            }
        }
    }

    // Function can be called when player gets damage or health
    public void ShowDamage(float dmg)
    {
        transform.localPosition = startPosition;

        // Find if health is added or removed
        string damage;
        if (dmg < 0)
        {
            dmgText.color = Color.red;
            damage = "";
        }
        else
        {
            dmgText.color = Color.green;
            damage = "+";
        }

        // Change text of ui
        dmgText.text = damage + dmg;

        // Make ui visible
        dmgText.gameObject.SetActive(true);

        // Set all standard positions
        startPosition = transform.localPosition;
        resetPosition = startPosition + new Vector3(0, 300, 0);
        endPosition = startPosition + new Vector3(0, 400, 0);

        // Start moving the ui in Update()
        move = true;
    }
}
