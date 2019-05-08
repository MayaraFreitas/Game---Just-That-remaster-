using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Propriedades

    public float speed = 10f;
    public float jumpForce = 600f;

    public Transform[] GroundCheck;
    public LayerMask GroundLayer;
    public LayerMask BoatLayer;
    public float Radius = 0.2f;

    private bool paddling = false;

    private Rigidbody2D body2D;
    private SpriteRenderer sprite;
    private Animator animator;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        body2D = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        movimentar();
    }

    private void movimentar()
    {
        // Movimentar horizontalmente
        float horizontal = Input.GetAxis("Horizontal");
        body2D.velocity = new Vector2(horizontal * speed, body2D.velocity.y);
        flip(horizontal);

        // Pular
        if (Input.GetButtonDown("Jump") && canJump())
        {
            body2D.AddForce(new Vector2(0f, jumpForce));
        }
        
        // Remar
        if (Input.GetButton("Paddle") && canPaddle())
        {
            paddling = true;
        }
        
        // Aplicar animação
        playerAnimation();
    }

    public void flip(float horizontal)
    {
        if ((horizontal > 0 && sprite.flipX == true) || (horizontal < 0 && sprite.flipX == false))
        {
            sprite.flipX = !sprite.flipX;
        }
    }

    private void playerAnimation()
    {
        //print("Player Die: " + isDie);
        /*if (isDie)
        {
            animator.Play("die");
        }
        else */
        if (paddling)
        {
            animator.Play("remo");
            paddling = false;
        }
        else if (body2D.velocity.x == 0 && body2D.velocity.y == 0)
        {
            animator.Play("idle");
        }
        else if (body2D.velocity.x != 0 && body2D.velocity.y == 0)
        {
            animator.Play("walk");
        }
        else if (body2D.velocity.y != 0)
        {
            animator.Play("jump");
        }
    }

    private bool canJump()
    {
        foreach (Transform t in GroundCheck)
        {
            if (Physics2D.OverlapCircle(t.position, Radius, GroundLayer) || Physics2D.OverlapCircle(t.position, Radius, BoatLayer))
            {
                return true;
            }
        }
        return false;
    }

    private bool canPaddle()
    {
        Transform groundCheck = GroundCheck.Where(g => g.transform.name == "GroundCheck0").First();
        if (Physics2D.OverlapCircle(groundCheck.position, Radius, BoatLayer))
        {
            return true;
        }
        return false;
    }
}
