using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float horizontalInput;
    [SerializeField] float movementSpeed = 5f;
    bool isFacingRight = true;
    [SerializeField] float jumpPower = 5f;
    bool isJumping = false;

    [SerializeField] private Transform Magic;
    [SerializeField] private GameObject fireball;
    [SerializeField] private float fireRate = .5f;

    Rigidbody2D rb;
    public Animator animator;
    private float nextFire;

    [SerializeField] Transform Shield;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        nextFire = 0f;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        FlipCharacter();

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            isJumping = true;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && !Input.GetKey(KeyCode.Q))
        {
            MeeleAttack();
        }

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalInput * movementSpeed, rb.velocity.y);
        
        if (Time.time >= nextFire && Input.GetKeyDown(KeyCode.Mouse1) && !Input.GetKey(KeyCode.Q))
        {
            nextFire = Time.time + fireRate;
            Instantiate(fireball, Magic.position,
                Quaternion.Euler(x: 0, y: 0, z: isFacingRight ? 0 : 180));
        }

        if (Input.GetKey(KeyCode.Q)) Shield.gameObject.SetActive(true);
        else Shield.gameObject.SetActive(false);
    }


    void FlipCharacter()
    {
        if (isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Ground")) isJumping = false;
    }

    void MeeleAttack()
    {
        animator.SetTrigger("MeeleAttack");
    }
}
