using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private float HP = 50;
    [SerializeField] float knockbackForce = 25f;
    Rigidbody2D rb;
    [SerializeField] PlayerController player;
    private float damage = 20;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0) MakeDead();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!gameObject.tag.Equals("Enemy")){
            switch (other.tag)
            {
                case "Weapon":
                    Vector2 difference = (transform.position - other.transform.position).normalized;
                    Vector2 force = difference * knockbackForce;
                    rb.AddForce(force, ForceMode2D.Impulse);
                    GetDamage(player.MeeleDamage);
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

    public void GetDamage(float damage)
    {
            HP -= damage;
    }

    private void MakeDead()
    {
        player.kills++;
        Destroy(gameObject);
    }

}
