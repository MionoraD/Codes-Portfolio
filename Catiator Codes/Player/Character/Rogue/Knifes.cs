using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knifes : Weapon
{
    private Animator animations;

    void Start()
    {
        animations = GetComponent<Animator>();
    }

    public override void UseWeaponBasic(Animator anim)
    {
        Transform leftKnife = transform.Find("LeftKnife");
        OnTrigger leftTrigger = leftKnife.GetComponent<OnTrigger>();
        leftTrigger.SetPlayerDmg(mPlayer, dmgBasic);

        Transform rightKnife = transform.Find("RightKnife");
        OnTrigger rightTrigger = rightKnife.GetComponent<OnTrigger>();
        rightTrigger.SetPlayerDmg(mPlayer, dmgBasic);

        animations.SetTrigger("BasicAttack");
    }

    public override void UseWeaponCC()
    {
        Debug.Log("CC Knife");
    }

    public override void UseWeaponUltimate()
    {
        Debug.Log("Ultimate Knife");
    }
}
