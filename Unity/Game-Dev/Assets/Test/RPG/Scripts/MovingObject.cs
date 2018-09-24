using UnityEngine;
using Game;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class MovingObject : MonoBehaviour
{
    public int pixelLengthsPerFrame = 10;

    private Rigidbody2D rb;
    protected Direction heading = new Direction(180);
  
	protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SnapToPixelGrid();     
	}

    protected virtual void Move()
    {
        Vector2 displacement = CalculateDisplacement();
        rb.MovePosition(rb.position + displacement);       
    }

    protected virtual void Move(float speed) // redundancy
    {
        //Vector2 displacement = heading.v2 * speed * Time.deltaTime;
        //rb.MovePosition(rb.position + displacement);
    }

    private void SnapToPixelGrid()
    {
        Vector2 pixelPosition = GetPixelPosition(rb.position);
        rb.MovePosition(pixelPosition);
    }

    private Vector2 GetPixelPosition(Vector2 position)
    {
        return new Vector2(PixelCoordinate(position.x), PixelCoordinate(position.y));
    }

    private float PixelCoordinate(float coordinate)
    {
        return Mathf.Round(coordinate / Dimensions.UPP) * Dimensions.UPP;
    }

    private Vector2 CalculateDisplacement()
    {
        float scalar = Mathf.Round(pixelLengthsPerFrame / heading.pixelDirection.magnitude);
        Debug.Log("number of pixels: " + scalar + "\ndistance moved: " + scalar * heading.pixelDirection.magnitude);

        return scalar * heading.pixelDirection * Dimensions.UPP;
    }
}
