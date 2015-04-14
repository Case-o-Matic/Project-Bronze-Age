using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AIAction
{
    public AIActionType type;

    public LiveActor attackEnemyTarget;
    public int playAnimationIndex;

    public AIAction(AIActionType type)
    {
        this.type = type;
    }
}

[Serializable]
public enum AIActionType
{
    AttackEnemy,
    PlayAnimation
}