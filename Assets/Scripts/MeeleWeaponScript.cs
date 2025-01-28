using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleWeaponScript : MonoBehaviour
{
    [SerializeField] private float knockbackForce = 25f;
    private PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                StartCoroutine(DealDamage(collision));
                break;
        }
    }

    IEnumerator DealDamage(Collider2D collision)
    {
        yield return new WaitForSeconds(0.4f);
        try
        {
            Vector2 difference = (transform.position - collision.transform.position).normalized;
            Vector2 force = difference * knockbackForce;
            var rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.AddForce(force, ForceMode2D.Impulse);
            var enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.GetDamage(player.MeeleDamage, 0);
        }
        catch (UnityEngine.MissingReferenceException e)
        {
                Debug.Log(e);
        }
    }
}
