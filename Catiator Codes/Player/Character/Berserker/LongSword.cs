using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongSword : Weapon
{
    private Animator animations;

    [SerializeField] private float slashTime = 0.5f;

    [SerializeField] private PlayerManager pManager;
    [SerializeField] private GameObject hit;
    private OnTrigger trigger;
    //Audio
    private AudioManager audioManager;

    private bool slashing = false;
    private Vector3 trailPosition;
    [SerializeField] private GameObject swordTrail;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();

        if (swordTrail != null)
            trailPosition = swordTrail.transform.position;
    }

    public virtual void Start()
    {
        trigger = hit.GetComponent<OnTrigger>();
        hit.SetActive(false);
        if (swordTrail != null)
            swordTrail.SetActive(false);
    }

    public override void Update()
    {
        base.Update();

        if (slashing)
        {
            if (swordTrail != null)
                swordTrail.SetActive(true);
        }
    }

    public override void UseWeaponBasic(Animator anim)
    {
        anim.SetTrigger("BasicSlash");
        int slashnr = anim.GetInteger("SlashNr") + 1;
        if (slashnr >= 3) slashnr = 0;
        anim.SetInteger("SlashNr", slashnr);

        //hit Audio
        audioManager.Play("Berserker_Hit");

        slashing = true;

        trigger.SetPlayerDmg(pManager, dmgBasic);
        StartCoroutine(Slash());
    }

    private IEnumerator Slash()
    {
        hit.SetActive(true);
        yield return new WaitForSeconds(slashTime/2);

        hit.SetActive(false);
        slashing = false;
        // swordTrail.transform.position = trailPosition;
        if (swordTrail != null)
            swordTrail.SetActive(false);
    }

    public override void UseWeaponUltimate()
    {
        Debug.Log("Ultimate Longsword");
    }
}
