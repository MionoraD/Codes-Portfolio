using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSphere : Weapon
{
    // For the basic shot
    private Vector3 direction;
    [SerializeField] private Transform fireTowards;
    [SerializeField] private GameObject prefabMagicBall;

    public override void Update()
    {
        base.Update();
        direction = fireTowards.position;
    }

    public override void UseWeaponBasic(Animator anim)
    {
        GameObject newBall = Instantiate(prefabMagicBall, transform.position, transform.rotation);
        newBall.transform.localScale = transform.lossyScale;

        MagicBall magicScript = newBall.GetComponent<MagicBall>();
        magicScript.FireMagic(direction, mPlayer, dmgBasic);
    }
}
