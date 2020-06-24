using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticePlayer : MonoBehaviour
{
    private bool alert = false;
    
    private GameObject watchOverItem;
    [SerializeField] private Guard guard;

    private float step;
    [SerializeField] private float duration = 2.5f;

    private Color currentColor = Color.white;
    [SerializeField] private Color ignoreColor = Color.white;
    [SerializeField] private Color alertColor = Color.black;
    private Color startColor, endColor;

    private Renderer rend;

    private Vector3 currentScale;
    private Vector3 smallScale;
    private Vector3 biggScale;

    void Start()
    {
        rend = gameObject.GetComponent<Renderer>();
        watchOverItem = guard.watchOverItem;

        step = 0;
        startColor = currentColor;
        endColor = ignoreColor;

        smallScale = transform.localScale;
        biggScale = smallScale + new Vector3(.5f, 0, 1f);
    }

    void Update()
    {
        if (step < duration)
        {
            currentColor = Color.Lerp(startColor, endColor, step);
            transform.localScale = Vector3.Lerp(transform.localScale, currentScale, step);
            rend.material.color = currentColor;
            step += Time.deltaTime / duration;
        }
        else
        {
            bool active = true;
            if (watchOverItem != null) active = watchOverItem.activeSelf;

            if (active && !guard.recognize)
            {
                startColor = currentColor;
                endColor = ignoreColor;
                step = 0;

                currentScale = smallScale;

                alert = false;
            }
            else
            {
                startColor = currentColor;
                endColor = alertColor;
                step = 0;

                currentScale = biggScale;

                alert = true;
            }
        }
    }

    // The guard hits something
    private void OnTriggerEnter(Collider other)
    {
        // Check if the guard hits the player
        if (other.tag.Equals("Player"))
        {
            PlayerMovement playerControl = other.gameObject.GetComponent<PlayerMovement>();
            if(playerControl != null)
            {
                // If the player acts suspicious (runs) or if the guard is in alert mode
                if (playerControl.suspicious || alert)
                    // Notice the player
                    guard.NoticePlayer(other.gameObject.transform);
            }
        }
    }
}
