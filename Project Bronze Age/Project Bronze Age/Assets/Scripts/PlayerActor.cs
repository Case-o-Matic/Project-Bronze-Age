using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlayerActor : LiveActor
{
    public float horizontalMove, verticalMove;

    protected override void Update()
    {
        DoMove();
        DoControl();

        base.Update();
    }

    private void DoMove()
    {
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");
    }

    private void DoControl()
    {

    }
}