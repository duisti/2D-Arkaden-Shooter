using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float MaxSpeed = 5f;
    public float Acceleration = 5f; //for example 5 would mean that we accelerate to 5 in 1 second.
    //public float Decceleration = 7f; // commenting out to keep it simple. Accel will also be used for deccel.
    [SerializeField]
    Vector2 lastVelocity; // save previous velocity / direction

    [HideInInspector] //hides the values in inspector (public variables show up on inspector, reduce clutter)
    public float XInput;
    [HideInInspector]
    public float YInput; //we don't apply input directly to the values we want, instead we read them from these variables. Reason: Multiple control possibilities
                         //(virtual joystick, testing with keyboard etc..)

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void DEBUG_ReadInput()
    {
        XInput = Mathf.Clamp(Input.GetAxis("Horizontal"), -1f, 1f);
        YInput = Mathf.Clamp(Input.GetAxis("Vertical"), -1f, 1f); //clamp value between -1 and 1, because apparently for keyboard controls for example the axis goes up to 3
                                                                  //and i'm lazy and CBA changing it.
    }
    void Update()
    {
        //debug, editor mode only, read keyboard input
#if UNITY_EDITOR
        DEBUG_ReadInput();
#endif
        //When XInput and YInput has been set, apply values
        ApplyValues();
    }

    void ApplyValues()
    {
        Vector2 newVelocity = lastVelocity;
        float applyX;
        float applyY;
        //math from inputs and accel/decel
        //so when we multiply MaxSpeed with Input, not only this allows half values (for example joystick not completely tilted), it also allows negatives
        applyX = Mathf.MoveTowards(newVelocity.x, MaxSpeed * XInput, Acceleration * Time.deltaTime);
        applyY = Mathf.MoveTowards(newVelocity.y, MaxSpeed * YInput, Acceleration * Time.deltaTime);

        newVelocity = new Vector2(applyX, applyY);
        this.transform.localPosition += (Vector3)newVelocity * Time.deltaTime;
        lastVelocity = newVelocity;
    }
}
