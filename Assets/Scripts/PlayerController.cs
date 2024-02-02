using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damagable))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float airWalkSpeed = 3f;
    public float jumpImpulse = 10f;
    public float gravityScaleOnPlatform = 50f;

    public float currentMoveSpeed { get
    {
        if (CanMove)
            if (IsMoving && !touchingDirections.IsOnWall) {
                    if (touchingDirections.IsGrounded) {
                        return IsRunning ? runSpeed : walkSpeed;
                    }
                    else {
                        return airWalkSpeed;
                    }
            } else {
                    return 0f;
            }
        else
            // movement is disabled
            return 0f;
    }}

    public bool IsMoving { get
        {
            return _isMoving;
        } private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }
    public bool IsRunning { get
        {
            return _isRunning;
        } private set
        {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }

    public bool _isFacingRight = true;
    public bool isFacingRight { get
        {
            return _isFacingRight;
        } private set {
            if (value != _isFacingRight) {
                _isFacingRight = value;
                transform.localScale *= new Vector2(-1, 1);
            }
        }
    }

    public bool CanMove {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public bool IsAlive {
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }

    private Rigidbody2D _platformRb;
    public Rigidbody2D PlatformRb {
        get
        {
            return _platformRb;
        }
        set
        {
            _platformRb = value;
            if (value)
            {
                rb.velocity = value.velocity + rb.velocity;
            }
        }
    }
    [SerializeField]
    private bool _isOnPlatform = false;
    public bool IsOnPlatform {
        get
        {
            return _isOnPlatform;
        }
        set
        {
            _isOnPlatform = value;
        }
    }

    [SerializeField]
    private bool _isMoving = false;
    [SerializeField]
    private bool _isRunning = false;
    Vector2 moveInput;
    Rigidbody2D rb;
    Animator animator;
    TouchingDirections touchingDirections;

    Damagable damageable;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damagable>();
    }

    void FixedUpdate()
    {
        if (!damageable.IsHit) {
            float targetVelocityX = moveInput.x * currentMoveSpeed;
            float targetVelocityY = rb.velocity.y;
            if (IsOnPlatform)
            {
                // se o jogador estiver indo na mesma direção que a plataforma, a velocidade da plataforma é somada à velocidade do jogador
                if (targetVelocityX > 0 && PlatformRb.velocity.x > 0 || targetVelocityX < 0 && PlatformRb.velocity.x < 0)
                    targetVelocityX += PlatformRb.velocity.x / 2.0f;
                // se o jogador estiver indo na direção oposta à da plataforma, a velocidade da plataforma é subtraída da velocidade do jogador
                else if (targetVelocityX > 0 && PlatformRb.velocity.x < 0 || targetVelocityX < 0 && PlatformRb.velocity.x > 0)
                    targetVelocityX += PlatformRb.velocity.x;
                // se o jogador estiver parado, a velocidade da plataforma é mantida
                else if (targetVelocityX == 0)
                    targetVelocityX += PlatformRb.velocity.x;
                // a velocidade vertical e substituída pela velocidade da plataforma
                targetVelocityY = PlatformRb.velocity.y;
            }
            rb.velocity = new Vector2(targetVelocityX, targetVelocityY);
            animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);

        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        // Verifica se mais de uma tecla está sendo pressionada
        if (Mathf.Abs(input.x) > 0 && Mathf.Abs(input.y) > 0)
        {
            // Se mais de uma tecla está sendo pressionada, ajusta o vetor de entrada
            input = new Vector2(Mathf.Sign(input.x), 0);
        }
        if (IsAlive)
        {
            moveInput = input;
            IsMoving = moveInput != Vector2.zero;

            SetFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }

    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !isFacingRight)
        {
                isFacingRight = true;
        }
        else if (moveInput.x < 0 && isFacingRight)
        {
                isFacingRight = false;
        }
    }

	public void OnRun(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			IsRunning = true;
		}
		else if (context.canceled)
		{
			IsRunning = false;
		}
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		// TODO check if is alive
		if (context.started && touchingDirections.IsGrounded && CanMove)
		{
			animator.SetTrigger(AnimationStrings.jumpTrigger);

            float impulse = jumpImpulse;

            if (IsOnPlatform)
            {
                impulse += PlatformRb.velocity.y / 2;
                IsOnPlatform = false;
                PlatformRb = null;
            }

            rb.velocity = new Vector2(rb.velocity.x, impulse);

		}
	}

	public void OnAttack(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			animator.SetTrigger(AnimationStrings.attackTrigger);
		}
	}

	public void OnRangedAttack(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			animator.SetTrigger(AnimationStrings.rangedAttackTrigger);
		}
	}

	public void OnHit(int damage, Vector2 knockBack)
	{
		rb.velocity = new Vector2(knockBack.x, rb.velocity.y + knockBack.y);
	}
}
