using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Propriedades

    public float speed = 10f;
    public float jumpForce = 600f;

    public Transform GroundCheck;
    public LayerMask GroundLayer;
    public LayerMask WaterLayer;
    public float Radius = 0.2f;

    private bool paddling = false;

    private Rigidbody2D body2D;
    private SpriteRenderer sprite;
    private Animator animator;

    public bool m_IsGrounded;
    public bool m_CanPaddle;
    public bool m_IsDie;
    public bool m_Jump;

    private float m_Horizontal;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        body2D = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        m_IsGrounded = Physics2D.OverlapCircle(GroundCheck.position, Radius, GroundLayer);
        body2D.velocity = new Vector2(m_Horizontal * speed, body2D.velocity.y);
        m_IsDie = Physics2D.OverlapCircle(GroundCheck.position, Radius, WaterLayer);
    }

    void Update()
    {
        m_Horizontal = Input.GetAxis("Horizontal");
        paddling = Input.GetButton("Paddle");

        if (Input.GetButtonDown("Jump") && m_IsGrounded)
        {
            m_Jump = true;
            body2D.AddForce(new Vector2(0f, jumpForce));
        }

        flip();
        playerAnimation();
    }

    public void flip()
    {
        if ((m_Horizontal > 0 && sprite.flipX == true) || (m_Horizontal < 0 && sprite.flipX == false))
        {
            sprite.flipX = !sprite.flipX;
        }
    }

    private void playerAnimation()
    {
        animator.SetBool("Grounded", m_IsGrounded);
        animator.SetBool("Die",m_IsDie);
        animator.SetFloat("Velocity", Mathf.Abs(body2D.velocity.x));
        animator.SetBool("Remar",  paddling && m_CanPaddle);

        if (m_Jump)
        {
            animator.SetTrigger("Jump");
            m_Jump = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boat"))
        {
            Debug.Log("Over boat");
            m_CanPaddle = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Boat"))
        {
            Debug.Log("Exit boat");
            m_CanPaddle = false;
        }
    }

}
