using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneControl : MonoBehaviour
{
    [SerializeField] private Joystick joystick;
	[SerializeField] private Joystick joystickAiming;
    float x, z;
    bool jump;

    Vector3 currentMove;
    Vector3 lastMove;

	Vector3 currentAiming;

    // Update is called once per frame
    void Update()
    {
        x = joystick.Horizontal;
        z = joystick.Vertical;

        currentMove = new Vector3(x, 0, z);

		currentAiming = new Vector3(joystickAiming.Horizontal, 0, joystickAiming.Vertical);

        Move();
		Aim();
    }

    private void Move()
    {
        string move = "" + Mathf.Round(x*1000);
        move += "/" + Mathf.Round(z*1000);
        move += "|" + jump;

        Client.UseButton("Move", move);
    }

	public void Aim()
	{
		string aiming = "" + Mathf.Round(currentAiming.x * 1000);
		aiming += "/" + Mathf.Round(currentAiming.z * 1000);

		Client.UseButton("Aim", aiming);
        ////New shooting system
        //if(currentAiming.magnitude>0.9f)
        //{
        //    HoldBasic(true);
        //}
        //else
        //{
        //    HoldBasic(false);
        //}
	}

    public void Jump()
    {
        jump = true;
        Move();
        jump = false;
    }
    
    public void HoldBasic(bool holding)
    {
        Client.UseButton("Basic", "" + holding);
    }

    public void UseCC()
    {
        Client.UseButton("CC", "");
    }

    public void UseUltimate()
    {
        Client.UseButton("Ultimate", "");
    }


}
