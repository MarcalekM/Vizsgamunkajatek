using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Enemy
{
    [SerializeField] private float playerFollowDistance = 12f;
    [SerializeField] private float preferredDistanceFromGround = 6f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float attackFrequency = 2f;
    private float AttackTimer = 0f;
    public override void GetDamage(float damageTaken, float magicDamageTaken)
    {
        HP -= magicDamageTaken;
        if (HP <= 0) MakeDead();
    }
    
    protected override void MovementHandler()
    {
        MovementTimer += Time.deltaTime;
        if (!PlayerSpotted)
        {
            if (MovementTimer >= 3f){
                Flip();
                MovementTimer = 0f;
            }
        }
        else if (PlayerSpotted)
        {
            direction = (player.transform.position - transform.position).normalized;
        }
        if (GetDistanceToGround() < preferredDistanceFromGround) direction.Set(direction.x, 1f);
        if (GetDistanceToPlayer() < playerFollowDistance) direction.Set(-direction.x, direction.y);
    }
    
    protected override void ApplyMovement()
    {
        var force = direction * hSpeed;
        rb.velocity = Vector2.Lerp(rb.velocity, force, Time.deltaTime * 10f);
    }
    
    public float GetDistanceToGround() {
        //Debug.DrawRay(nextPosition ?? transform.position, Vector2.down * 20, Color.green, 1, false);
        var hit = Physics2D.Raycast(
            transform.position,
            Vector2.down,
            20,
            LayerMask.GetMask("Ground"));
        if (hit) return hit.distance;
        return -1;
    }

    public float GetDistanceToPlayer()
    {
        return (transform.position - player.transform.position).magnitude;
    }

    protected override void AttackHandler()
    {
        AttackTimer += Time.deltaTime;
        if (PlayerSpotted && AttackTimer > attackFrequency)
        {
            Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            AttackTimer = 0f;
        }
    }
}
