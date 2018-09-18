using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float magnitude = 1.0F;
    public float jumpForceMag = 5.0F;
    public float maxLinearSpeed = 5.0F;

    public bool IsGrounded
    {
        get
        {
            int num = Physics2D.OverlapCircleNonAlloc(transform.position, 1.2F * myRadius, overlapColliders, platforms);
            return num > 0;
        }
    }

    private float myRadius;
    private float maxAngularSpeed;

    private LayerMask platforms;
    private Collider2D[] overlapColliders = new Collider2D[10];

    private Collider2D coll;
    private Rigidbody2D rb;

    private void Start()
    {
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();

        myRadius = coll.bounds.extents.x;
        maxAngularSpeed = Mathf.Rad2Deg * (maxLinearSpeed / myRadius);

        platforms = 1 << LayerMask.NameToLayer("Platforms");
    }
	
	private void Update()
    {
	    if(Input.GetKey(KeyCode.D))
            rb.AddTorque(-magnitude);
        if(Input.GetKey(KeyCode.A))
            rb.AddTorque(magnitude);

        if(Input.GetKeyDown(KeyCode.Space) && IsGrounded)
            Jump();
    }

    private void FixedUpdate()
    {
        if(maxAngularSpeed > 0.0F && Mathf.Abs(rb.angularVelocity) > maxAngularSpeed)
            rb.angularVelocity = Mathf.Sign(rb.angularVelocity) * maxAngularSpeed;
    }

    private void Jump()
    {
        Vector2 pt = overlapColliders[0].Distance(coll).pointA;
        Vector2 dir = ((Vector2)transform.position - pt).normalized;
        Vector2 force = jumpForceMag * dir;

        rb.AddForce(force, ForceMode2D.Impulse);
    }
}
