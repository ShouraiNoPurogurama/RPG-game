using UnityEngine;

public class Entity : MonoBehaviour
{
    protected Animator Animator;
    protected Rigidbody2D Rb;

    protected int FacingDirection = 1;
    private bool _facingRight = true;

    [Header("Collision info")] 
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    /// <summary>
    /// Layer contains Colliders that need to be detected by Physics2D.Raycast/>.
    /// </summary>
    [SerializeField] protected LayerMask whatIsGround;
    protected bool IsGrounded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        Animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CollisionChecks();
    }

    /// <summary>
    /// Checks for ground collision beneath the player and updates the <see cref="IsGrounded"/> state.
    /// </summary>
    /// <remarks>
    /// This method uses a 2D raycast to determine if the player is in contact with the ground. 
    /// The raycast is projected downward from the player's position over a specified distance, 
    /// and it checks for collision with layers defined in <see cref="whatIsGround"/>.
    /// </remarks>
    /// <example>
    /// Example usage:
    /// <code>
    /// private void Update()
    /// {
    ///     CollisionChecks();
    ///     if (_isGrounded)
    ///     {
    ///         Debug.Log("Player is grounded.");
    ///     }
    /// }
    /// </code>
    /// </example>
    private void CollisionChecks()
    {
        IsGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    #region Flip character

    /// <summary>
    /// Flip character to the opposite direction based on current direction
    /// </summary>
    protected void FlipController()
    {
        if (Rb.linearVelocity.x > 0 && !_facingRight)
        {
            Flip();
        }
        else if (Rb.linearVelocity.x < 0 && _facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        FacingDirection = -FacingDirection;
        _facingRight = !_facingRight;
        transform.Rotate(0, 180, 0);
    }

    #endregion
    
    /// <summary>
    /// Draw a vector from Player position to the ground/>
    /// </summary>
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
    }
}