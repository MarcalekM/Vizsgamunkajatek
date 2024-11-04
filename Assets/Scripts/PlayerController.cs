using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Lv = 1;
    [SerializeField] public float HP = 10;
    [SerializeField] public float MeeleDamage = 10;
    [SerializeField] public float MagicDamage = 12;
    public float kills = 0;
    public float SP = 3;

    float horizontalInput;
    [SerializeField] float movementSpeed = 15f;
    bool isFacingRight = true;
    [SerializeField] float jumpPower = 8f;
    bool isJumping = false;

    [SerializeField] private Transform Magic;
    [SerializeField] private GameObject fireball;
    [SerializeField] private float fireRate = .5f;

    Rigidbody2D rb;
    public Animator animator;
    private float nextFire;

    [SerializeField] Transform Shield;
    public float MaxShield = 10;
    public float ShieldHP = 10;
    public bool ShieldActive = false;
    public bool ShieldAlive = true;

    [SerializeField] TextMeshProUGUI UI_HP;
    [SerializeField] TextMeshProUGUI UI_Kill;
    [SerializeField] TextMeshProUGUI UI_SP;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        nextFire = 0f;
    }

    private void Update()
    {
        UI_HP.text = "HP: " + HP;
        UI_Kill.text = "Kills: " + kills;
        UI_SP.text = "SP: " + SP;
        horizontalInput = Input.GetAxis("Horizontal");

        FlipCharacter();

        if (Input.GetButtonDown("Jump") && !isJumping) Jump();

        if (Input.GetKeyDown(KeyCode.Mouse0) && !ShieldActive) MeeleAttack();

        if (Input.GetKey(KeyCode.LeftShift)) movementSpeed = 10;
        else movementSpeed = 5;

        rb.velocity = new Vector2(horizontalInput * movementSpeed, rb.velocity.y);

        if (Time.time >= nextFire && Input.GetKeyDown(KeyCode.Mouse1) && !ShieldActive) SummonFireball();

        if(ShieldAlive){
            if (Input.GetKey(KeyCode.Q)) ActivateShield();
            else DeactivateShield();
        }
        else DeactivateShield();

        if(HP <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (!ShieldAlive) AddShieldHP();
        if (ShieldAlive && !ShieldActive && ShieldHP != MaxShield) AddShieldHP();
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

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        isJumping = true;
    }

    void MeeleAttack()
    {
        animator.SetTrigger("MeeleAttack");
    }

    void SummonFireball()
    {
        nextFire = Time.time + fireRate;
        Instantiate(fireball, Magic.position,
            Quaternion.Euler(x: 0, y: 0, z: isFacingRight ? 0 : 180));
    }

    void ActivateShield()
    {
        Shield.gameObject.SetActive(true);
        ShieldActive = true;
    }

    void DeactivateShield()
    {
        Shield.gameObject.SetActive(false);
        ShieldActive = false;
    }

    void AddShieldHP()
    {
        if (ShieldHP + MagicDamage / 100 > MaxShield) ShieldHP = MaxShield;
        else ShieldHP += MagicDamage / 100;
        if (ShieldHP == MaxShield) ShieldAlive = true;
    }

    public void TakeDamage(float damage)
    {
        if (HP - damage < 0) HP -= damage;
        else HP = 0;
    }
}
