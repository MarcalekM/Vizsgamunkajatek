using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] int HP = 10;
    [SerializeField] float knockbackForce = 0.5f;
    Rigidbody2D rb;
    [SerializeField] PlayerController  player;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Weapon" || other.tag == "Magic")
        {
            Vector2 difference = (transform.position - other.transform.position).normalized;
            if (other.tag == "Magic") knockbackForce *= 2;
            Vector2 force = difference * knockbackForce;
            rb.AddForce(force, ForceMode2D.Impulse);
            Debug.Log(player.MeeleDamage);
            if (other.tag == "Weapon") GetDamage(player.MeeleDamage);
        }
    }

    public void GetDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0) MakeDead();
    }

    private void MakeDead()
    {
        Destroy(gameObject);
    }
}
