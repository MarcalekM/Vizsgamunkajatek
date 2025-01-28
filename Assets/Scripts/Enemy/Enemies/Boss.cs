using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField] private float playerFollowDistance = 12f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float attackFrequency = 2f;
    private float AttackTimer = 0f;
    public override void GetDamage(float damageTaken, float magicDamageTaken)
    {
        HP -= damageTaken;
        HP -= magicDamageTaken;
        healthbar.rectTransform.anchorMax = new Vector2(HP / maxHP, healthbar.rectTransform.anchorMax.y);
        if (HP <= 0) MakeDead();
    }
    
    protected override void MovementHandler()
    {
        MovementTimer += Time.deltaTime;
        if (GetDistanceToPlayer() < playerFollowDistance) direction.Set(-direction.x, direction.y);
    }
    
    protected override void ApplyMovement()
    {
        var force = direction * 0;
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
