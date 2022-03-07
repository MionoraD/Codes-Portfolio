using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon
{
    private bool canShoot = true;
    [SerializeField] private float timeBetweenShots = 1.0f;

    // For the basic shot
    private Vector3 direction;
    [SerializeField] private Transform shootTowards;
    [SerializeField] private Transform placeholderArrow;
    [SerializeField] private GameObject prefabArrow;

    //Audio
    private AudioManager audioManager;

    [Header("Animation & audio names")]
    [SerializeField] private string animationShoot = "BasicShoot";
    [SerializeField] private string audioShoot = "Ranger_Hit";

    [SerializeField] private BasicMovement basic;

    [SerializeField] private float timeShootingArrow = 0.5f;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public override void UseWeaponBasic(Animator anim)
    {
        if (canShoot)
        {
            anim.SetTrigger(animationShoot);
            StartCoroutine(ShootArrow(timeShootingArrow));
            StartCoroutine(BetweenShots());
        }
    }

    public IEnumerator ShootArrow(float seconds)
    {
		direction = shootTowards.position;

        yield return new WaitForSeconds(seconds);

        //hit Audio
        audioManager.Play(audioShoot);

        GameObject newObject = Instantiate(prefabArrow, placeholderArrow.position, Quaternion.identity);
        newObject.transform.localScale = placeholderArrow.lossyScale;

        Arrow arrowScript = newObject.GetComponent<Arrow>();
        if(arrowScript != null)
        {
            arrowScript.ShootingArrow(direction, mPlayer, dmgBasic);
        }

        MagicBall magicScript = newObject.GetComponent<MagicBall>();
        if(magicScript != null)
        {
            magicScript.FireMagic(direction, mPlayer, dmgBasic);
        }
    }

    private IEnumerator BetweenShots()
    {
        canShoot = false;
        yield return new WaitForSeconds(timeBetweenShots);
        canShoot = true;
    }
}
