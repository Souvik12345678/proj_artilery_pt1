using UnityEngine;

[RequireComponent(typeof(HealthScript))]
public class NewTankScript : MonoBehaviour
{
    public float driveSpeed;
    public float rotateSpeed;
    public float muzzRotateSpeed;
    public bool isDestroyed;

    public Transform tankBodyTransform;
    public Transform muzzleTransform;

    HealthScript healthScript;

    private void OnEnable()
    {
        healthScript.OnHealthDepleted += OnTankDestroyed;
    }

    private void OnDisable()
    {
        healthScript.OnHealthDepleted -= OnTankDestroyed;
    }

    private void Awake()
    {
        tankBodyTransform = transform.GetChild(0);
        muzzleTransform = transform.Find("tank_body/muzzle");
    }

    /// <summary>
    /// forward 1 -> move forward, forward -1 -> movebackward,turn-> Rotate clockwise if -1 and counterclockwise if 1
    /// </summary>
    public void Move(int forward, int turn)
    {
        if (!isDestroyed)
        {
            transform.Translate(forward * driveSpeed * Time.deltaTime * transform.up, Space.World);
            transform.Rotate(0, 0, turn * Time.deltaTime * rotateSpeed);
        }
    }

    /// <summary>
    /// turn == 1 clockwise, turn == -1 anticlockwise
    /// </summary>
    public void MuzzleRotate(int turn)
    {
        if (!isDestroyed)
        {
            //float angle = Vector2.SignedAngle(tankBodyTransform.up, muzzleTransform.up);
            //float newAngle = angle + (-turn * Time.deltaTime * muzzRotateSpeed);
            //muzzleTransform.Rotate(0, 0, -turn * Time.deltaTime * muzzRotateSpeed, Space.Self);
            //Debug.Log(angle);

            muzzleTransform.Rotate(0, 0, -turn * Time.deltaTime * muzzRotateSpeed, Space.Self);
            float newAngle = Vector2.SignedAngle(tankBodyTransform.up, muzzleTransform.up);
            if (newAngle > 90)
            {
                muzzleTransform.localRotation = Quaternion.Euler(0, 0, 90);
            }
            if (newAngle < -90)
            {
                muzzleTransform.localRotation = Quaternion.Euler(0, 0, -90);
            }
        }
    }

    public HealthScript GetHealthScript()
    { return healthScript; }

    void OnTankDestroyed()
    {
        Debug.Log("Tank Destroyed");   
    }

}
