using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Obsolete()]
public class NpcActor : MovableActor
{
    protected override void Update()
    {
        DoAIControl();
        base.Update();
    }

    public void DoAIControl()
    {

    }
}