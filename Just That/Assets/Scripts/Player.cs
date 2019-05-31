using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

public class Player : MonoBehaviour
{
    #region Propriedades

    public float speed = 10f;
    public float jumpForce = 600f;

    public Transform GroundCheck;
    public LayerMask GroundLayer;
    public LayerMask WaterLayer;
    public float Radius = 0.2f;
    public AudioSource WalkSound;
    public GameObject SubMenuCanvas;

    private float horizontal;
    private bool paddling = false;

    private bool isGrounded;
    private bool isDie;
    private bool canPaddle;
    private bool jump;

    private Rigidbody2D body2D;
    private SpriteRenderer sprite;
    private Animator animator;

    bool playerIndexSet = false;
    PlayerIndex playerIndex = (PlayerIndex)0;
    GamePadState state;
    GamePadState prevState;

    #endregion

    void Start()
    {
        body2D = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        SubMenuCanvas.SetActive(false);
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, Radius, GroundLayer);
        body2D.velocity = new Vector2(horizontal * speed, body2D.velocity.y);
        isDie = Physics2D.OverlapCircle(GroundCheck.position, Radius, WaterLayer);
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        paddling = Input.GetButton("Paddle") && canPaddle;

        Debug.Log("isGrounded: " + isGrounded);
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jump = true;
            body2D.AddForce(new Vector2(0f, jumpForce));
        }

        // Mover quando remar
        if (paddling && this.transform.parent != null)
        {
            float y = sprite.flipX ? -(1 * 0.2f) : (1 * 0.2f);
            this.transform.parent.position = this.transform.parent.position + new Vector3(y, 0);
        }

        Flip();
        PlayerAnimation();

        if (isDie)
        {
            Debug.Log("MORRI");
            //GamePad.SetVibration(playerIndex, 1, 0);
            //Debug.Log("Entrando");
            StopAllCoroutines();
            StartCoroutine(Die(0.05f));
            //Die(5);
            Debug.Log("Saindo");
            //GamePad.SetVibration(playerIndex, 0, 0);
        }
    }

    public IEnumerator Die(float delay)
    {
        GamePad.SetVibration(playerIndex, 0, (float)0.3);
        yield return new WaitForSeconds(delay);
        GamePad.SetVibration(playerIndex, 0, 0);
        //StopAllCoroutines();
        //SubMenuCanvas.SetActive(true);

        SceneManager.LoadScene("Menu");
    }

    public void Flip()
    {
        if ((horizontal > 0 && sprite.flipX == true) || (horizontal < 0 && sprite.flipX == false))
        {
            sprite.flipX = !sprite.flipX;
        }
    }

    private void PlayerAnimation()
    {
        animator.SetBool("Grounded", isGrounded);
        animator.SetBool("Die",isDie);
        animator.SetFloat("Velocity", Mathf.Abs(body2D.velocity.x));
        animator.SetBool("Remar",  paddling);

        if (jump)
        {
            animator.SetTrigger("Jump");
            jump = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Tag: " + collision.tag);
        if (collision.CompareTag("Boat"))
        {
            Debug.Log("Over boat");
            canPaddle = true;
            this.transform.parent = collision.transform;
        }
        if (collision.CompareTag("Hit"))
        {
            isDie = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Boat"))
        {
            Debug.Log("Exit boat");
            canPaddle = false;
            this.transform.parent = null;
        }
    }

    public void walk()
    {
        WalkSound.Play();
    }

}
