/// <summary>
/// Simulates a state machine, To use the state machine user needs to call its Start, Update, Exit functions
/// </summary>
public class StateMachine
{
    State[] currentStates = new State[3];//Can hold 3 states concurrently
    int stackPtr = 0;

    public int stackTop
    {
        get { return stackPtr; }
    }

    public void Initialize(State startingState)
    {
        currentStates[0] = startingState;
        stackPtr = 0;
        startingState.OnEnter();
    }

    public void Start()
    {

    }

    public void Update()
    {
        //Update states
        for (int i = 0; i < 3; i++)
        {
            if (currentStates[i] != null)
            {
                currentStates[i].OnUpdate();
            }
        }
    }

    public void Exit()
    {
       // Update states
        for (int i = 0; i < 3; i++)
        {
            if (currentStates[i] != null)
            {
                currentStates[i].OnExit();
            }
        }
    }

    public void AddState(State state)
    {
        stackPtr = (stackPtr + 1) % 3;//loop values within 0-2
        currentStates[stackPtr] = state;
        state.OnEnter();
    }

    public void AddState(State state, int index)//Add State to the state stack,Index should be within 2
    {
        currentStates[index] = state;
        state.OnEnter();
    }

    public void ChangeState(State newState)//
    {
        //Cleanup old states
        for (int i = 0; i < 3; i++)
        {
            if (currentStates[i] != null)
            {
                currentStates[i].OnExit();
                currentStates[i] = null;
            }
        }

        currentStates[0] = newState;
        newState.OnEnter();
    }


}
