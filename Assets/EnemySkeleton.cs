using UnityEngine;

public class EnemySkeleton : Entity
{
    private bool _isAttacking;

    [Header("Move info")]
    [SerializeField] private float moveSpeed;

    [Header("Player detection")]
    [SerializeField] private float playerCheckDistance;

    [SerializeField] private LayerMask whatIsPlayer;

    private RaycastHit2D _isPlayerDetected;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        Movement();

        if (_isPlayerDetected)
        {
            if (_isPlayerDetected.distance > 2)
            {
                Rb.linearVelocity = new Vector2(moveSpeed * 1.5f * FacingDirection, Rb.linearVelocity.y);

                _isAttacking = false;
            }
            else
            {
                _isAttacking = true;
            }
        }

        if (!IsGrounded || IsWallDetected)
        {
            Flip();
        }
    }

    private void Movement()
    {
        if (!_isAttacking)
            Rb.linearVelocity = new Vector2(moveSpeed * FacingDirection, Rb.linearVelocity.y);
    }

    protected override void CollisionChecks()
    {
        base.CollisionChecks();

        _isPlayerDetected =
            Physics2D.Raycast(transform.position, Vector2.right, playerCheckDistance * FacingDirection, whatIsPlayer);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + playerCheckDistance * FacingDirection, transform.position.y));
    }
}