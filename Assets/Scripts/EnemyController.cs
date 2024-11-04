using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float HP = 20;
    [SerializeField] float knockbackForce = 0.5f;
    Rigidbody2D rb;
    [SerializeField] PlayerController player;



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
        switch (other.tag)
        {
            case "Weapon":
                Vector2 difference = (transform.position - other.transform.position).normalized;
                Vector2 force = difference * knockbackForce;
                rb.AddForce(force, ForceMode2D.Impulse);
                GetDamage(player.MeeleDamage);
                break;
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
