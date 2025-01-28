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
    private float healthBarTargetFillAmount = 1f;
    private BossStage2 stage2;
    private bool finishedSummoning = false;
    private float summonTimer = 0f;

    public override void Start()
    {
        base.Start();
        PlayerSpotted = true;
        stage2 = FindObjectOfType<BossStage2>();
    }

    public override void Update()
    {
        base.Update();
        healthbar.fillAmount = Mathf.Lerp(healthbar.fillAmount, healthBarTargetFillAmount, Time.deltaTime);
        if (phase == 2 && !stage2.Stage2)
        {
            stage2.Stage2 = true;
            _animator.SetTrigger("Summon");
        }

        if (phase == 2 && !finishedSummoning)
        {
            summonTimer += Time.deltaTime;
            if (summonTimer > 3.0f)
            {
                _animator.SetBool("Stage2", true);
                finishedSummoning = true;
            }
        }
    }

    public override void GetDamage(float damageTaken, float magicDamageTaken)
    {
        HP -= damageTaken;
        HP -= magicDamageTaken;
        if (healthbar is not null)
        {
            healthBarTargetFillAmount = Normalize(HP, 0,maxHP, 0, 1);
        }
        if (HP <= maxHP/2) phase = 2;
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
        AttackTimer += Time.deltaTime;
        switch (phase)
        {
            case 1:
                if (AttackTimer > attackFrequency)
                {
                    Instantiate(projectilePrefab, transform.position - new Vector3(direction.x, direction.y, 0) * 2, Quaternion.identity);
                    AttackTimer = 0f;
                }
                break;
            case 2:
                if (AttackTimer > attackFrequency && finishedSummoning)
                {
                    _animator.SetTrigger("Stage2Attack");
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
    
    float Normalize(float val, float valmin, float valmax, float min, float max) 
    {
        return (((val - valmin) / (valmax - valmin)) * (max - min)) + min;
    }

    protected override void MakeDead()
    {
        player.kills++;
        healthbar.fillAmount = 0f;
        Destroy(gameObject);
    }
}
