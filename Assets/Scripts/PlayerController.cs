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

    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        FlipCharacter();

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            rb.velocity = new Vector2 (rb.velocity.x, jumpPower);
            isJumping = true;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalInput * movementSpeed, rb.velocity.y);
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
}
