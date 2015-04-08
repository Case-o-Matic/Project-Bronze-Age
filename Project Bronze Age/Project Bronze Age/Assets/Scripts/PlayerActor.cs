using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PlayerActor : MovableActor
{
    protected override void Update()
    {
        DoMove();
        DoControl();

        base.Update();
    }

    private void DoMove()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
    }

    private void DoControl()
    {

    }
}