using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBall : MonoBehaviour
{
    // For the basic attack
    private bool fire = false;
    [SerializeField] private float speed = 50.0f;
    private float startTime;
    private float journeyLength;
    private Vector3 startMarker, endMarker;

    [SerializeField] private GameObject magicExplosionPrefab;

    private PlayerManager mPlayer;

    private int dmg;

    void Start()
    {
        startTime = Time.time;
        startMarker = transform.position;
        journeyLength = Vector3.Distance(startMarker, endMarker);
    }

    void Update()
    {
        if (fire)
        {
            // Distance moved = time * speed.
            float distCovered = (Time.time - startTime) * speed;

            // Fraction of journey completed = current distance divided by total distance.
            float fracJourney = distCovered / journeyLength;

            // Set our position as a fraction of the distance between the markers.
            transform.position = Vector3.Lerp(startMarker, endMarker, fracJourney);

            if (transform.position == endMarker)
            {   // At the end of the road
                GameObject explosion = Instantiate(magicExplosionPrefab, transform.position, Quaternion.identity);

                OnTrigger trigger = explosion.GetComponent<OnTrigger>();
                trigger.SetPlayerDmg(mPlayer, dmg);

                Destroy(gameObject);
            }
        }
    }

    public void FireMagic(Vector3 end, PlayerManager pManager, int damage)
    {
        mPlayer = pManager;
        dmg = damage;
        
        endMarker = end;
        transform.LookAt(endMarker);
        fire = true;
    }
}
