using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Goblin : Enemy
{
    [SerializeField] private float attackRange = 1.2f;
    [SerializeField] private float attackFrequency = 0.5f;
    private float damageTimer = 0f;
    protected override void ApplyMovement()
    {
        if (IsGrounded(transform.position + new Vector3(direction.x, direction.y, 0) * hSpeed) || PlayerSpotted)
        {
            var force = direction * hSpeed;
            if (PlayerSpotted && rb.velocity.magnitude < 0.001)
            {
                rb.velocity = new Vector2(rb.velocity.x, hSpeed);
            }
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, force.x, Time.deltaTime * 100), rb.velocity.y);
            
        }
    }

    protected override void AttackHandler()
    {
        damageTimer += Time.deltaTime;
        if (damageTimer < attackFrequency) return;
        List<RaycastHit2D> hits = new();
        Debug.DrawRay(transform.position, direction * attackRange, Color.green, 1, false);
        var filter = new ContactFilter2D();
        filter.NoFilter();
        var hitCount = Physics2D.Raycast(transform.position,
            direction, filter, hits, attackRange);
        if (hitCount > 0)
        {
            foreach (var hit in hits)
            {
                switch (hit.collider.tag)
                {
                    case "Shield":
                        if (_animator is not null) _animator.SetTrigger("Attack");
                        shield.GetDamage(damage);
                        damageTimer = 0;
                        break;
                    case "Player":
                        if (_animator is not null) _animator.SetTrigger("Attack");
                        player.GetDamage(damage);
                        damageTimer = 0;
                        break;
                }
            }
        }
    }
}
