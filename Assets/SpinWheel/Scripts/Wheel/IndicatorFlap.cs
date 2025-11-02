using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorFlap : MonoBehaviour
{
    public float restAngle = 0f;     // The default upright angle in local rotation
    public float stiffness = 50f;    // Higher = stronger return force
    public float damping = 5f;       // Prevents overshooting or oscillation

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float delta = Mathf.DeltaAngle(rb.rotation, restAngle);
        float torque = -delta * stiffness - rb.angularVelocity * damping;
        rb.AddTorque(torque);
    }
}
