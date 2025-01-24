using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField] public float HP = 50;
    [SerializeField] public float damage = 20;
    [SerializeField] public float hSpeed = 1f;
    [SerializeField] public float vSpeed = 1f;

    protected Animator _animator;
    
    protected Rigidbody2D rb;
    protected PlayerController player;
    protected Collider2D Collider;
    
    protected float timer = 0f; 
    
    public bool PlayerSpotted = false;
    protected Vector2 direction = Vector2.left; 
    public bool isFacingRight = false;


    // Start is called before the first frame update
    public virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        Collider = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        timer += Time.deltaTime;
        HandleMovement();
        AttackHandler();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Shield":
                other.gameObject.GetComponent<Shield_Script>().GetDamage(damage);
                break;
            case "Player":
                if (_animator is not null) _animator.SetTrigger("Attack");
                other.gameObject.GetComponent<PlayerController>().GetDamage(damage);
                break;
        }
    }

    virtual public void GetDamage(float damageTaken, float magicDamageTaken)
    {
        HP -= damageTaken;
        HP -= magicDamageTaken;
        if (HP <= 0) MakeDead();
    }

    virtual protected void AttackHandler()
    {
        
    }
    
    protected virtual void HandleMovement()
    {
        if (!PlayerSpotted && IsGrounded())
        {
            if (timer >= 3f){
                direction = -direction;
                Flip();
                timer = 0f;
            }
        }
        else
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
        isFacingRight = !isFacingRight;
    }

    protected virtual void MakeDead()
    {
        player.kills++;
        Destroy(gameObject);
    }
    public bool IsGrounded(Vector2? nextPosition = null) {
        List<RaycastHit2D> hits = new();
        var lenghtToFloor = (Collider.bounds.size.y / 2) + 0.5f;
        var filter = new ContactFilter2D();
        filter.NoFilter();
        filter.layerMask = LayerMask.GetMask("Enemy");
        //Debug.DrawRay(nextPosition ?? transform.position, Vector2.down * lenghtToFloor, Color.green, 1, false);
        var hit = Physics2D.Raycast(nextPosition ?? new Vector2(transform.position.x, transform.position.y),
            Vector2.down, filter, hits, lenghtToFloor);
        if (hit > 0) return true;
        return false;
    }

}