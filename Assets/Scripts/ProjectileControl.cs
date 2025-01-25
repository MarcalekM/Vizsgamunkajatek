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
        rb.AddForce(new Vector2(transform.rotation.z == 0 ? 1 : -1, 0) * projectileSpeed, ForceMode2D.Impulse);
        if (gameObject.name.Contains("Fireball")) damage = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().MagicDamage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Enemy":
                other.gameObject.GetComponent<Enemy>().GetDamage(0, damage);

                Vector2 difference = (other.transform.position - transform.position).normalized;
                Vector2 force = difference * knockbackForce;
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
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
