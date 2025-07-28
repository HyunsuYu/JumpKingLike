using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D m_rigidBody;
    private SpriteRenderer m_spriteRenderer;
    private Animator m_animator;

    [SerializeField] private float m_horizontalSpeed = 5f;
    [SerializeField] private float m_jumpForce = 10.0f;

    [SerializeField] private float m_JumpChargeMaxTime = 2.0f;
    [SerializeField] private RectTransform m_ChargingState;

    private bool m_bisJumpCharging = false;
    private bool m_bisJumping = false;

    private bool m_bisLoogLeft = false;

    private float m_jumpChargeTime = 0.0f;


    public void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();
    }
    public void Update()
    {
        if (!m_bisJumpCharging && !m_bisJumping)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            Vector2 movement = new Vector2(horizontalInput * m_horizontalSpeed, m_rigidBody.velocity.y);
            m_rigidBody.velocity = movement;

            if (horizontalInput != 0)
            {
                m_spriteRenderer.flipX = horizontalInput < 0;
                m_bisLoogLeft = horizontalInput < 0;

                m_animator.SetBool("BIsWalk", true);
            }
            else
            {
                m_animator.SetBool("BIsWalk", false);
            }
        }
        m_animator.SetBool("BIsJump", m_bisJumping);

        if (Input.GetKeyDown(KeyCode.Space) && !m_bisJumping)
        {
            m_bisJumpCharging = true;
            m_jumpChargeTime = 0.0f;

            m_rigidBody.velocity = new Vector2(0, m_rigidBody.velocity.y);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            if (m_bisJumpCharging)
            {
                float jumpForce = Mathf.Clamp(Mathf.Log10(10.0f * (m_jumpChargeTime / m_JumpChargeMaxTime) + 1.0f) / 1.32f, 0.0f, 1.0f) * m_jumpForce;
                m_rigidBody.AddForce(new Vector2(m_bisLoogLeft ? -m_horizontalSpeed : m_horizontalSpeed, jumpForce), ForceMode2D.Impulse);
                m_bisJumping = true;
                m_bisJumpCharging = false;
            }
        }

        if (m_bisJumpCharging)
        {
            m_jumpChargeTime += Time.deltaTime;

            if (m_jumpChargeTime > m_JumpChargeMaxTime)
            {
                m_jumpChargeTime = m_JumpChargeMaxTime;
            }

            m_ChargingState.localScale = new Vector3()
            {
                x = m_jumpChargeTime / m_JumpChargeMaxTime,
                y = 1.0f,
                z = 1.0f
            };
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            m_bisJumping = false;
            m_bisJumpCharging = false;
            m_jumpChargeTime = 0.0f;
            m_ChargingState.localScale = new Vector3(0, 1.0f, 1.0f);
        }
    }
}