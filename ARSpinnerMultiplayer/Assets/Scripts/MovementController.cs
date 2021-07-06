using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Header("Joystick Properties")]
    public Joystick joystick;
    public float Speed = 4;
    Vector3 velocityVector = Vector3.zero;
    Rigidbody rb;
    public float maxVelocityChange = 4.0f;
    public float TiltAmount = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //taking the joystick inputs
        float xMovementInput = joystick.Horizontal;
        float zMovementInput = joystick.Vertical;

        //calculing the velocity of vectors
        Vector3 movementHorizontal = transform.right * xMovementInput;
        Vector3 movementVertical = transform.forward * zMovementInput;

        //Calculate the final movement velocity vector
        Vector3 movementVelocityVector = (movementHorizontal + movementVertical).normalized * Speed;

        //Apply Movement
        Move(movementVelocityVector);

        transform.rotation = Quaternion.Euler(joystick.Vertical * Speed * TiltAmount, 0, -joystick.Horizontal * Speed * TiltAmount);

    }

    void Move(Vector3 movementVelocityVector)
    {
        velocityVector = movementVelocityVector;
    }

    private void FixedUpdate()
    {
        if(velocityVector != Vector3.zero)
        {
            //GET current rigidbodys velocity
            Vector3 velocity = rb.velocity;
            Vector3 velocityChange = (velocityVector - velocity);

            //Apply Force by the amount of velocity change toreach the target velocity

            velocityChange.x = Mathf.Clamp(velocityChange.x,-maxVelocityChange,maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0.0f;

            rb.AddForce(velocityChange,ForceMode.Acceleration);
        }

        

    }

}
