using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaspAnimation : MonoBehaviour
{

    [SerializeField] Animator animation;
    [SerializeField] Billboard billboard;

    private string direction;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setDirection(string d)
    {

        switch (d)
        {

            case "SW":
                animation.Play("Wasp_FlyingBack");
                billboard.flip = true;
                direction = "SW";
                break;
            case "SE":
                animation.Play("Wasp_FlyingBack");
                billboard.flip = false;
                direction = "SE";
                break;
            case "NW":
                animation.Play("Wasp_Flying");
                billboard.flip = true;
                direction = "NW";
                break;
            default:
                animation.Play("Wasp_Flying");
                billboard.flip = false;
                direction = "NE";

                break;

        }


    }

    public void goBack()
    {

        billboard.flip = !billboard.flip;

        switch (direction)
        {
            case "SW":
                animation.Play("Wasp_Carry");

                break;
            case "SE":
                animation.Play("Wasp_Carry");
                break;
            case "NW":
                animation.Play("Wasp_CarryBack");

                break;
            default:
                animation.Play("Wasp_CarryBack");

                break;
        }

    }
}
