using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kirby : MonoBehaviour
{
    public float walkMag = 10.0F;
    public float airMag = 5.0F;
    public float jumpMag = 5.0F;

    private bool prevGrounded = true;
    private bool grounded = true;

    private Collider2D[] overlapColliders = new Collider2D[10];
    private LayerMask platforms;

    private SpriteRenderer sr;
    private Animator ar;

    private Rigidbody2D rb;
    private Collider2D coll;
    
    private void Start()
    {
        platforms = 1 << LayerMask.NameToLayer("Platforms");

        sr = GetComponent<SpriteRenderer>();
        ar = GetComponent<Animator>();

        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
	}

	private void Update()
    {
        FaceForward();

        if(Input.GetKey(KeyCode.A))
            Move(Vector2.left);
        else if(Input.GetKey(KeyCode.D))
            Move(Vector2.right);

        if(Input.GetKeyDown(KeyCode.Space) && grounded)
            StartCoroutine(Jump());

	}

    private void FixedUpdate()
    {
        prevGrounded = grounded;
        grounded = IsGrounded();

        SetAnimState();
        ar.SetFloat("Speed", rb.velocity.magnitude);
    }

    private void FaceForward()
    {
        float x = rb.velocity.x;

        if(x > 0.0F && sr.flipX)
            sr.flipX = false;
        else if(x < 0.0F && !sr.flipX)
            sr.flipX = true;
    }

    private void SetAnimState()
    {
        int state;

        if(!grounded) {
            if(rb.velocity.y > 0.0F)
                state = 3;
            else if(rb.velocity.y > -3.0F)
                state = 4;
            else 
                state = 5;
        }
        else if(!prevGrounded)
            state = 6;
        else
            state = rb.velocity == Vector2.zero ? 0 : 1;

        ar.SetInteger("State", state);
    }

    private void Move(Vector2 dir)
    {
        rb.AddForce((grounded ? walkMag : airMag) * dir);
    }

    private IEnumerator Jump()
    {
        Vector2 dir = overlapColliders[0].transform.up;

        rb.AddForce(jumpMag * dir, ForceMode2D.Impulse);

        float t = 0.0F;
        while(!Input.GetKeyUp(KeyCode.Space) && t < 1.0F) {
            rb.AddForce(Mathf.Lerp(3.6F * jumpMag, 0.0F, t) * dir, ForceMode2D.Force);
            t += Time.deltaTime;

            yield return null;
        }
    }

    private bool IsGrounded()
    {
        int num = Physics2D.OverlapCircleNonAlloc(transform.position, 0.01F, overlapColliders, platforms);
        return num > 0;
    }
}
