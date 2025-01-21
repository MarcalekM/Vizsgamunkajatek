using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public float HP = 50;
    [SerializeField] public float damage = 20;
    
    private Rigidbody2D rb;
    private PlayerController player;


    // Start is called before the first frame update
    public virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Shield":
                other.gameObject.GetComponent<Shield_Script>().GetDamage(damage);
                break;
            case "Player":
                other.gameObject.GetComponent<PlayerController>().GetDamage(damage);
                break;
        }
    }

    virtual public void GetDamage(float damageTaken, float magicDamageTaken)
    {
        HP -= damageTaken;
        HP -= magicDamageTaken;
        if (HP <= 0) MakeDead();
    }

    virtual protected void FollowPlayer()
    {
        
    }
    virtual protected void MakeDead()
    {
        player.kills++;
        Destroy(gameObject);
    }

}