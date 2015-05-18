using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBronzeAge.Data
{
    public class NPCActor : LiveActor
    {
        public string dialogText;
        public int staticLevel;
        public List<Quest> quests;

        public LiveActor enemyTarget;
        public Vector3 walkTarget;
        public NpcActorActionTypeAnimation specialAnimation;
        public float currentActionExecutionTime;

        // Add Navmesh agent info

        public Stack<NpcActorAction> queuedActions;
        public NpcActorAction currentAction;

        public override void Start()
        {
            base.Start();
        }
        public override void Update(float deltatime)
        {
            UpdateActions(deltatime);
            base.Update(deltatime);
        }

        public void QueueAction(NpcActorAction action)
        {
            queuedActions.Push(action);
        }
        public void SelectNextAction()
        {
            if (queuedActions.Count != 0)
                currentAction = queuedActions.Pop();
            else
                currentAction = null;
        }

        private void AttackLiveActor(LiveActor actor)
        {
            if (actor == null)
                return;
            if (!actor.isDead)
                QueueAction(new NpcActorAction(NpcActorActionType.AttackLiveActor, attacktargetactor: actor));
        }
        private void GotoPosition(Vector3 position)
        {
            QueueAction(new NpcActorAction(NpcActorActionType.GotoPosition, walkposition: position));
        }
        private void PerformAnimation(NpcActorActionTypeAnimation animation)
        {
            QueueAction(new NpcActorAction(NpcActorActionType.PerformAnimation, performedanimation: animation));
        }

        private void UpdateActions(float deltatime)
        {
            if (currentAction != null)
            {
                switch (currentAction.finish)
                {
                    case NpcActorActionFinish.OnEnemyKilled:
                        if (enemyTarget != null)
                        {
                            if (enemyTarget.isDead)
                                FinishCurrentAction();
                        }
                        break;
                    case NpcActorActionFinish.OnEnemyOutOfSight:
                        if (enemyTarget == null)
                            FinishCurrentAction();
                        break;
                    case NpcActorActionFinish.OnReachWalkTargetPosition:
                        if (position.Equals(currentAction.walkPosition)) // create = operator and Vector3.Distance-method
                            FinishCurrentAction();
                        break;
                    case NpcActorActionFinish.OnEndTimer:
                        if (currentActionExecutionTime >= currentAction.actionExecutionTime)
                            FinishCurrentAction();
                        else
                            currentActionExecutionTime += deltatime;
                        break;
                }

                switch (currentAction.actionType)
                {
                    case NpcActorActionType.AttackLiveActor:
                        AttackLiveActor(currentAction.attackTargetActor);
                        break;
                    case NpcActorActionType.GotoPosition:
                        GotoPosition(currentAction.walkPosition);
                        break;
                    case NpcActorActionType.PerformAnimation:
                        specialAnimation = currentAction.performedAnimation;
                        break;
                }
            }
        }
        private void FinishCurrentAction()
        {
            switch (currentAction.actionType)
            {
                case NpcActorActionType.AttackLiveActor:
                    enemyTarget = null;
                    break;
                case NpcActorActionType.GotoPosition:
                    // If walkTarget = new Vector3(0, 0, 0) the npc will run to this position
                    break;
                case NpcActorActionType.PerformAnimation:
                    specialAnimation = NpcActorActionTypeAnimation.Idle;
                    break;
            }

            currentActionExecutionTime = 0;
            if(!currentAction.isLoop)
                SelectNextAction();
        }
    }

    public class NpcActorAction
    {
        public NpcActorActionType actionType;
        public NpcActorActionFinish finish;
        public LiveActor attackTargetActor;
        public Vector3 walkPosition;
        public NpcActorActionTypeAnimation performedAnimation;
        public float actionExecutionTime;
        public bool isLoop;

        public NpcActorAction(NpcActorActionType actiontype, NpcActorActionFinish finish = NpcActorActionFinish.OnEndTimer, LiveActor attacktargetactor = null, Vector3 walkposition = default(Vector3), NpcActorActionTypeAnimation performedanimation = NpcActorActionTypeAnimation.Idle, float actionexecutiontime = 0, bool isloop = false)
        {
            actionType = actiontype;
            this.finish = finish;
            attackTargetActor = attacktargetactor;
            walkPosition = walkposition;
            performedAnimation = performedanimation;
            this.actionExecutionTime = actionexecutiontime;
            isLoop = isloop;
        }
    }
    public enum NpcActorActionType : byte
    {
        AttackLiveActor = 0,
        GotoPosition = 1,
        PerformAnimation = 2
    }
    public enum NpcActorActionTypeAnimation : byte
    {
        Idle = 0,
        CutWood = 1,
        Dance = 2,
        Talk = 3
    }
    public enum NpcActorActionFinish : byte
    {
        OnEnemyKilled = 0,
        OnEnemyOutOfSight = 1,

        OnReachWalkTargetPosition = 2,
        OnEndTimer = 3
    }
}
