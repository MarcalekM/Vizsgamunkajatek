using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using UnityEngine.PlayerLoop;

public class Enemy : MonoBehaviour
{
    protected float maxHP;
    [SerializeField] public UnityEngine.UI.Image healthbar;
    [SerializeField] public float HP = 50;
    [SerializeField] public float damage = 20;
    [SerializeField] public float hSpeed = 1f;
    [SerializeField] public float vSpeed = 1f;
    [SerializeField] protected bool useAttackHandler = true;
    [SerializeField] public GameObject deathFX;

    protected Animator _animator;
    
    protected Rigidbody2D rb;
    protected PlayerController player;
    protected Shield_Script shield;
    protected Collider2D MainCollider;
    
    protected float MovementTimer = 0f; 
    
    public bool PlayerSpotted = false;
    protected Vector2 direction = Vector2.left; 
    public bool isFacingRight = false;
    protected float DeltaMovementTime = 0f;



    // Start is called before the first frame update
    public virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        shield = FindObjectsOfType<Shield_Script>(true).FirstOrDefault();
        rb = GetComponent<Rigidbody2D>();
        MainCollider = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
        maxHP = HP;
        if (healthbar is not null)
            healthbar.rectTransform.anchorMax = new Vector2(HP / maxHP, healthbar.rectTransform.anchorMax.y);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        MovementHandler();
        if (PlayerSpotted && useAttackHandler)
            AttackHandler();
    }

    public virtual void GetDamage(float damageTaken, float magicDamageTaken)
    {
        HP -= damageTaken;
        HP -= magicDamageTaken;
        if (healthbar is not null)
            healthbar.rectTransform.anchorMax = new Vector2(HP / maxHP, healthbar.rectTransform.anchorMax.y);
        
        if (HP <= 0) MakeDead();
    }

    protected virtual void AttackHandler()
    {
        
    }
    
    protected virtual void MovementHandler()
    {
        MovementTimer += Time.deltaTime;
        if (!PlayerSpotted && IsGrounded())
        {
            if (MovementTimer >= 3f){
                Flip();
                MovementTimer = 0f;
            }
        }
        else if (PlayerSpotted)
        {
            direction = (player.transform.position - transform.position).normalized;
        }
    }

    protected virtual void FixedUpdate()
    {
        ApplyMovement();
    }

    protected virtual void ApplyMovement()
    {
        if (IsGrounded(transform.position + new Vector3(direction.x, direction.y, 0) * hSpeed) || PlayerSpotted)
        {
            var force = direction * hSpeed;
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, force.x, Time.deltaTime * 100), rb.velocity.y);
        }
    }
    protected virtual void Flip()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        if (healthbar is not null)
            healthbar.transform.parent.localScale = new Vector2(healthbar.transform.parent.localScale.x * -1, healthbar.transform.parent.localScale.y);
        direction = -direction;
        isFacingRight = !isFacingRight;
    }
    
    protected virtual void MakeDead()
    {
        player.kills++;
        Destroy(gameObject);
        Instantiate(deathFX, transform.position, deathFX.transform.localRotation);
    }
    public bool IsGrounded(Vector2? nextPosition = null) {
        var lenghtToFloor = (MainCollider.bounds.size.y / 2) + 0.5f;
        //Debug.DrawRay(nextPosition ?? transform.position, Vector2.down * lenghtToFloor, Color.green, 1, false);
        var hit = Physics2D.Raycast(
            nextPosition ?? transform.position,
            Vector2.down,
            lenghtToFloor,
            LayerMask.GetMask("Ground"));
        if (hit) return true;
        return false;
    }

}