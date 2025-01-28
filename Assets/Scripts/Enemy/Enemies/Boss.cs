using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float attackFrequency = 2f;
    private float AttackTimer = 0f;
    private int phase = 1;

    public override void Start()
    {
        base.Start();
        PlayerSpotted = true;
    }

    public override void GetDamage(float damageTaken, float magicDamageTaken)
    {
        HP -= damageTaken;
        HP -= magicDamageTaken;
        if (healthbar is not null)
            healthbar.rectTransform.anchorMax = new Vector2(HP / maxHP, healthbar.rectTransform.anchorMax.y);
        if (HP <= 0) MakeDead();
    }
    
    protected override void MovementHandler()
    {
        MovementTimer += Time.deltaTime;
        direction = (transform.position - player.transform.position).normalized;
    }
    
    protected override void ApplyMovement()
    {
        var force = direction * 0;
        if (direction.x > 0f) FlipVisual(true);
        else if (direction.x < 0f) FlipVisual(false);
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
        switch (phase)
        {
            case 1:
                AttackTimer += Time.deltaTime;
                if (AttackTimer > attackFrequency)
                {
                    Instantiate(projectilePrefab, transform.position - new Vector3(direction.x, direction.y, 0) * 2, Quaternion.identity);
                    AttackTimer = 0f;
                }
                break;
        }
       
    }
    void FlipVisual(bool right)
    {
        isFacingRight = right;
        Vector3 ls = transform.localScale;
        //if (Math.Abs(ls.x) > ls.x && isFacingRight) ls.x *= -1;
        //else if (Mathf.Approximately(Math.Abs(ls.x), ls.x) && !isFacingRight) ls.x *= -1;
        if (!right && transform.rotation.eulerAngles.y == 0)transform.RotateAround(transform.parent.position, Vector3.up, 180);
        else if (right && transform.rotation.eulerAngles.y == 180)transform.RotateAround(transform.parent.position, Vector3.up, 180);
        //transform.localScale = ls;
    }
}
