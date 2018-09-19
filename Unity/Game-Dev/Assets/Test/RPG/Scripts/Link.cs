using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
    private Coroutine inst = null;

    private Animator ar;
    private Rigidbody2D rb;

    private float magnitude = 0.0F;

    private Vector2 heading = Vector2.down;
    private Vector2 Heading
    {
        get { return heading; }
        set
        {
            if(value == Vector2.zero)
                return;

            heading = value;
        }
    }

    private void Start()
    {
        ar = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
        ar.Play("Idle");      
	}
	
	private void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        magnitude = input.magnitude;
        Heading = input.normalized;

        ar.SetFloat("FaceX", Heading.x);
        ar.SetFloat("FaceY", Heading.y);

        if(magnitude > 0.5F && inst == null)
            inst = StartCoroutine(WalkLoop());
            
	}

    private IEnumerator WalkLoop()
    {
        ar.Play("Walk");

        while(magnitude > 0.5F) {
            rb.velocity = Heading;
            yield return null;
        }

        rb.velocity = Vector2.zero;
        ar.Play("Idle");
        inst = null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }
}
