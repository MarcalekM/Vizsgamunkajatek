using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SummonFireball : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 1f;
    public float damage = 0;

    private Rigidbody2D rigidbody2d;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();

        rigidbody2d.AddForce(new Vector2(transform.rotation.z == 0 ? 1 : -1, 0) * projectileSpeed, ForceMode2D.Impulse);

        if (gameObject.name.Contains("Fireball")) damage = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().MagicDamage;
        else if (gameObject.name.Contains("EnemyProjectile")) damage = 30;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Enemy":
                other.gameObject.GetComponent<EnemyController>().GetDamage(damage);
                break;
            case "Shield":
                other.gameObject.GetComponent<Shield_Script>().GetDamage(damage);
                break;
            case "Player":
                other.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
                break;
        }
        Destroy(gameObject);
    }

    public void Stop()
    {
        transform.localRotation = Quaternion.Euler(0, 0, -90);
        rigidbody2d.velocity = new(0, 0);
    }

    

}
