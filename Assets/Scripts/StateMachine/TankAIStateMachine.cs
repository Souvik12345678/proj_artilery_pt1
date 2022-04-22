using UnityEngine;

public class TankAIStateMachine : StateMachine
{
    public TankAIStateMachine(TankAIScript_pt1 tankAIScript)
    {
        stateDict.Add("APPR_BASE", new ApproachingBaseAndMuzzleAimState(this, tankAIScript));
        stateDict.Add("ATTK_ENEM", new AttackingTroopsState(this, tankAIScript));
    }

}
