using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SummonFireball : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 1f;
    public float damage = 0;
    public float knockbackForce = 0;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (gameObject.name.Contains("EnemyProjectile"))
        {
            Vector2 targetPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
            Vector2 currentPosition = transform.position;
            Vector2 direction = (targetPosition - currentPosition).normalized;
            rb.velocity = direction * 8;
        }
        else
        {
            rb.AddForce(new Vector2(transform.rotation.z == 0 ? 1 : -1, 0) * projectileSpeed, ForceMode2D.Impulse);
        }

        if (gameObject.name.Contains("Fireball")) damage = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().MagicDamage;
        else if (gameObject.name.Contains("EnemyProjectile")) damage = 30;
        else if(gameObject.name.Contains("Blow")) knockbackForce = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().MagicDamage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Enemy":
                if (other.gameObject.name.Contains("Ghost")) other.gameObject.GetComponent<EnemyController>().GetDamage(damage * 1.5f);
                else if (other.gameObject.name.Contains("Golem")) other.gameObject.GetComponent<EnemyController>().GetDamage(damage * 0.5f);
                else other.gameObject.GetComponent<EnemyController>().GetDamage(damage);

                Vector2 difference = (other.transform.position - transform.position).normalized;
                Vector2 force = difference * knockbackForce;
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
                break;
            case "Shield":
                if (gameObject.name.Contains("EnemyProjectile")) other.gameObject.GetComponent<Shield_Script>().GetDamage(damage);
                break;
            case "Player":
                if (gameObject.name.Contains("EnemyProjectile")) other.gameObject.GetComponent<PlayerController>().GetDamage(damage);
                break;
        }
        if(!other.tag.Equals("Finder"))Destroy(gameObject);
    }

    public void Stop()
    {
        transform.localRotation = Quaternion.Euler(0, 0, -90);
        rb.velocity = new(0, 0);
    }

    

}
