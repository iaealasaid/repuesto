using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;

    [Header("Movement")]
    public float speed = 6f;
    public float direction;
    public float acceleration = 20f;
    public float deceleration = 25f;

    [Header("Jump")]
    public float jumpPower = 10f;
    public float gravityMultiplier = 2.5f;
    public float fallMultiplier = 3.5f;
    public float jumpCutMultiplier = 2f;

    public float jumpBufferTime = 0.15f;
    private float jumpBufferCounter;

    [Header("Fast Fall")]
    public float fastFallMultiplier = 2f;
    private float verticalInput;

    [Header("Air Control")]
    public float airAccelerationMultiplier = 0.6f;
    public float airMaxSpeedMultiplier = 1f;

    [Header("GroundCheck")]
    public Transform groundCheck;
    public float groundRadius = 0.3f;
    public LayerMask groundLayer;

    [Header("Wall")]
    public Transform wallCheck;
    public float wallDistance = 0.5f;
    public LayerMask wallLayer;
    public float wallSlidingSpeed = 1.5f;

    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector3 wallJumpingPower = new Vector3(5f, 14f);

    private float wallJumpLockTime = 0.2f;
    private float wallJumpLockCounter;

    private bool isWallSliding;
    private bool isWallJumping;

    [Header("CoyoteTime")]
    public float coyoteTime = 0.15f;
    private float coyoteCounter;

    //public Animator playerAnimator;

    private bool isFacingRight = true;

    private bool jumpPressed;
    private bubble bubuja;
    private Vector3 spawnPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        bubuja = GetComponent<bubble>();
        //playerAnimator = GetComponent<Animator>();
        spawnPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //playerAnimator.SetFloat("MoveAnim", Mathf.Abs(direction));

        //Movement
        float targetSpeed = direction * speed;

        if (!IsGrounded())
        {
            targetSpeed *= airMaxSpeedMultiplier;
        }

        float speedDif = targetSpeed - rb.linearVelocity.x;
        float accelRate;

        if (IsGrounded())
        {
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        }
        else
        {
            float apexBonus = Mathf.InverseLerp(0, 5f, Mathf.Abs(rb.linearVelocity.y));

            if (Mathf.Abs(targetSpeed) > 0.01f)
            {
                accelRate = acceleration * airAccelerationMultiplier * (1f + (1f - apexBonus));
            }
            else
            {
                accelRate = deceleration * 0.5f;
            }
        }

        float movement = speedDif * accelRate;
        rb.AddForce(new Vector3(movement, 0, 0));

        //Coyote
        if (IsGrounded())
        {
            coyoteCounter = coyoteTime;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        //Jump
        if (jumpBufferCounter > 0f && coyoteCounter > 0f)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, 0);
            rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);

            jumpBufferCounter = 0f;
            jumpPressed = false;
        }

        if (jumpPressed)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        wallJumpLockCounter -= Time.deltaTime;

        WallSlide();
        WallJump();

        //Gravity
        float gravityMult = bubuja.bubbleActive ? 0.3f : 1f;

        bool isFastFalling = verticalInput < -0.5f;

        if (rb.linearVelocity.y < 0)
        {
            float currentFallMultiplier = fallMultiplier;

            if (isFastFalling)
            {
                currentFallMultiplier *= fastFallMultiplier;
            }

            rb.linearVelocity += Vector3.up * Physics.gravity.y * (currentFallMultiplier * gravityMult - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (gravityMultiplier * gravityMult - 1) * Time.deltaTime;
        }

        //Flip
        if (!isWallJumping)
        {
            if (direction > 0 && !isFacingRight)
            {
                Flip();
            }
            else if (direction < 0 && isFacingRight)
            {
                Flip();
            }
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        direction = input.x;
        verticalInput = input.y;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpPressed = true;
        }

        if (context.canceled)
        {
            if (rb.linearVelocity.y > 0)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f, 0);
            }
        }
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundRadius, groundLayer);
    }

    private bool IsWalled()
    {
        if (wallJumpLockCounter > 0f) return false;
        return Physics.CheckSphere(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && direction != 0f && !isWallJumping)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, -wallSlidingSpeed, 0);
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (jumpPressed && wallJumpingCounter > 0f)
        {
            isWallJumping = true;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }

            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            rb.AddForce(new Vector3(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y, 0), ForceMode.VelocityChange);

            wallJumpingCounter = 0f;
            jumpPressed = false;

            wallJumpLockCounter = wallJumpLockTime;

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    public void ActivateBubble(InputAction.CallbackContext context)
    {
        if (context.performed)
            bubuja.TryActivate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Void"))
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.position = spawnPoint;
    }
}
