using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GhostProjectile : MonoBehaviour
{
    private PlayerController player;
    private Vector3 direction;
    [SerializeField] float speed = 3f;
    [SerializeField] float lifetime = 15f;
    [SerializeField] float damage = 3f;
    private float timer;
    private Shield_Script shield;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        shield = FindObjectsOfType<Shield_Script>(true).FirstOrDefault();
        direction = (player.transform.position - transform.position).normalized;
        var rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime) { Destroy(gameObject); }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.collider.tag)
        {
            case "Shield":
                shield.GetDamage(damage);
                break;
            case "Player":
                player.GetDamage(damage);
                break;
        }
        Destroy(gameObject);
    }
}
