using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private bool shoot = false;
    [SerializeField] private float speed = 50.0f;
    
    private float startTime;
    private float journeyLength;
    private Vector3 startMarker, endMarker;
    [SerializeField] private Transform arrowPointCollider;

    void Start()
    {
        startTime = Time.time;
        startMarker = transform.position;
        journeyLength = Vector3.Distance(startMarker, endMarker);
    }

    void Update()
    {
        if (shoot)
        {
            // Distance moved = time * speed.
            float distCovered = (Time.time - startTime) * speed;

            // Fraction of journey completed = current distance divided by total distance.
            float fracJourney = distCovered / journeyLength;

            // Set our position as a fraction of the distance between the markers.
            transform.localPosition = Vector3.Lerp(startMarker, endMarker, fracJourney);

            float size = fracJourney * 10;
            arrowPointCollider.localScale = new Vector3(size*3, arrowPointCollider.localScale.y, size);

            if (transform.position == endMarker)
            {
                Destroy(gameObject);
            }
        }
    }

    public void ShootingArrow(Vector3 end, PlayerManager mPlayer, int dmg)
    {
        Transform collider = transform.Find("ArrowPointCollider");
        if(collider != null)
        {
            OnTrigger trigger = collider.GetComponent<OnTrigger>();
            trigger.SetPlayerDmg(mPlayer, dmg);
        }

        endMarker = end;

        transform.LookAt(endMarker);

        shoot = true;
    }
}
