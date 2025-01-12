using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float _xInput;
    [SerializeField] private float jumpForce = 5;
    [SerializeField] private float moveSpeed = 5;

    [Header("Dash info")] 
    [SerializeField] private float dashSpeed = 50;
    [SerializeField] private float dashDuration = 0.05f;
    [SerializeField] private float dashTime = 1;

    private int _facingDirection = 1;
    private bool _facingRight = true;

    private Rigidbody2D _rb;
    private Animator _animator;

    [Header("Collision info")] 
    [SerializeField] private float groundCheckDistance;

    /// <summary>
    /// Layer contains Colliders that need to be detected by Physics2D.Raycast/>.
    /// </summary>
    [SerializeField] private LayerMask whatIsGround;

    private bool _isGrounded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        CheckInput();
        CollisionChecks();

        dashTime -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            dashTime = dashDuration;
        }

        if (dashTime > 0)
        {
        }

        FlipController();
        AnimatorControllers();
    }

    /// <summary>
    /// Checks for ground collision beneath the player and updates the <see cref="_isGrounded"/> state.
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
        _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }


    private void CheckInput()
    {
        //Get horizontal vector value based on keyboard key down
        _xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    /// <summary>
    /// Control Player horizontal movement by assigning new 2d vector based on <see cref="_xInput"/>
    /// </summary>
    private void Movement()
    {
        // If dash is in use
        if (dashTime > 0)
        {
            _rb.linearVelocity = new Vector2(_xInput * dashSpeed, 0);
        }
        else
        {
            _rb.linearVelocity = new Vector2(_xInput * moveSpeed, _rb.linearVelocity.y);
        }
    }

    /// <summary>
    /// Control Player jump behaviour by assigning new 2d vector based on <see cref="jumpForce"/>
    /// </summary>
    private void Jump()
    {
        if (_isGrounded)
            _rb.linearVelocity = new Vector2(_rb.linearVelocityX, jumpForce);
    }

    /// <summary>
    /// Assign a bool value to "isMoving" animator parameter value based on the current horizontal velocity
    /// </summary>
    private void AnimatorControllers()
    {
        bool isMoving = _rb.linearVelocity.x != 0;

        _animator.SetBool("isMoving", isMoving);
        _animator.SetBool("isGrounded", _isGrounded);
        _animator.SetFloat("yVelocity", _rb.linearVelocity.y);
        _animator.SetBool("isDashing", dashTime > 0);
    }

    #region Flip character

    /// <summary>
    /// Flip character to the opposite direction based on current direction
    /// </summary>
    private void Flip()
    {
        _facingDirection = -_facingDirection;
        _facingRight = !_facingRight;
        transform.Rotate(0, 180, 0);
    }

    private void FlipController()
    {
        if (_rb.linearVelocity.x > 0 && !_facingRight)
        {
            Flip();
        }
        else if (_rb.linearVelocity.x < 0 && _facingRight)
        {
            Flip();
        }
    }

    #endregion

    /// <summary>
    /// Draw a vector from Player position to the ground/>
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position,
            new Vector3(transform.position.x, transform.position.y - groundCheckDistance, transform.position.z));
    }
}