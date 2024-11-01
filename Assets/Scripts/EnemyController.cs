using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float knockbackForce = 0.5f;
    Rigidbody2D rb;


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
        }
    }
}
