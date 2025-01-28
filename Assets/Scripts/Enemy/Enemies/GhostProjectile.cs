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
    bool isFacingRight = true;
    private Shield_Script shield;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        shield = FindObjectsOfType<Shield_Script>(true).FirstOrDefault();
        direction = (player.transform.position - transform.position).normalized;
        if (direction.x < 0f) FlipVisual(false);
        else if (direction.x > 0f) FlipVisual(true);
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
    
    void FlipVisual(bool right)
    {
        isFacingRight = right;
        Vector3 ls = transform.localScale;
        if (Math.Abs(ls.x) > ls.x && isFacingRight) ls.x *= -1;
        else if (Mathf.Approximately(Math.Abs(ls.x), ls.x) && !isFacingRight) ls.x *= -1;
        transform.localScale = ls;
    }
}
