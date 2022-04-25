using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{

    public Transform target;

    public float smoothing;

    Vector2 targetDirection;//It is a unit vector

    List<int> list;


    TankAIStateMachine stateMachine;
    StateMachine stateMachine1;

    // Start is called before the first frame update
    void Start()
    {
        list = new List<int>();
        list.Add(10);
        list.Add(11);
        list.Add(102);

        stateMachine = new TankAIStateMachine(new TankAIScript_pt1());
        stateMachine1 = new StateMachine();

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dirTotarget = (target.transform.position - transform.position).normalized;
        targetDirection = dirTotarget;
        RotateLogic();
    }

    private void OnDestroy()
    {
        Debug.Log("On Destroy");
    }

    void RotateLogic()
    {
        // The step size is equal to speed times frame time.
        // float singleStep = 1.0f * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        // Vector3 newDirection = Vector3.RotateTowards(transform.up, targetDirection, singleStep, 0.1f);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        // transform.rotation = Quaternion.LookRotation(newDirection);

        Debug.DrawRay(transform.position, targetDirection, Color.green);

        float angle = Vector2.Angle(transform.up, targetDirection);
        Quaternion intRotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, targetDirection), Time.deltaTime * smoothing);
        transform.rotation = intRotation;

    }

}
