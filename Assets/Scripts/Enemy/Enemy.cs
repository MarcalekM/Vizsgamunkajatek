using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float HP = 50;
    [SerializeField] float knockbackForce = 25f;
    [SerializeField] float damage = 20;
    
    private Rigidbody2D rb;
    private PlayerController player;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollision2D(Collider2D other)
    {
        if (!gameObject.tag.Equals("Enemy")){
            switch (other.tag)
            {
                case "Weapon":
                    Vector2 difference = (transform.position - other.transform.position).normalized;
                    Vector2 force = difference * knockbackForce;
                    rb.AddForce(force, ForceMode2D.Impulse);
                    GetDamage(player.MeeleDamage, 0);
                    break;
                case "Shield":
                    other.gameObject.GetComponent<Shield_Script>().GetDamage(damage);
                    break;
                case "Player":
                    other.gameObject.GetComponent<PlayerController>().GetDamage(damage);
                    break;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag.Equals("Magic2")) GetDamage(0, player.MagicDamage * 0.05f);
    }

    virtual public void GetDamage(float damageTaken, float magicDamageTaken)
    {
        HP -= damageTaken;
        HP -= magicDamageTaken;
        if (HP <= 0) MakeDead();
    }

    virtual protected void MakeDead()
    {
        player.kills++;
        Destroy(gameObject);
    }

}