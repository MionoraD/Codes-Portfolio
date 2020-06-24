using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Particle 

public class OnTrigger : MonoBehaviour
{
    [SerializeField] private int dmg = 5;
    [SerializeField] private float feedbackForce;

    List<GameObject> hashit = new List<GameObject>();

    [SerializeField] private PlayerManager playerDoingDamage;

    [SerializeField] private bool destroyOnHit = false;
    [SerializeField] private GameObject destroyObject;

    public void SetPlayerDmg(PlayerManager player, int damage)
    {
        playerDoingDamage = player;
        dmg = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check what the item has hit

        if (other.gameObject.tag.Equals("Player"))
            HittingPlayer(other.gameObject);

        if (other.gameObject.tag.Equals("Enemy"))
            HittingEnemy(other.gameObject);

        if (other.gameObject.tag.Equals("DestroyObjects"))
            HittingObject(other.gameObject);
	}

    private void OnParticleCollision(GameObject other)
    {
        // Check what particle collider has hit

        if (hashit.Contains(other)) return;
        hashit.Add(other);

        if (other.tag.Equals("Player"))
            HittingPlayer(other);
        else if (other.tag.Equals("Enemy"))
            HittingEnemy(other);
        else if (other.tag.Equals("DestroyObjects"))
            HittingObject(other);
		else
			HitDestroy();
    }

    private void HittingPlayer(GameObject other)
    {
        PlayerManager mPlayer = other.GetComponent<PlayerManager>();
        // If another player is hitting them
        if(playerDoingDamage != null)
        {
            // If hitting player and player does not hit themselves
            if(mPlayer != null && mPlayer != playerDoingDamage)
            {
                if (mPlayer.DealDamage(playerDoingDamage, dmg)) // if other character dies by doing damage
                {
                    playerDoingDamage.KilledCharacter(); // Add score
                }

                // Feedback
                Vector3 pushDirection = other.transform.position - transform.position;
                mPlayer.Push(pushDirection, feedbackForce);
            }
        }
        // If anything else hits the player
        else if (mPlayer != null)
        {
            mPlayer.DealDamage(null, dmg);
        }

        HitDestroy();
    }

    public void HittingEnemy(GameObject other)
    {
        Debug.Log("Hit enemy");
        HitDestroy();
    }

    public void HittingObject(GameObject other)
    {
        Debug.Log("Hit obstacle");
        HitDestroy();
    }


    // Remove destroyobject (bullets etc)
    public void HitDestroy()
    {
        if (destroyOnHit)
            Destroy(destroyObject);
    }

}
