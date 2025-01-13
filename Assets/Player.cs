using UnityEngine;
using UnityEngine.Serialization;

public class Player : Entity
{
    private float _xInput;
    [Header("Move info")]
    [SerializeField] private float jumpForce = 5;
    [SerializeField] private float moveSpeed = 5;

    [Header("Dash info")] [SerializeField] public float dashSpeed = 25;
    [SerializeField] public float dashDuration = 0.05f;
    [SerializeField] public float dashTime = 1;

    [SerializeField] public float dashCooldown = 2;
    [SerializeField] public float dashCooldownTimer;
    

    [FormerlySerializedAs("comboTimeCounter")]
    [Header("Attack info")] 
    [SerializeField] private float comboTimeWindow;
    [SerializeField] private float comboTime = 1f;
    private bool isAttacking;
    private int comboCounter;
    
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Movement();
        CheckInput();
        
        dashTime -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;
        comboTimeWindow -= Time.deltaTime;
        
        FlipController();
        AnimatorControllers();
    }

    /// <summary>
    /// This method will be called by the AnimationTrigger at the ending frame of attack animations
    /// </summary>
    public void AttackOver()
    {
        isAttacking = false;

        comboCounter++;
        if (comboCounter > 2)
        {
            comboCounter = 0;
        }
    }


    private void CheckInput()
    {
        //Get horizontal vector value based on keyboard key down
        _xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartAttackEvent();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            DashAbility();
        }
    }

    private void StartAttackEvent()
    {
        if (!IsGrounded)
        {
            return;
        }
        
        //Reset comboCounter if Player exceeds the time of comboTimeWindow
        if (comboTimeWindow <= 0)
        {
            comboCounter = 0;
        }
            
        isAttacking = true;
        comboTimeWindow = comboTime;
    }

    private void DashAbility()
    {
        if (dashCooldownTimer <= 0 && !isAttacking)
        {
            dashCooldownTimer = dashCooldown;
            dashTime = dashDuration;
        }
    }

    /// <summary>
    /// Control Player horizontal movement by assigning new 2d vector based on <see cref="_xInput"/>
    /// </summary>
    private void Movement()
    {
        if (isAttacking)
        {
            Rb.linearVelocity = new Vector2(0, 0);
        }
        // If dash is in use
        else if (dashTime > 0) 
        {
            Rb.linearVelocity = new Vector2(FacingDirection * dashSpeed, 0);
        }
        else
        {
            Rb.linearVelocity = new Vector2(_xInput * moveSpeed, Rb.linearVelocity.y);
        }
    }

    /// <summary>
    /// Control Player jump behaviour by assigning new 2d vector based on <see cref="jumpForce"/>
    /// </summary>
    private void Jump()
    {
        if (IsGrounded)
            Rb.linearVelocity = new Vector2(Rb.linearVelocityX, jumpForce);
    }

    /// <summary>
    /// Assign a bool value to "isMoving" animator parameter value based on the current horizontal velocity
    /// </summary>
    private void AnimatorControllers()
    {
        bool isMoving = Rb.linearVelocity.x != 0;

        Animator.SetFloat("yVelocity", Rb.linearVelocity.y);
        Animator.SetBool("isMoving", isMoving);
        Animator.SetBool("isGrounded", IsGrounded);
        Animator.SetBool("isDashing", dashTime > 0);
        
        Animator.SetBool("isAttacking", isAttacking);
        Animator.SetInteger("comboCounter", comboCounter);
    }

    protected override void CollisionChecks()
    {
        base.CollisionChecks();
    }
}