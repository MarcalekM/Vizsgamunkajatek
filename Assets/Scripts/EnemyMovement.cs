using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 5f; // Mozgási sebesség
    private Vector2 direction = Vector2.right; // Kezdeti irány
    private float timer = 0f; // Idõzítõ
    private Rigidbody2D rb;

    private void Start()
    {
        // Megkeressük a Rigidbody2D komponenst
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Növeljük az idõzítõt a deltaTime-mal (az elõzõ frame óta eltelt idõ)
        timer += Time.deltaTime;

        // Ha eltelt 5 másodperc, megfordítjuk az irányt és lenullázzuk az idõzítõt
        if (timer >= 5f)
        {
            direction = -direction;
            timer = 0f;
        }
    }

    private void FixedUpdate()
    {
        // Mozgatjuk az objektumot a Rigidbody2D-vel az aktuális irányban
        rb.velocity = direction * speed;
    }
}
