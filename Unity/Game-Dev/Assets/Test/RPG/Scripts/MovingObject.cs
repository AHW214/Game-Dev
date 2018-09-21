using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class MovingObject : MonoBehaviour
{
    public float movementSpeed = 1.0F;

    private Rigidbody2D rb;
    protected Direction heading = new Direction(180);
  
	protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
	}

    protected virtual void Move()
    {
        Vector2 displacement = heading.v2 * movementSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + displacement);       
    }
}
