using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public float HP = 50;
    [SerializeField] public float damage = 20;
    [SerializeField] public float lenghtToFloor = 0.5f;
    [SerializeField] public float speed = 10f;
    
    private Rigidbody2D rb;
    private PlayerController player;
    
    private float timer = 0f; 
    
    public bool PlayerSpotted = false;
    private Vector2 direction = Vector2.left; 
    public bool isFacingRight = false;


    // Start is called before the first frame update
    public virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        timer += Time.deltaTime;
        HandleMovement();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Shield":
                other.gameObject.GetComponent<Shield_Script>().GetDamage(damage);
                break;
            case "Player":
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

    protected virtual void HandleMovement()
    {
        Debug.Log(IsGrounded());
        if (!PlayerSpotted && IsGrounded())
        {
            if (timer >= 3f){
                direction = -direction;
                Flip();
                timer = 0f;
            }
        }
    }

    protected virtual void FixedUpdate()
    {
        if (IsGrounded(transform.position + new Vector3(direction.x, direction.y, 0) * speed))
        rb.velocity = direction * speed;
    }
    protected virtual void Flip()
    {
        transform.localScale *= -1;
        isFacingRight = !isFacingRight;
    }

    protected virtual void MakeDead()
    {
        player.kills++;
        Destroy(gameObject);
    }
    public bool IsGrounded(Vector3? nextPosition = null) {
        RaycastHit hit;
        if (Physics.Raycast(nextPosition ?? transform.position, Vector3.down, out hit, lenghtToFloor)) {
            return true;
        }
        return false;
    }

}