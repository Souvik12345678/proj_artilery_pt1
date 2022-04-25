
public class ArtileryStateMachine : StateMachine
{
    public ArtileryStateMachine(ArtileryScript artScript)
    {
        stateDict.Add("IDLE", new ArtileryIdleState(this, artScript));
        stateDict.Add("ATTK", new ArtileryAttackingState(this, artScript));
    }

}

public class ArtileryIdleState : State
{
    ArtileryScript artScript;
    StateMachine stateMachine;
    public ArtileryIdleState(StateMachine artStateMachine, ArtileryScript artileryScript)
    { artScript = artileryScript; stateMachine = artStateMachine; }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (artScript.enemiesInSight.Count > 0)
        {
            stateMachine.ChangeState("ATTK");
        }

    }


}

public class ArtileryAttackingState : State
{
    ArtileryScript artScript;
    StateMachine stateMachine;
    public ArtileryAttackingState(StateMachine artStateMachine, ArtileryScript artileryScript)
    { artScript = artileryScript; stateMachine = artStateMachine; }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (artScript.enemiesInSight.Count > 0)
        {
            artScript.Shoot();
        }
        else
        {
            stateMachine.ChangeState("IDLE");
        }

    }

}