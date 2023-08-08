using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCarController : MonoBehaviour
{
    private Rigidbody2D m_RigidBody;

    [Header("Car Settings")]
    public float m_Speed = 30.0f;
    public float m_Rotation_Speed = 5.5f;
    public float m_Max_Speed = 20;

    public float driftFactor = 0.95f;
    float accelerationInput = 0;
    float steeringInput = 0;
    float velocityVsUp = 0;

    float rotationAngle = 0;


    void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ApplyEngineForce();
        KillOrthogonalVelocity();
        ApplySteering();
    }

    private void ApplySteering()
    {
        // Limit the cars ability to turn when moving slowly
        float minSpeedBeforeAllowTurningFactor = m_RigidBody.velocity.magnitude / 8;

        // Update the rotation based on input

        rotationAngle -= steeringInput * m_Rotation_Speed * minSpeedBeforeAllowTurningFactor;
        m_RigidBody.MoveRotation(rotationAngle);
    }

    private void ApplyEngineForce()
    {
        velocityVsUp = Vector2.Dot(m_RigidBody.velocity, transform.right);

        if (velocityVsUp > m_Max_Speed && accelerationInput > 0)
            return;

        if (velocityVsUp > -m_Max_Speed * 0.5f && accelerationInput < 0)
            return;

        if(m_RigidBody.velocity.sqrMagnitude > m_Max_Speed * m_Max_Speed && accelerationInput > 0)
            return;

        if (accelerationInput == 0)
        {
            m_RigidBody.drag = Mathf.Lerp(m_RigidBody.drag, 3.0f, Time.fixedDeltaTime * 3);
        }
        else m_RigidBody.drag = 0;

        Vector2 engineForward = accelerationInput * m_Speed * transform.right;
        m_RigidBody.AddForce(engineForward, ForceMode2D.Force);
    }

    void KillOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.right * Vector2.Dot(m_RigidBody.velocity, transform.right);
        Vector2 upVelocity = transform.up * Vector2.Dot(m_RigidBody.velocity, transform.up);

        m_RigidBody.velocity = forwardVelocity + upVelocity * driftFactor;
    }

    public void SetInputVector(Vector2 inputVector)
    {
        steeringInput = inputVector.x;
        accelerationInput = inputVector.y;
    }
}
