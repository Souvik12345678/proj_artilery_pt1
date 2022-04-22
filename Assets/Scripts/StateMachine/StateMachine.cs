/// <summary>
/// Simulates a state machine, To use the state machine user needs to call its Start, Update, Exit functions
/// </summary>
public class StateMachine
{
    State currentState;

    public void Initialize(State startingState)
    {
        currentState = startingState;
        startingState.OnEnter();
    }

    public void Start()
    {

    }

    public void Update()
    {
        currentState.OnUpdate();
    }

    public void Exit()
    {
        currentState.OnExit();
    }

    /*
    public void AddState(State state)
    {
        for (int i = 0; i < 3; i++)
        {
            if (currentStates[i] != null)
            {
                if (currentStates[i].GetType() == state.GetType())
                    return;//State type already exists return
            }
        }
        stackPtr = (stackPtr + 1) % 3;//loop values within 0-2
        currentStates[stackPtr] = state;
        state.OnEnter();
    }

    public void AddState(State state, int index)//Add State to the state stack,Index should be within 2
    {
        currentStates[index] = state;
        state.OnEnter();
    }

    */

    public void ChangeState(State newState)//Change state
    {
        if (currentState.GetType() != newState.GetType())
        {
            currentState.OnExit();
            currentState = newState;
            newState.OnEnter();
        }
    }


}
