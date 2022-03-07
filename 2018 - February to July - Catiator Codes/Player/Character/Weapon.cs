using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    protected PlayerManager mPlayer;

    [SerializeField] protected int basicDamage;
    protected int dmgBasic;
    [SerializeField] protected float feedbackForceBasic;

    protected int dmgCC;
    protected float feedbackForceCC;

    protected int dmgUltimate;
    protected float feedbackForceUltimate;

    protected float dmgMultiplyer = 0;

    [SerializeField] private Transform hand;

    public virtual void Update()
    {
        if (dmgMultiplyer != 0)
            dmgBasic = Mathf.RoundToInt(basicDamage * dmgMultiplyer);
        else
            dmgBasic = basicDamage;

        if (hand == null) return;

        transform.position = hand.position;
        transform.eulerAngles = hand.eulerAngles;
    }

    public virtual void UseWeaponBasic(Animator anim)
    {
        Debug.Log("Basic Attack");
    }

    public virtual void UseWeaponCC()
    {
        Debug.Log("CC Attack");
    }

    public virtual void UseWeaponUltimate()
    {
        Debug.Log("Ultimate Attack");
    }

    public void SetPlayerManager(PlayerManager pManager)
    {
        mPlayer = pManager;
    }

    public void ChangeMultiplayer(float newDmg)
    {
        dmgMultiplyer = newDmg;
    }
}
